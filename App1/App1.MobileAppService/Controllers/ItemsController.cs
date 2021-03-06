﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using App1.MobileAppService.Hubs;
using App1.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace App1.Controllers
{
    [Authorize]
    [Route("api/items")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemRepository ItemRepository;
        private readonly IHubContext<ItemsHub, IItemsClient> hubContext;

        public ItemsController(IItemRepository itemRepository, IHubContext<ItemsHub, IItemsClient> hubContext)
        {
            ItemRepository = itemRepository;
            this.hubContext = hubContext;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Item>>> List()
        {
            var items = await ItemRepository.GetAllAsync();
            return items.ToList();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Item>> GetItem(string id)
        {
            var item = await ItemRepository.GetAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Item>> Create([FromBody]Item item)
        {
            await ItemRepository.AddAsync(item);
            await hubContext.Clients.All.ItemAdded(item);
            return CreatedAtAction(nameof(GetItem), new { item.Id }, item);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Edit([FromBody] Item item)
        {
            try
            {
                await ItemRepository.UpdateAsync(item);
                await hubContext.Clients.All.ItemUpdated(item);
            }
            catch (Exception)
            {
                return BadRequest("Error while editing item");
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(string id)
        {
            var item = await ItemRepository.RemoveAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            await hubContext.Clients.All.ItemDeleted(item);

            return Ok();
        }
    }
}
