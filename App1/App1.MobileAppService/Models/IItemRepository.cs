using System.Collections.Generic;
using System.Threading.Tasks;

namespace App1.Models
{
    public interface IItemRepository
    {
        Task AddAsync(Item item);
        Task UpdateAsync(Item item);
        Task<Item> RemoveAsync(string key);
        Task<Item> GetAsync(string id);
        Task<IEnumerable<Item>> GetAllAsync();
    }
}
