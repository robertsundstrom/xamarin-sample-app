using System;

namespace App1.MobileAppService.Models.Dtos
{
    public class Message
    {
        public Guid Id { get; set; }

        public ConversationInfo Conversation { get; set; }

        public UserInfo Sender { get; set; }

        public DateTime SendDate { get; set; }

        public string Text { get; set; }
    }

    public class NewMessage
    {
        public Guid ConversationId { get; set; }

        public string Text { get; set; }
    }
}
