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

        public void Sort(Deck deck)
        {
            for (var i = 0; i < countRepeat; i++)
            {
                var random = new Random();
                var firstRandomIndex = random.Next(deck.Cards.Count);
                var secondRandomIndex = random.Next(firstRandomIndex);
                SwapDeckParts(deck, firstRandomIndex, secondRandomIndex);
            }
        }

        private void SwapDeckParts(Deck deck, int firstIndex, int secondIndex)
        {
            var firstCardsBuffer = deck.Cards.Skip(firstIndex);
            var secondCardsBuffer = deck.Cards.Take(secondIndex);
            var result = secondCardsBuffer
                .Concat(firstCardsBuffer)
                .Concat(deck.Cards.Skip(secondIndex).Take(firstIndex - secondIndex))
                .Select(card => card.Order)
                .ToList();

            var index = 0;
            foreach(var card in deck.Cards) 
            { 
                card.Order = result[index++];
            }
            deck.Cards = deck.Cards.OrderBy(card => card.Order).ToList();
        }
    }
}
