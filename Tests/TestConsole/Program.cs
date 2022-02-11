
using Microsoft.AspNetCore.SignalR.Client;

var builder = new HubConnectionBuilder();
var connection = builder
   .WithUrl("http://localhost/chat")
   .Build();

using var registration = connection.On<string>("MessageFromClient", MessageFromClient);

static void MessageFromClient(string Message)
{
    Console.WriteLine("Сообщение от сервера: {0}", Message);
}

Console.WriteLine("Ожидание сервера. Нажмите Enter для запуска соединения.");
Console.ReadLine();

await connection.StartAsync();
Console.WriteLine("Соединение установлено");

while (true)
{
    var message = Console.ReadLine();
    await connection.InvokeAsync("SendMessage", message);
}