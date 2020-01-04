﻿using System.ComponentModel.DataAnnotations;

namespace App1.MobileAppService.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [StringLength(20, MinimumLength = 8)]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 8)]
        public string NewPassword { get; set; }
    }
}
