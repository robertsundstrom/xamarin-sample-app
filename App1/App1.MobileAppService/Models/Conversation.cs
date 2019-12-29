using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace App1.MobileAppService.Models
{
    public class Conversation
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public User StartedBy { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
}
