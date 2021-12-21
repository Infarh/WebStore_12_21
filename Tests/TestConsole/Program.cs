using Microsoft.Extensions.DependencyInjection;

using TestConsole.Data;
using TestConsole.Services;
using TestConsole.Services.Interfaces;

var service_collection = new ServiceCollection();

service_collection.AddScoped<IDataManager, DataManager>();
service_collection.AddScoped<IDataProcessor, ConsolePrintProcessor>();
//service_collection.AddSingleton<IDataProcessor, WriteToFileProcessor>();

//service_collection.AddScoped<>()

//service_collection.AddTransient<>()

var provider = service_collection.BuildServiceProvider();

var service = provider.GetRequiredService<IDataManager>();

using (var scope = provider.CreateScope())
{
    var scope_provider = scope.ServiceProvider;
    var service2 = scope_provider.GetRequiredService<IDataManager>();
    var is_equals = ReferenceEquals(service, service2);
}


var data = Enumerable.Range(1, 100).Select(i => new DataValue
{
    Id = i,
    Value = $"Value-{i}",
    Time = DateTime.Now.AddHours(-i * 10),
});

service.ProcessData(data);

Console.ReadLine();

provider.GetRequiredService<IDataManager>();
