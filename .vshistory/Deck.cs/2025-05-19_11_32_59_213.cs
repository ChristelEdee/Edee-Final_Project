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


        //Property:
        public int CardsLeft
        {
            get { return _deck.Count; }
        }



        //Constructor:
        public Deck()
        {
            _deck = new List<Card>();
            int count = 0;
            Color colorOfCard;


            Color[] colors = { Color.Green, Color.Red, Color.Blue, Color.Yellow};
            Rank[] ranks = { Rank.Zero, Rank.One, Rank.Two, Rank.Three, Rank.Four, Rank.Five, Rank.Six,
                            Rank.Seven, Rank.Eight, Rank.Nine, Rank.Skip, Rank.Reverse, Rank.PlusTwo, Rank.PlusFour, Rank.ChangeColor };

            //Populating the deck with random cards (going through the while-loop 3 times just to add more cards):
            while(count != 3)
            {
                foreach (Color color in colors)
                {
                    colorOfCard = color;

                    foreach (Rank rank in ranks)
                    {
                        if (rank == Rank.ChangeColor || rank == Rank.PlusFour)
                            colorOfCard = Color.Gray; //The color of the card is immediately dark gray if it's a "special" card (+4 or ChangeColor)

                        _deck.Add(new Card(colorOfCard, rank));
                    }
                }
                count++;
            }
            
        }


        //Methods:

        /*  The Shuffle() method randomizes the order of cards in the deck using the Fisher-Yates shuffle algorithm.
         *  
         *  Parameters: None
         *  Returns: None
         *  
         *  Algorithm:
         *    - Initialize a Random object for generating random indices.
         *    - Set n to the number of cards left in the deck.
         *    - While n is greater than 1:
         *      - Decrement n by 1.
         *      - Generate a random index i between 0 and n inclusive.
         *      - Swap the card at index i with the card at index n.
         *    - Result: The deck is shuffled in-place.
         */
        public void Shuffle()
        {
            Random random = new Random();
            int n = CardsLeft;

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

        public void ReputCardInDeck(Card card)
        {
            _deck.Add(card);
        }


        //Override method:
        public override string ToString()
        {
            return $"There are {this.CardsLeft} cards left.";
        }
    }
}
        
        
