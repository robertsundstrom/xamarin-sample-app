using System.Threading.Tasks;

using App1.Models;

namespace App1.MobileAppService.Hubs
{
    public interface IItemsClient
    {
        Task ItemAdded(Item item);
        Task ItemDeleted(Item item);
        Task ItemUpdated(Item item);
    }
}
