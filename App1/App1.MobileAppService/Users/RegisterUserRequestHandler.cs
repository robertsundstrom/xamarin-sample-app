using System.Threading;
using System.Threading.Tasks;

using App1.MobileAppService.Services;

using AutoMapper;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace App1.MobileAppService.Users
{
    public class RegisterUserRequestHandler : IRequestHandler<RegisterUserRequest, IdentityResult>
    {
        private readonly IIdentityService identityService;
        private readonly IMapper mapper;

        public RegisterUserRequestHandler(IIdentityService identityService, IMapper mapper)
        {
            this.identityService = identityService;
            this.mapper = mapper;
        }

        public async Task<IdentityResult> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var user = mapper.Map<Models.User>(request);

            return await identityService.CreateUserAsync(user, request.Password);
        }
    }
}
