﻿using System.Security.Claims;
using System.Threading.Tasks;

using App1.MobileAppService.Models;

using Microsoft.AspNetCore.Authorization;
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
        public async Task UpdateUser(Models.Dtos.UpdateUser updateUser)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var email = claimsIdentity.FindFirst(ClaimTypes.Email).Value;
            var user = await userManager.FindByEmailAsync(email);
            user.FirstName = updateUser.FirstName;
            user.LastName = updateUser.LastName;
            user.Email = updateUser.Email;
            await userManager.UpdateAsync(user);
        }
    }
}
