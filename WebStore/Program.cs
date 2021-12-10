var builder = WebApplication.CreateBuilder(args);

//builder.Configuration.AddCommandLine(args);

var services = builder.Services;
services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

// Загрузка информации из файла конфигурации
//var configuration = app.Configuration;
//var greetings = configuration["CustomGreetings"];

//app.MapGet("/", () => app.Configuration["CustomGreetings"]);
app.MapGet("/throw", () =>
{
    throw new ApplicationException("Ошибка в программе!");
});

app.MapDefaultControllerRoute();

app.Run();
