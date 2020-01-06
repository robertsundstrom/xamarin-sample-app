using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App1.Services
{
    public class MockDataStore : IDataStore<Models.Item>
    {
        private readonly List<Models.Item> items;

        public MockDataStore()
        {
            items = new List<Models.Item>()
            {
                new Models.Item { Id = Guid.NewGuid().ToString(), Text = "First item", Description="This is an item description." },
                new Models.Item { Id = Guid.NewGuid().ToString(), Text = "Second item", Description="This is an item description." },
                new Models.Item { Id = Guid.NewGuid().ToString(), Text = "Third item", Description="This is an item description." },
                new Models.Item { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description="This is an item description." },
                new Models.Item { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is an item description." },
                new Models.Item { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is an item description." }
            };
        }

        public async Task<bool> AddItemAsync(Models.Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Models.Item item)
        {
            var oldItem = items.Where((Models.Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Models.Item arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Models.Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Models.Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}
