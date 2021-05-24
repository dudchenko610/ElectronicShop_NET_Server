using System.Threading.Tasks;

namespace Shared.SignalR
{
    public delegate Task SendFeedBackDelegate(object message, string userName);
}
