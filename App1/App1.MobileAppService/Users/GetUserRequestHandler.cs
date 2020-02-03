using System.Threading;
using System.Threading.Tasks;

using App1.MobileAppService.Services;

using AutoMapper;

using MediatR;

namespace App1.MobileAppService.Users
{
    public class GetUserRequestHandler : IRequestHandler<GetUserRequest, User>
    {
        private readonly IIdentityService identityService;
        private readonly IMapper mapper;

        public GetUserRequestHandler(
            IIdentityService identityService,
            IMapper mapper)
        {
            this.identityService = identityService;
            this.mapper = mapper;
        }

        public async Task<User> Handle(GetUserRequest request, CancellationToken cancellationToken)
        {
            var user = await identityService.GetUserAsync();
            return mapper.Map<User>(user);
        }
    }
}
