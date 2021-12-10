var builder = WebApplication.CreateBuilder(args);

//builder.Configuration.AddCommandLine(args);

var app = builder.Build();

// Загрузка информации из файла конфигурации
//var configuration = app.Configuration;
//var greetings = configuration["CustomGreetings"];

app.MapGet("/", () => app.Configuration["CustomGreetings"]);

app.Run();
