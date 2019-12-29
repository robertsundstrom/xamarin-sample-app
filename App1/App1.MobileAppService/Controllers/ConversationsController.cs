using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using App1.MobileAppService.Data;
using App1.MobileAppService.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App1.MobileAppService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> userManager;

        public ConversationsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        // GET: api/Conversations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Dtos.Conversation>>> GetConversations()
        {
            var conversations = await _context.Conversations.Include(x => x.StartedBy).ToListAsync();
            var foo = conversations.Select(c => CreateDto(c));
            return Ok(foo);
        }

        private static Models.Dtos.Conversation CreateDto(Models.Conversation c)
        {
            return new Models.Dtos.Conversation
            {
                Id = c.Id,
                Title = c.Title,
                StartedBy = new Models.Dtos.UserInfo
                {
                    Id = c.StartedBy.Id,
                    Name = $"{c.StartedBy.FirstName} {c.StartedBy.LastName}"
                },
                StartDate = c.StartDate
            };
        }

        // GET: api/Conversations/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Models.Dtos.Conversation>> GetConversation(Guid id)
        {
            var conversation = await _context.Conversations.FindAsync(id);

            if (conversation == null)
            {
                return NotFound();
            }

            return CreateDto(conversation);
        }

        //// PUT: api/Conversations/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for
        //// more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPut("{id}")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesDefaultResponseType]
        //public async Task<IActionResult> PutConversation(Guid id, Models.Dtos.Conversation conversation)
        //{
        //    if (id != conversation.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(conversation).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ConversationExists(id))
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

        // POST: api/Conversations
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Models.Dtos.Conversation>> StartConversation(string title)
        {
            var conversation = new Models.Conversation()
            {
                Title = title,
                StartedBy = await userManager.FindByNameAsync(User.Identity.Name),
                StartDate = DateTime.Now
            };

            _context.Conversations.Add(conversation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConversation", new { id = conversation.Id }, CreateDto(conversation));
        }

        // DELETE: api/Conversations/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Models.Dtos.Conversation>> DeleteConversation(Guid id)
        {
            var conversation = await _context.Conversations.FindAsync(id);
            if (conversation == null)
            {
                return NotFound();
            }

            _context.Conversations.Remove(conversation);
            await _context.SaveChangesAsync();

            return CreateDto(conversation);
        }

        private bool ConversationExists(Guid id)
        {
            return _context.Conversations.Any(e => e.Id == id);
        }
    }
}
