using System.ComponentModel.DataAnnotations;

namespace App1.MobileAppService.Models
{
    public class UpdateUserDto
    {
        [Required]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Email { get; set; }
    }
}
