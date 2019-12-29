using System;
using System.Threading.Tasks;

using App1.MobileAppService.Data;
using App1.MobileAppService.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace App1.MobileAppService.Hubs
{
    [Authorize]
    public class ChatHub : Hub<IChatClient>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> userManager;

        public ChatHub(
            ApplicationDbContext context,
            UserManager<User> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
        }

        public async Task SendMessage(Models.Dtos.NewMessage newMessage)
        {
            var conversation = await _context.Conversations.FindAsync(newMessage.ConversationId);

            var message = new Message()
            {
                Conversation = conversation,
                Sender = await userManager.FindByNameAsync(Context.User.Identity.Name),
                SendDate = DateTime.Now,
                Text = newMessage.Text
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            await Clients.All.OnMessageReceived(CreateDto(message));
        }

        private Models.Dtos.Message CreateDto(Message arg)
        {
            return new Models.Dtos.Message()
            {
                Id = arg.Id,
                Conversation = new Models.Dtos.ConversationInfo
                {
                    Id = arg.Conversation.Id.ToString(),
                    Name = arg.Conversation.Title,
                },
                SendDate = arg.SendDate,
                Sender = new Models.Dtos.UserInfo
                {
                    Id = arg.Sender.Id,
                    Name = $"{arg.Sender.FirstName} {arg.Sender.LastName}",
                },
                Text = arg.Text
            };
        }
    }
}
