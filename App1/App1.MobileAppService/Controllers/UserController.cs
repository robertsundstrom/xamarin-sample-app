using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using App1.MobileAppService.Models;
using App1.MobileAppService.ViewModels;

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
        private readonly UserManager<User> userManager;

        public UserController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<Models.Dtos.User> GetUser()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var email = claimsIdentity.FindFirst(ClaimTypes.Email).Value;
            var user = await userManager.FindByEmailAsync(email);
            return new Models.Dtos.User()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                RegistrationDate = user.RegistrationDate
            };
        }


        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUser(Models.Dtos.UpdateUser updateUser)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var email = claimsIdentity.FindFirst(ClaimTypes.Email).Value;
            var user = await userManager.FindByEmailAsync(email);
            user.FirstName = updateUser.FirstName;
            user.LastName = updateUser.LastName;
            user.Email = updateUser.Email;
            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Errors);
            }

            return Ok();
        }

        [HttpPost]
        [Route("ChangePassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel vm)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var email = claimsIdentity.FindFirst(ClaimTypes.Email).Value;
            var user = await userManager.FindByEmailAsync(email);
            var result = await userManager.ChangePasswordAsync(user, vm.CurrentPassword, vm.NewPassword); ;

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Errors);
            }

            return Ok();
        }
    }
}
