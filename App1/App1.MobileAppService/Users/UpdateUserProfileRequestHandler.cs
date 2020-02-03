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
        private readonly IIdentityService identityService;
        private readonly IMapper mapper;

        public UpdateUserProfileRequestHandler(
            IIdentityService identityService,
            IMapper mapper)
        {
            this.identityService = identityService;
            this.mapper = mapper;
        }

        public async Task<IdentityResult> Handle(UpdateUserProfileRequest request, CancellationToken cancellationToken)
        {
            var user = await identityService.GetUserAsync();
            user = mapper.Map(request, user);
            return await identityService.UpdateUserAsync(user);
        }
    }
}
