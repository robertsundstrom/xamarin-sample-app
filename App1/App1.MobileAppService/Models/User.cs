using Microsoft.AspNetCore.Identity;

namespace App1.MobileAppService.Models
{

    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string RefreshToken { get; set; }
    }
}
