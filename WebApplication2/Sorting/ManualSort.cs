using System;
using System.Linq;

namespace WebApplication2.Models
{
    /* Если делать эмуляцию перетасовки вручную, разделяя колоду пополам и меняя части местами,
     * то перемешивания не произойдет, поэтому разделяю колоду на три части
     */
    public class ManualSort : ISorter
    {
        private int countRepeat;

        public ManualSort(int countRepeat = 100)
        {
            this.countRepeat = countRepeat;
        }

        public string Name => "manual";

        public Deck Sort(Deck deck)
        {
            for (var i = 0; i < countRepeat; i++)
            {
                var random = new Random();
                var firstRandomIndex = random.Next(deck.Cards.Count);
                var secondRandomIndex = random.Next(firstRandomIndex);
                deck = SwapDeckParts(deck, firstRandomIndex, secondRandomIndex);
            }
            return deck;
        }

        private Deck SwapDeckParts(Deck deck, int firstIndex, int secondIndex)
        {
            var firstCardsBuffer = deck.Cards.Skip(firstIndex);
            var secondCardsBuffer = deck.Cards.Take(secondIndex);
            deck.Cards = secondCardsBuffer
                .Concat(firstCardsBuffer)
                .Concat(deck.Cards.Skip(secondIndex).Take(firstIndex - secondIndex))
                .ToList();
            return deck;
        }
    }
}
