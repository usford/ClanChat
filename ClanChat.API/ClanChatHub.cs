using Microsoft.AspNetCore.SignalR;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClanChat.API
{
    public class ClanChatHub : Hub
    {
        public async Task SendMessageToClan(string clanName, string user, string message, DateTime dispatchDate)
        {
            await Clients.Group(clanName).SendAsync("ReceiveMessage", user, message, dispatchDate);
        }

        public async Task JoinClan(string clanName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, clanName);
        }

        public async Task LeaveClan(string clanName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, clanName);
        }
    }
}
