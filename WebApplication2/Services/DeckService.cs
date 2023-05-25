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
            var decks = await _context.Decks
                .Include(deck => deck.Cards)
                .ToListAsync();
            foreach (var deck in decks)
                deck.Cards = deck.Cards
                    .OrderBy(deck => deck.Order)
                    .ToList();
            return decks;
        }

        public async Task<IEnumerable<string?>> GetNames()
        {
            return await _context.Decks
                .Select(deck => deck.Name)
                .ToListAsync();
        }

        public Deck GetDeckById(int id)
        {
            var deck = GetDeckOutContext(id).Result;
            return deck;
        }

        public async void DeleteDeck(int id)
        {
            var deck = GetDeckOutContext(id).Result;

            _context.Decks.Remove(deck);
            await _context.SaveChangesAsync();
        }

        public async void RenameDeck(int id, string name)
        {
            var deck = GetDeckOutContext(id).Result;
            deck.Name = name;
            await _context.SaveChangesAsync();
        }

        public async Task<Deck> SortDeck(int id, string nameSort)
        {
            var deck = GetDeckOutContext(id).Result;
            _sortService.Sort(nameSort, deck);
            await _context.SaveChangesAsync();
            deck.Cards = deck.Cards
                .OrderBy(deck => deck.Order)
                .ToList();
            return deck;
        }

        public async Task<Deck> CreateDeck(DeckDto deckDto)
        {
            var deck = new Deck()
            {
                DeckId = deckDto.Id,
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
            var order = 0;
            foreach (var suit in Enum.GetNames(typeof(Suit)))
                foreach (var name in Enum.GetNames(typeof(CardName)))
                {
                    var card = new Card
                    {
                        DeckId = deckId,
                        Order = order++,
                        Suit = suit,
                        CardName = name
                    };
                    cards.Add(card);
                }

            return cards;
        }

        private async Task<Deck> GetDeckOutContext(int id)
        {
            var deck = await _context.Decks
                .Include(deck => deck.Cards)
                .Where(deck => deck.DeckId == id)
                .FirstOrDefaultAsync();
            deck.Cards = deck.Cards
                .OrderBy(deck => deck.Order)
                .ToList();

            if (deck == null)
            {
                throw new Exception($"Deck with id - {id} not found");
            }
            return deck;
        }
    }
}
