using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;

namespace App1.MobileAppService.Models
{

    public class User : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string RefreshToken { get; set; }
    }
}
