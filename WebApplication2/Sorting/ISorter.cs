
namespace WebApplication2.Models
{
    public interface ISorter
    {
        public string Name { get; }
        public void Sort(Deck deck);
    }
}
