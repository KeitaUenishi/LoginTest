using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace LoginTest.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessageClientsAll(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}