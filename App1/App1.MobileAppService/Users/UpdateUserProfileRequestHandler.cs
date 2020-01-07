using System.Threading;
using System.Threading.Tasks;

using App1.MobileAppService.Services;

using AutoMapper;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace App1.MobileAppService.Users
{
    public class UpdateUserProfileRequestHandler : IRequestHandler<UpdateUserProfileRequest, IdentityResult>
    {
        private readonly UserManager<Models.User> userManager;
        private readonly IIdentityService identityService;
        private readonly IMapper mapper;

        public UpdateUserProfileRequestHandler(
            UserManager<Models.User> userManager,
            IIdentityService identityService,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.identityService = identityService;
            this.mapper = mapper;
        }

        public async Task<IdentityResult> Handle(UpdateUserProfileRequest request, CancellationToken cancellationToken)
        {
            var user = await identityService.GetCurrentUserAsync();
            user = mapper.Map(request, user);
            return await userManager.UpdateAsync(user);
        }
    }
}
