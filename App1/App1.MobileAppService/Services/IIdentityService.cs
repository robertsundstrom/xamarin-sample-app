using System.Threading.Tasks;

using App1.MobileAppService.Models;

using Microsoft.AspNetCore.Identity;

namespace App1.MobileAppService.Services
{
    public interface IIdentityService
    {
        Task<User> GetUserAsync();

        Task<IdentityResult> CreateUserAsync(User user, string password);

        Task<IdentityResult> UpdateUserAsync(User user);

        Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword);
    }
}
