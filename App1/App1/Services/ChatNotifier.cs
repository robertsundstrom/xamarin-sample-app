using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;

using App1.MobileAppService.Client;

using Microsoft.AspNetCore.SignalR.Client;

namespace App1.Services
{
    public sealed class ChatNotifier : IChatNotifier
    {
        private readonly HubConnection hubConnection;
        private readonly Subject<Message> _whenMessageReceivedSubject;
        private IDisposable? whenMessageReceivedSubscription;

        public ChatNotifier(HubConnection hubConnection)
        {
            _whenMessageReceivedSubject = new Subject<Message>();
            this.hubConnection = hubConnection;

            hubConnection.Reconnected += HubConnection_Reconnected;

            whenMessageReceivedSubscription = hubConnection
                .On<Message>("OnMessageReceived", _whenMessageReceivedSubject.OnNext);
        }

        public IObservable<Message> WhenMessageReceived => _whenMessageReceivedSubject;


        public event EventHandler? Reconnected;


        public void Dispose()
        {
            hubConnection.Reconnected -= HubConnection_Reconnected;

            _whenMessageReceivedSubject?.Dispose();
        }

        private Task HubConnection_Reconnected(string arg)
        {
            Reconnected?.Invoke(this, EventArgs.Empty);

            return Task.CompletedTask;
        }

        public async Task SendMessageAsync(NewMessage newMessage)
        {
            await hubConnection.SendAsync("SendMessage", newMessage);
        }
    }
}
