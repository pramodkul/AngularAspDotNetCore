using Cards.API.Data;
using Cards.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Cards.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardsController : Controller
    {
        private readonly CardDbContext cardDbContext;

        public CardsController(CardDbContext cardDbContext)
        {
            this.cardDbContext = cardDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCards()
        {
            var cards = await cardDbContext.Cards.ToListAsync();
            return Ok(cards);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetCard")]
        public async Task<IActionResult> GetCard([FromRoute] Guid id)
        {
            var card = await cardDbContext.Cards.FirstOrDefaultAsync(x=> x.Id == id);
            if (card != null) 
            {
                return Ok(card);
            }
            return NotFound("Card Not Found");
        }

        [HttpPost]
        public async Task<IActionResult> AddCards([FromBody] Card card)
        {
            card.Id = Guid.NewGuid();
            await cardDbContext.Cards.AddAsync(card);
            await cardDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCard), new { id = card.Id }, card);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateCard([FromRoute] Guid id,[FromBody] Card card)
        {
            var existingcard = await cardDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if (existingcard != null)
            {
                existingcard.CardHolderName = card.CardHolderName;
                existingcard.CardNumber = card.CardNumber;
                existingcard.ExpiryMonth = card.ExpiryMonth;
                existingcard.ExpiryYear = card.ExpiryYear;
                existingcard.CVC = card.CVC;
                await cardDbContext.SaveChangesAsync();
                return Ok(existingcard);
            }
            return NotFound("Card Not Found");
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteCard([FromRoute] Guid id)
        {
            var existingcard = await cardDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if (existingcard != null)
            {
                cardDbContext.Remove(existingcard);
                await cardDbContext.SaveChangesAsync();
                return Ok(existingcard);
            }
            return NotFound("Card Not Found");
        }
    }
}
