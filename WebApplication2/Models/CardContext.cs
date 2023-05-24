using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Models
{
    public class CardContext : DbContext
    {
        public CardContext(DbContextOptions<CardContext> options)
            : base(options)
        {
        }

        public DbSet<Card> Cards { get; set; } = null!;
    }
}
