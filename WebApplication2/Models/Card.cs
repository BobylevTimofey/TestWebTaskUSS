using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Models
{
    public class Card
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public int DeckId { get; set; }
        public string? CardName { get; set; }
        public string? Suit { get; set; }

    }
}
