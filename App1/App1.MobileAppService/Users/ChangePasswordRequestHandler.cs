using System.Threading;
using System.Threading.Tasks;

using App1.MobileAppService.Services;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace App1.MobileAppService.Users
{
    public class ChangePasswordRequestHandler : IRequestHandler<ChangePasswordRequest, IdentityResult>
    {
        private readonly UserManager<Models.User> userManager;
        private readonly IIdentityService identityService;

        public ChangePasswordRequestHandler(
            UserManager<Models.User> userManager,
            IIdentityService identityService)
        {
            this.userManager = userManager;
            this.identityService = identityService;
        }

        public async Task<IdentityResult> Handle(ChangePasswordRequest request, CancellationToken cancellationToken)
        {
            var user = await identityService.GetCurrentUserAsync();
            return await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        }
    }
}
