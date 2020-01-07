using System.ComponentModel.DataAnnotations;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace App1.MobileAppService.Users
{
    public class ChangePasswordRequest : IRequest<IdentityResult>
    {
        [Required]
        [StringLength(20, MinimumLength = 8)]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 8)]
        public string NewPassword { get; set; }
    }
}
