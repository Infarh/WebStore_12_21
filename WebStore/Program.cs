using WebStore.Infrastructure.Conventions;
using WebStore.Infrastructure.Middleware;

var builder = WebApplication.CreateBuilder(args);

#region Настройка построителя приложения - определение содержимого

var services = builder.Services;
services.AddControllersWithViews(opt =>
{
    opt.Conventions.Add(new TestConvention());
}); 

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

app.UseMiddleware<TestMiddleware>();

app.UseWelcomePage("/welcome");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); 

#endregion

app.Run();
