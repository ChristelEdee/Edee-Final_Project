using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edee_Final_Project
{
    internal class Hand
    {
        private List<Card> cards;

        public Hand()
        {
            cards = new List<Card>();
            Random random = new Random();
            const int HAND_SIZE = 7;

            for (int i = 0; i < HAND_SIZE; i++)
            {
                Rank rank = (Rank)random.Next(2, 15); //Choosing random rank from the Rank enum
                Color color = (Color)random.Next(0, 4); //Choosing random suit from the Color enum
                cards.Add(new Card(color, rank)); //Adding the created card to the hand
            }
        }
    }
}
