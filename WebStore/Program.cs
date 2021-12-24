using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Infrastructure.Conventions;
using WebStore.Infrastructure.Middleware;
using WebStore.Services;
using WebStore.Services.InMemory;
using WebStore.Services.InSQL;
using WebStore.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

#region Настройка построителя приложения - определение содержимого

var services = builder.Services;
services.AddControllersWithViews(opt =>
{
    opt.Conventions.Add(new TestConvention());
});

services.AddDbContext<WebStoreDB>(opt => 
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
services.AddTransient<IDbInitializer, DbInitializer>();

services.AddSingleton<IEmployeesData, InMemoryEmployeesData>(); // Singleton - потому что InMemory!
//services.AddSingleton<IProductData, InMemoryProductData>();     // Singleton - потому что InMemory!
services.AddScoped<IProductData, SqlProductData>(); // !!! AddScoped !!!

#endregion

var app = builder.Build(); // Сборка приложения

await using (var scope = app.Services.CreateAsyncScope())
{
    var db_initializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    await db_initializer.InitializeAsync(RemoveBefore: false);
}

//app.Urls.Add("http://+:80"); // - если хочется обеспечить видимость приложения в локальной сети

#region Конфигурирование конвейера обработки входящих соединения

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

//app.Map("/testpath", async context => await context.Response.WriteAsync("Test middleware"));

app.UseStaticFiles();

app.UseRouting();

app.UseMiddleware<TestMiddleware>();

app.UseWelcomePage("/welcome");

//app.MapDefaultControllerRoute();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); 

#endregion

app.Run();
