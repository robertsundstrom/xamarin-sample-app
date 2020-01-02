using System;
using System.Reactive.Subjects;

using App1.MobileAppService.Client;

using Microsoft.AspNetCore.SignalR.Client;

namespace App1.Services
{
    public sealed class ItemsHubClient : IItemsHubClient
    {
        private readonly HubConnection hubConnection;
        private IDisposable? whenItemAddedSubscription;
        private IDisposable whenItemDeletedSubscription;
        private IDisposable whenItemUpdatedSubscription;
        private Subject<Item> _whenItemAddedSubject;
        private Subject<Item> _whenItemDeletedSubject;
        private Subject<Item> _whenItemUpdatedSubject;

        public ItemsHubClient(HubConnection hubConnection)
        {
            this.hubConnection = hubConnection;

            _whenItemAddedSubject = new Subject<Item>();
            _whenItemDeletedSubject = new Subject<Item>();
            _whenItemUpdatedSubject = new Subject<Item>();

            whenItemAddedSubscription = hubConnection
                .On<Item>("ItemAdded", _whenItemAddedSubject.OnNext);

            whenItemDeletedSubscription = hubConnection
                .On<Item>("ItemDeleted", _whenItemDeletedSubject.OnNext);

            whenItemUpdatedSubscription = hubConnection
                .On<Item>("ItemUpdated", _whenItemUpdatedSubject.OnNext);
        }

        public IObservable<Item> WhenItemAdded => _whenItemAddedSubject;

        public IObservable<Item> WhenItemDeleted => _whenItemDeletedSubject;


        public IObservable<Item> WhenItemUpdated => _whenItemUpdatedSubject;

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
