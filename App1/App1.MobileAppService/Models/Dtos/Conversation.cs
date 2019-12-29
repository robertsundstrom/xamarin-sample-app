using System;

namespace App1.MobileAppService.Models.Dtos
{
    public class Conversation
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public UserInfo StartedBy { get; set; }

        public DateTime StartDate { get; set; }
    }
}
