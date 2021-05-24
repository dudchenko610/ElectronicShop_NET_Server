using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicShop.Core.SignalR
{
    public class SignalRHub : Hub
    {
        protected async Task SendToAll(Object response, string methodName)
        {
            await Clients.All.SendAsync(methodName, response);
        }

        protected async Task SendToUser(Object response, string userName, string methodName)
        {
            await Clients.User(userName).SendAsync(methodName, response);
        }
    }
}
