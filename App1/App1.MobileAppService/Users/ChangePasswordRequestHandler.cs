using System.Threading;
using System.Threading.Tasks;

using App1.MobileAppService.Services;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace App1.MobileAppService.Users
{
    public class ChangePasswordRequestHandler : IRequestHandler<ChangePasswordRequest, IdentityResult>
    {
        private readonly IIdentityService identityService;

        public ChangePasswordRequestHandler(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        public async Task<IdentityResult> Handle(ChangePasswordRequest request, CancellationToken cancellationToken)
        {
            var user = await identityService.GetUserAsync();
            return await identityService.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        }
    }
}
