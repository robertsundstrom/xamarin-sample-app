using System;

namespace App1.MobileAppService.Models
{
    public class UserDto
    {
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}
