
namespace WebApplication2.Models
{
    public interface ISorter
    {
        public string Name { get; }
        public Deck Sort(Deck deck);
    }
}
