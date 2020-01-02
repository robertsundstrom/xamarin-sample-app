using System;

using App1.MobileAppService.Client;

namespace App1.Services
{
    public interface IItemsHubClient : IDisposable
    {
        IObservable<Item> WhenItemAdded { get; }

        IObservable<Item> WhenItemDeleted { get; }

        IObservable<Item> WhenItemUpdated { get; }
    }
}
