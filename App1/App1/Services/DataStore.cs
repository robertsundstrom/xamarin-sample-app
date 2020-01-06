using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using App1.Data;
using App1.MobileAppService.Client;

using AutoMapper;

using Xamarin.Essentials;

namespace App1.Services
{
    public class DataStore : IDataStore<Models.Item>
    {
        private readonly IItemsClient client;
        private readonly ApplicationDbContext dataContext;
        private readonly IMapper mapper;

        public DataStore(IItemsClient client, ApplicationDbContext dataContext, IMapper mapper)
        {
            this.client = client;
            this.dataContext = dataContext;
            this.mapper = mapper;
        }

        private bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;
        public async Task<IEnumerable<Models.Item>> GetItemsAsync(bool forceRefresh = false)
        {
            if (forceRefresh && IsConnected)
            {
                return mapper.ProjectTo<Models.Item>(
                    (await client.ListAsync()).AsQueryable());
            }

            return dataContext.Items.ToList();
        }

        public async Task<Models.Item> GetItemAsync(string id)
        {
            if (id != null && IsConnected)
            {
                return mapper.Map<Models.Item>(
                    await client.GetItemAsync(id));
            }

            var item = dataContext.Items.Find(id);
            if (item != null)
            {
                return item;
            }

            throw new Exception("Item was not found");
        }

        public async Task<bool> AddItemAsync(Models.Item item)
        {
            if (item == null || !IsConnected)
            {
                return false;
            }

            var responseItem = await client.CreateAsync(
                mapper.Map<Item>(item));

            item.Id = responseItem.Id;

            await dataContext.Items.AddAsync(item);
            await dataContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateItemAsync(Models.Item item)
        {
            if (item == null || item.Id == null || !IsConnected)
            {
                return false;
            }

            await client.EditAsync(mapper.Map<Item>(item));

            dataContext.Items.Update(item);
            await dataContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            if (string.IsNullOrEmpty(id) && !IsConnected)
            {
                return false;
            }

            await client.DeleteAsync(id);

            var item = dataContext.Items.Find(id);
            if (item != null)
            {
                dataContext.Items.Remove(item);
                await dataContext.SaveChangesAsync();
            }

            return true;
        }
    }
}
