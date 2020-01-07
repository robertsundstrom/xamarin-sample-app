using System;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace App1.MobileAppService.Users
{
    public class RegisterUserRequestHandler : IRequestHandler<RegisterUserRequest, IdentityResult>
    {
        private readonly UserManager<Models.User> userManager;
        private readonly IMapper mediator;

        public RegisterUserRequestHandler(UserManager<Models.User> userManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.mediator = mapper;
        }

        public async Task<IdentityResult> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var user = mediator.Map<Models.User>(request);

            user.RegistrationDate = DateTime.Now;

            return await userManager.CreateAsync(user, request.Password);
        }
    }
}
