using System.ComponentModel.DataAnnotations;

namespace App1.MobileAppService.ViewModels
{
    public class RegistrationViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(8)]
        public string Password { get; set; }
    }
}
