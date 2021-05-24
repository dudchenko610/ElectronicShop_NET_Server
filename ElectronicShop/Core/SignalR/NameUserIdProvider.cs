using Microsoft.AspNetCore.SignalR;

namespace ElectronicShop.Core.SignalR
{
    public class NameUserIdProvider : IUserIdProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.Identity?.Name;
        }
    }
}
