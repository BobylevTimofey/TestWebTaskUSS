using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Models
{
    public class DeckContext : DbContext
    {
        public DeckContext(DbContextOptions<DeckContext> options)
            : base(options)
        {
        }

        public DbSet<Deck> Decks { get; set; } = null!;
        public DbSet<Card> Cards { get; set; } = null!;

    }
}
