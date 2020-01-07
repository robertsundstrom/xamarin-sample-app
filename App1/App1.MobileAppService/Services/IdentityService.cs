using System.Security.Claims;
using System.Threading.Tasks;

using App1.MobileAppService.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace App1.MobileAppService.Services
{
    public sealed class IdentityService : IIdentityService
    {
        private IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<User> userManager;

        public IdentityService(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<User> GetCurrentUserAsync()
        {
            var claimsIdentity = (ClaimsIdentity)httpContextAccessor.HttpContext.User.Identity;
            var email = claimsIdentity.FindFirst(ClaimTypes.Email).Value;
            return (User)await userManager.FindByEmailAsync(email);
        }
    }
}
