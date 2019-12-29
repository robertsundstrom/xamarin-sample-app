using System;
using System.Threading.Tasks;

using App1.MobileAppService.Client;

namespace App1.Services
{
    public interface IChatNotifier : IDisposable
    {
        IObservable<Message> WhenMessageReceived { get; }

        event EventHandler? Reconnected;

        Task SendMessageAsync(NewMessage newMessage);
    }
}
