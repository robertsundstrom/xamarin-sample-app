using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using App1.MobileAppService.Client;

using Xamarin.Essentials;

namespace App1.Services
{
    public class AzureDataStore : IDataStore<Item>
    {
        private readonly IItemsClient client;
        private readonly IEnumerable<Item> items;

        public AzureDataStore(IItemsClient client)
        {
            this.client = client;

            items = new List<Item>();
        }

        private bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;
        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            if (forceRefresh && IsConnected)
            {
                return await client.ListAsync();
            }

            return items;
        }

        public async Task<Item> GetItemAsync(string id)
        {
            if (id != null && IsConnected)
            {
                return await client.GetItemAsync(id);
            }

            throw new Exception("Item was not found");
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            if (item == null || !IsConnected)
            {
                return false;
            }

            var responseItem = await client.CreateAsync(item);

            item.Id = responseItem.Id;

            return true;
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            if (item == null || item.Id == null || !IsConnected)
            {
                return false;
            }

            await client.EditAsync(item);

            return true;
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            if (string.IsNullOrEmpty(id) && !IsConnected)
            {
                return false;
            }

            await client.DeleteAsync(id);

            return true;
        }
    }
}
