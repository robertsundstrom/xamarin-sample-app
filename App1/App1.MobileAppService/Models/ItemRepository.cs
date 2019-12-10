using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App1.Models
{
    public class ItemRepository : IItemRepository
    {
        private static readonly ConcurrentDictionary<string, Item> items =
            new ConcurrentDictionary<string, Item>();

        public ItemRepository()
        {
            Add(new Item { Id = Guid.NewGuid().ToString(), Text = "Item 1", Description = "This is an item description." });
            Add(new Item { Id = Guid.NewGuid().ToString(), Text = "Item 2", Description = "This is an item description." });
            Add(new Item { Id = Guid.NewGuid().ToString(), Text = "Item 3", Description = "This is an item description." });
        }

        public Task<IEnumerable<Item>> GetAllAsync()
        {
            return Task.FromResult(items.Values.AsEnumerable());
        }

        public Task AddAsync(Item item)
        {
            Add(item);
            return Task.CompletedTask;
        }

        public Task<Item> GetAsync(string id)
        {
            items.TryGetValue(id, out var item);
            return Task.FromResult(item);
        }

        public Task<Item> RemoveAsync(string id)
        {
            items.TryRemove(id, out var item);
            return Task.FromResult(item);
        }

        public Task UpdateAsync(Item item)
        {
            items[item.Id] = item;
            return Task.CompletedTask;
        }

        private static void Add(Item item)
        {
            item.Id = Guid.NewGuid().ToString();
            items[item.Id] = item;
        }
    }
}
