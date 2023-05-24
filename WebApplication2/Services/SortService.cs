using WebApplication2.Models;

namespace WebApplication2.Services
{
    public class SortService
    {
        private readonly IEnumerable<ISorter> _sorters;
        public SortService(IEnumerable<ISorter> sorters) 
        {
            _sorters = sorters;
        }

        public void Sort(string nameSort, Deck deck)
        {
           var sort = _sorters.Where(sort => sort.Name == nameSort).FirstOrDefault();
            if (sort != null)
                sort.Sort(deck);
            else
                throw new Exception("Name sort not found");
        }

    }
}
