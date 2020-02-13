using System.Collections.Generic;
using System.Threading.Tasks;

using App1.MobileAppService.Users;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace App1.MobileAppService.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator mediator;

        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<Users.User> GetUser()
        {
            return await mediator.Send(new GetUserRequest());
        }


        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUser(UpdateUserProfileRequest request)
        {
            var result = await mediator.Send(request);

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Errors);
            }

            return Ok();
        }

        [HttpPost("ChangePassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var result = await mediator.Send(request);

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Errors);
            }

            return Ok();
        }

        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var result = await mediator.Send(request);

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Errors);
            }

            return Ok();
        }

    }
}
