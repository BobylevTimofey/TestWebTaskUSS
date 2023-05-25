using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DecksController : ControllerBase
    {
        private readonly DeckContext _context;
        private readonly DeckService _deckService;

        public DecksController(DeckContext context, DeckService deckService)
        {
            _context = context;
            _deckService = deckService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Deck>> GetDecks()
        {
            if (_context.Decks == null)
            {
                return NotFound();
            }
 
            return Ok(_deckService.GetDecks().Result);

        }

        [HttpGet(template: "names")]
        public ActionResult<IEnumerable<string>> GetDeckNames()
        {
            if (_context.Decks == null)
            {
                return NotFound();
            }

            return Ok(_deckService.GetNames().Result);
        }

        [HttpGet("{id}")]
        public ActionResult<Deck> GetDeck(int id)
        {
            if (_context.Decks == null)
            {
                return NotFound();
            }
            
            return Ok(_deckService.GetDeckById(id));
        }


        [HttpPut("{id}")]
        public IActionResult RenameDeck(int id, [FromBody] string name)
        {
            _deckService.RenameDeck(id, name);
            return NoContent();
        }

        [HttpPost]
        public ActionResult<Deck> CreateDeck(DeckDto deckDto)
        {
            if (_context.Decks == null)
            {
                return Problem("Entity set 'DeckContext.DeckItems'  is null.");
            }

            var deck = _deckService.CreateDeck(deckDto).Result;
            return CreatedAtAction(nameof(GetDeck), new { id = deck.DeckId }, deck);
        }

        [HttpPost("sort/{id}")]
        public ActionResult<Deck> SortDeck(int id, [FromBody] string nameSort)
        {
            if (_context.Decks == null)
            {
                return Problem("Entity set 'DeckContext.DeckItems'  is null.");
            }

            var deck = _deckService.SortDeck(id, nameSort).Result;
            return CreatedAtAction(nameof(GetDeck), new { id = deck.DeckId }, deck);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDeck(int id)
        {
            if (_context.Decks == null)
            {
                return NotFound();
            }

            _deckService.DeleteDeck(id);
            return NoContent();
        }
    }
}
