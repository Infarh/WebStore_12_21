using WebStore.Infrastructure.Conventions;

var builder = WebApplication.CreateBuilder(args);

#region Настройка построителя приложения - определение содержимого

//builder.Configuration.AddCommandLine(args);

//builder.Logging.AddFilter("Microsoft", LogLevel.Warning);

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

app.UseStaticFiles(/*new StaticFileOptions { ServeUnknownFileTypes = true }*/);

app.UseRouting();

// Загрузка информации из файла конфигурации
//var configuration = app.Configuration;
//var greetings = configuration["CustomGreetings"];

//app.MapGet("/", () => app.Configuration["CustomGreetings"]);
app.MapGet("/throw", () =>
{
    throw new ApplicationException("Ошибка в программе!");
});

//app.MapDefaultControllerRoute();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); 

#endregion

// Запуск приложения

//app.Start(); - не работает! Нужно Run()
app.Run();
