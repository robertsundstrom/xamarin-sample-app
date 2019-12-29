using System;
using System.ComponentModel.DataAnnotations;

namespace App1.MobileAppService.Models
{
    public class Message
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Conversation Conversation { get; set; }

        [Required]
        public User Sender { get; set; }

        [Required]
        public DateTime SendDate { get; set; }

        [Required]
        public string Text { get; set; }
    }
}
