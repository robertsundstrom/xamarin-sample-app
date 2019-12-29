using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using App1.MobileAppService.Data;
using App1.MobileAppService.Hubs;
using App1.MobileAppService.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace App1.MobileAppService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> userManager;
        private readonly IHubContext<ChatHub, IChatClient> hubContext;

        public MessagesController(ApplicationDbContext context,
            UserManager<User> userManager,
            IHubContext<ChatHub, IChatClient> hubContext)
        {
            _context = context;
            this.userManager = userManager;
            this.hubContext = hubContext;
        }

        // GET: api/Messages
        [HttpGet("{conversationId}")]
        public async Task<ActionResult<IEnumerable<Models.Dtos.Message>>> GetMessages(Guid conversationId)
        {
            var messages = await _context.Messages
                .Include(x => x.Conversation)
                .Include(x => x.Sender)
                .Where(x => x.Conversation.Id == conversationId)
                .ToListAsync();

            return Ok(messages.Select(CreateDto));
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

        //// PUT: api/Messages/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for
        //// more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPut("{id}")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesDefaultResponseType]
        //public async Task<IActionResult> PutMessage(Guid id, Message message)
        //{
        //    if (id != message.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(message).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!MessageExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Messages
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("{conversationId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Models.Dtos.Message>> PostMessage(Guid conversationId, Models.Dtos.NewMessage arg)
        {
            var conversation = await _context.Conversations.FindAsync(conversationId);

            if (conversation == null)
            {
                return NotFound($"Conversation {conversationId} does not exist.");
            }

            var message = new Message()
            {
                Conversation = conversation,
                Sender = await userManager.FindByNameAsync(User.Identity.Name),
                SendDate = DateTime.Now,
                Text = arg.Text
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            await hubContext.Clients.All
                .OnMessageReceived(CreateDto(message));

            return Created(string.Empty, CreateDto(message));
        }

        // DELETE: api/Messages/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Models.Dtos.Message>> DeleteMessage(Guid id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();

            return CreateDto(message);
        }

        private bool MessageExists(Guid id)
        {
            return _context.Messages.Any(e => e.Id == id);
        }
    }
}
