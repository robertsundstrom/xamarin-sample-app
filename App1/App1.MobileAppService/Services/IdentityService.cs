using System;
using System.Security.Claims;
using System.Threading.Tasks;

using App1.MobileAppService.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace App1.MobileAppService.Services
{
    public sealed class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<User> userManager;

        public IdentityService(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<User> GetUserAsync()
        {
            var claimsIdentity = (ClaimsIdentity)httpContextAccessor.HttpContext.User.Identity;
            string email = claimsIdentity.FindFirst(ClaimTypes.Email).Value;
            return await userManager.FindByEmailAsync(email);
        }

        public async Task<IdentityResult> CreateUserAsync(User user, string password)
        {
            user.RegistrationDate = DateTime.Now;

            return await userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            user.RegistrationDate = DateTime.Now;

            return await userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword)
        {
            return await userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }
    }
}
