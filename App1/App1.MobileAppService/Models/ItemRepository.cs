using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using App1.MobileAppService.Data;

using Microsoft.EntityFrameworkCore;

namespace App1.Models
{
    public class ItemRepository : IItemRepository
    {
        private readonly ApplicationContext db;

        public ItemRepository(ApplicationContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<Item>> GetAllAsync()
        {
            return await db.Items.ToListAsync();
        }

        public async Task AddAsync(Item item)
        {
            item.Id = Guid.NewGuid().ToString();
            await db.Items.AddAsync(item);
            await db.SaveChangesAsync();
        }

        public async Task<Item> GetAsync(string id)
        {
            return await db.Items.FindAsync(id);
        }

        public async Task<Item> RemoveAsync(string id)
        {
            var item = await db.Items.FindAsync(id);
            db.Items.Remove(item);
            await db.SaveChangesAsync();
            return item;
        }

        public async Task UpdateAsync(Item item)
        {
            db.Items.Update(item);
            await db.SaveChangesAsync();
        }
    }
}
