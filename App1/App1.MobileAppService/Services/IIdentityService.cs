using System.Threading.Tasks;
using App1.MobileAppService.Models;

namespace App1.MobileAppService.Services
{
    public interface IIdentityService
    {
        Task<User> GetCurrentUserAsync();
    }
}