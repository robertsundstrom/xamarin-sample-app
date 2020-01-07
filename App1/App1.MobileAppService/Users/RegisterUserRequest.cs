using System.ComponentModel.DataAnnotations;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace App1.MobileAppService.Users
{
    public class RegisterUserRequest : IRequest<IdentityResult>
    {
        [Required]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
