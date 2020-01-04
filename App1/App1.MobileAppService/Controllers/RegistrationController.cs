using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using App1.MobileAppService.Models;
using App1.MobileAppService.ViewModels;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace App1.MobileAppService.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public RegistrationController(
            UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        // Registration method to create new Identity users
        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Register([FromBody] RegistrationViewModel vm)
        {
            var result = await _userManager.CreateAsync(new User()
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                UserName = vm.Email,
                Email = vm.Email,
                RegistrationDate = DateTime.Now
            }, vm.Password); ;

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Errors);
            }

            return Ok();
        }

    }
}
