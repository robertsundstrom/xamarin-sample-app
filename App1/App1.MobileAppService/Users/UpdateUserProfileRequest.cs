using System.ComponentModel.DataAnnotations;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace App1.MobileAppService.Users
{
    public class UpdateUserProfileRequest : IRequest<IdentityResult>
    {
        [Required]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Email { get; set; }
    }
}
