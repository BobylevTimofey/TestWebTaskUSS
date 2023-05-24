using System;

namespace WebApplication2.Models
{
    public class FisherYatesSort : ISorter
    {
        public string Name => "Fisher Yates";

        public Deck Sort(Deck deck)
        {
            var random = new Random();

            for (var i = deck.Cards.Count - 1; i > 0; i--)
            {
                var randomIndex = random.Next(i + 1);
                var buffer = deck.Cards[i];
                deck.Cards[i] = deck.Cards[randomIndex];
                deck.Cards[randomIndex] = buffer;
            }
            return deck;
        }
    }
}
