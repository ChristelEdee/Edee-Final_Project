using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edee_Final_Project
{
    internal class Deck
    {
        //Field:
        private List<Card> _deck;


        public int CardsLeft
        {
            get { return _deck.Count; }
        }



        //Constructor:
        public Deck()
        {
            _deck = new List<Card>();

            Color[] colors = { Color.Green, Color.Red, Color.Blue, Color.Yellow, Color.Gray };
            Rank[] ranks = { Rank.Zero, Rank.One, Rank.Two, Rank.Three, Rank.Four, Rank.Five, Rank.Six,
                            Rank.Seven, Rank.Eight, Rank.Nine, Rank.Skip, Rank.Reverse, Rank.PlusTwo, Rank.PlusTwo };

            foreach (Color color in colors)
            {
                foreach (Rank rank in ranks)
                {
                    _deck.Add(new Card(color, rank));
                }
            }
        }


        //Methods:
        public void Shuffle()
        {
            Random random = new Random();
            int n = _deck.Count;

            //Shuffling process:
            while (n > 1)
            {
                n--;
                int i = random.Next(n + 1); //Taking a random index
                Card tempCard = _deck[i]; //Picking the specific card at that index
                _deck[i] = _deck[n]; //Switching that card for another one in the deck
                _deck[n] = tempCard;

            }
        }

        public Card Draw()
        {
            if (CardsLeft == 0)
                return null;

            Card topCard = _deck[0];
            _deck.Remove(topCard);

            return topCard;
        }
    }
}
        
        
