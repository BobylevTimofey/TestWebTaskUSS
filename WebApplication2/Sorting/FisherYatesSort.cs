using System;

namespace WebApplication2.Models
{
    public class FisherYatesSort : ISorter
    {
        public string Name => "Fisher Yates";

        public void Sort(Deck deck)
        {
            var random = new Random();

            for (var i = deck.Cards.Count - 1; i > 0; i--)
            {
                var randomIndex = random.Next(i + 1);
                var buffer = deck.Cards[i].Order;
                deck.Cards[i].Order = deck.Cards[randomIndex].Order;
                deck.Cards[randomIndex].Order = buffer;
            }
        }
    }
}
