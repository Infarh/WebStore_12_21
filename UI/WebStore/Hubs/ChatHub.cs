using Microsoft.AspNetCore.SignalR;
// ReSharper disable AsyncApostle.AsyncAwaitMayBeElidedHighlighting
// ReSharper disable AsyncApostle.AsyncMethodNamingHighlighting

namespace WebStore.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string Message) => 
            await Clients.Others.SendAsync("MessageFromClient", Message);
    }
}
