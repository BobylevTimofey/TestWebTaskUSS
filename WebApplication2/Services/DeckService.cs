using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public class DeckService
    {
        private readonly DeckContext _context;
        private readonly SortService _sortService;

        public DeckService(DeckContext context, SortService sortService)
        {
            _context = context;
            _sortService = sortService;
        }

        public async Task<List<Deck>> GetDecks()
        {
            var decks = await _context.Decks.ToListAsync();
            foreach (var deck in decks)
            {
                deck.Cards = await _context.Cards.Where(card => card.DeckId == deck.Id).ToListAsync();
            }
            return decks;
        }

        public async Task<IEnumerable<string>> GetNames()
        {
            return await _context.Decks.Select(deck => deck.Name).ToListAsync();
        }

        public async Task<Deck> GetDeckById(int id)
        {
            var deck = GetDeckOutContext(id).Result;
            deck.Cards = await _context.Cards.Where(card => card.DeckId == id).ToListAsync();
            return deck;
        }

        public async void DeleteDeck(int id)
        {
            var deck = GetDeckOutContext(id).Result;
            var cards = _context.Cards.Where(card => card.DeckId == id);

            foreach (var card in cards)
            {
                _context.Cards.Remove(card);
            }

            _context.Decks.Remove(deck);
            await _context.SaveChangesAsync();
        }

        public async void RenameDeck(int id, string name)
        {
            var deck =  GetDeckOutContext(id).Result;
            deck.Name = name;
            _context.Entry(deck).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeckExists(id))
                {
                    throw new Exception($"Deck with id - {id} not found");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<Deck> SortDeck(int id, string nameSort)
        {
            var deck = GetDeckOutContext(id).Result;

            deck.Cards = await _context.Cards.Where(card => card.DeckId == id).ToListAsync();

            _sortService.Sort(nameSort, deck);

            var newDeck = new Deck()
            {
                Id = deck.Id,
                Name = deck.Name,
                Cards = deck.Cards
            };

            foreach (var card in deck.Cards)
            {
                _context.Cards.Remove(card);
            }

            _context.Decks.Remove(deck);
            await _context.SaveChangesAsync();

            _context.Decks.Add(newDeck);
            foreach (var card in newDeck.Cards)
            {
                _context.Cards.Add(card);
            }
            await _context.SaveChangesAsync();

            return newDeck;
        }

        public async Task<Deck> CreateDeck(DeckDto deckDto)
        {
            var deck = new Deck()
            {
                Id = deckDto.Id,
                Name = deckDto.Name,
                Cards = CreateCards(deckDto.Id)
            };

            _context.Decks.Add(deck);
            await _context.SaveChangesAsync();
            return deck;
        }

        private List<Card> CreateCards(int deckId)
        {
            var cards = new List<Card>();
            foreach (var suit in Enum.GetNames(typeof(Suit)))
                foreach (var name in Enum.GetNames(typeof(CardName)))
                {
                    var card = new Card
                    {
                        DeckId = deckId,
                        Suit = suit,
                        CardName = name
                    };
                    cards.Add(card);
                    _context.Cards.Add(card);
                }
            return cards;
        }

        private bool DeckExists(int id)
        {
            return (_context.Decks?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private async Task<Deck> GetDeckOutContext(int id)
        {
            var deck = await _context.Decks.FindAsync(id);
            if (deck == null)
            {
                throw new Exception($"Deck with id - {id} not found");
            }
            return deck;
        }
    }
}
