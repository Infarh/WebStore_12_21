using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;using Polly;
using Polly.Extensions.Http;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using Serilog.Formatting.Json;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Hubs;
using WebStore.Infrastructure.Conventions;
using WebStore.Infrastructure.Middleware;
using WebStore.Interfaces.Services;
using WebStore.Interfaces.TestAPI;
using WebStore.Logging;
using WebStore.Services.Services;
using WebStore.Services.Services.InCookies;
using WebStore.WebAPI.Clients.Employees;
using WebStore.WebAPI.Clients.Identity;
using WebStore.WebAPI.Clients.Orders;
using WebStore.WebAPI.Clients.Products;
using WebStore.WebAPI.Clients.Values;

var builder = WebApplication.CreateBuilder(args);

//builder.Logging
//.ClearProviders()
//.AddConsole(opt => opt.LogToStandardErrorThreshold = LogLevel.Information)
//.AddFilter("Microsoft", level => level >= LogLevel.Information)
//.AddFilter<DebugLoggerProvider>((category, level) => category.StartsWith("Microsoft") && level > LogLevel.Debug)
//;

builder.Logging.AddLog4Net();
builder.Host.UseSerilog((host, log) => log.ReadFrom.Configuration(host.Configuration)
   .MinimumLevel.Debug()
   .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
   .Enrich.FromLogContext()
   .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}]{SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}")
   .WriteTo.RollingFile($@".\Logs\WebStore[{DateTime.Now:yyyy-MM-ddTHH-mm-ss}].log")
   .WriteTo.File(new JsonFormatter(",", true), $@".\Logs\WebStore[{DateTime.Now:yyyy-MM-ddTHH-mm-ss}].log.json")
   .WriteTo.Seq("http://localhost:5341/"));

#region Настройка построителя приложения - определение содержимого

var configuration = builder.Configuration;
var services = builder.Services;
services.AddControllersWithViews(opt =>
{
    opt.Conventions.Add(new TestConvention());
});

services.AddIdentity<User, Role>()
   .AddDefaultTokenProviders();

services.AddHttpClient("WebStoreAPIIdentity", client => client.BaseAddress = new(configuration["WebAPI"]))
   .AddTypedClient<IUserStore<User>, UsersClient>()
   .AddTypedClient<IUserRoleStore<User>, UsersClient>()
   .AddTypedClient<IUserPasswordStore<User>, UsersClient>()
   .AddTypedClient<IUserEmailStore<User>, UsersClient>()
   .AddTypedClient<IUserPhoneNumberStore<User>, UsersClient>()
   .AddTypedClient<IUserTwoFactorStore<User>, UsersClient>()
   .AddTypedClient<IUserClaimStore<User>, UsersClient>()
   .AddTypedClient<IUserLoginStore<User>, UsersClient>()
   .AddTypedClient<IRoleStore<Role>, RolesClient>()
   .AddPolicyHandler(GetRetryPolicy());

services.Configure<IdentityOptions>(opt =>
{
#if DEBUG
    opt.Password.RequireDigit = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequiredLength = 3;
    opt.Password.RequiredUniqueChars = 3;
#endif

    opt.User.RequireUniqueEmail = false;
    opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIGKLMNOPQRSTUVWXYZ1234567890";

    opt.Lockout.AllowedForNewUsers = false;
    opt.Lockout.MaxFailedAccessAttempts = 10;
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
});

services.ConfigureApplicationCookie(opt =>
{
    opt.Cookie.Name = "WebStore.GB";
    opt.Cookie.HttpOnly = true;

    //opt.Cookie.Expiration = TimeSpan.FromDays(10); // устарело
    opt.ExpireTimeSpan = TimeSpan.FromDays(10);

    opt.LoginPath = "/Account/Login";
    opt.LogoutPath = "/Account/Logout";
    opt.AccessDeniedPath = "/Account/AccessDenied";

    opt.SlidingExpiration = true;
});

//services.AddSingleton<IEmployeesData, InMemoryEmployeesData>(); // Singleton - потому что InMemory!
//services.AddSingleton<IProductData, InMemoryProductData>();     // Singleton - потому что InMemory!

//services.AddScoped<IEmployeesData, SqlEmployeesData>();
//services.AddScoped<IProductData, SqlProductData>(); // !!! AddScoped !!!
//services.AddScoped<IOrderService, SqlOrderService>();
services.AddScoped<ICartStore, InCookiesCartStore>();
services.AddScoped<ICartService, CartService>();

//services.AddHttpClient<IValuesService, ValuesClient>(client => client.BaseAddress = new(configuration["WebAPI"]));
//services.AddHttpClient<IEmployeesData, EmployeesClient>(client => client.BaseAddress = new(configuration["WebAPI"]));
//services.AddHttpClient<IProductData, ProductsClient>(client => client.BaseAddress = new(configuration["WebAPI"]));
//services.AddHttpClient<IOrderService, OrdersClient>(client => client.BaseAddress = new(configuration["WebAPI"]));

services.AddHttpClient("WebStoreAPI", client => client.BaseAddress = new(configuration["WebAPI"]))
   .AddTypedClient<IValuesService, ValuesClient>()
   .AddTypedClient<IEmployeesData, EmployeesClient>()
   .AddTypedClient<IProductData, ProductsClient>()
   .AddTypedClient<IOrderService, OrdersClient>()
   .AddPolicyHandler(GetRetryPolicy())
   .AddPolicyHandler(GetCircuitBreakerPolicy());

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int MaxRetryCount = 5, int MaxJitterTime = 1000)
{
    var jitter = new Random();
    return HttpPolicyExtensions
       .HandleTransientHttpError()
       .WaitAndRetryAsync(MaxRetryCount, RetryAttempt => 
            TimeSpan.FromSeconds(Math.Pow(2, RetryAttempt)) +
            TimeSpan.FromMilliseconds(jitter.Next(0, MaxJitterTime)));
}

static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() =>
    HttpPolicyExtensions
       .HandleTransientHttpError()
       .CircuitBreakerAsync(handledEventsAllowedBeforeBreaking: 5, TimeSpan.FromSeconds(30));

//services.AddAutoMapper(typeof(Program));
services.AddAutoMapper(Assembly.GetEntryAssembly());

services.AddSignalR();

#endregion

var app = builder.Build(); // Сборка приложения

//app.Urls.Add("http://+:80"); // - если хочется обеспечить видимость приложения в локальной сети

#region Конфигурирование конвейера обработки входящих соединения

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

//app.Map("/testpath", async context => await context.Response.WriteAsync("Test middleware"));

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseMiddleware<TestMiddleware>();

app.UseWelcomePage("/welcome");

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chat");

    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

#endregion

app.Run();
