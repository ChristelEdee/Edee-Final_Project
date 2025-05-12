using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edee_Final_Project
{
    internal class Hand
    {
        //Field:
        private List<Card> cards;


        //Constructor
        public Hand()
        {
            cards = new List<Card>();
            Random random = new Random();
            Color color;
            const int HAND_SIZE = 7; //All players start with 7 cards


            for (int i = 0; i < HAND_SIZE; i++)
            {
                Rank rank = (Rank)random.Next(0, 15); //Choosing random rank from the Rank enum

                if (rank == Rank.ChangeColor || rank == Rank.PlusFour)
                    color = Color.Gray; //The color of the card is immediately dark gray if it's a "special" card (+4 or ChangeColor)
                else
                    color = (Color)random.Next(0, 4); //Choosing random color from the Color enum (minus Gray) if the card is normal

                cards.Add(new Card(color, rank)); //Adding the created card to the hand
            }
        }


        public ConsoleColor GetCardColor(Card card)
        {
            ConsoleColor color = ConsoleColor.DarkGray; //Color for special cards (+4 or ChangeColor)

            if (card.Color == Color.Red)
                color = ConsoleColor.Red;
            if (card.Color == Color.Green)
                color = ConsoleColor.Green;
            if (card.Color == Color.Blue)
                color = ConsoleColor.Blue;
            if (card.Color == Color.Yellow)
                color = ConsoleColor.Yellow;

            return color;
        }


        //Method
        public void DisplayHand()
        {
            foreach (Card card in cards)
            {
                Console.ForegroundColor = GetCardColor(card);
                Console.Write($"{card}  ");
            }

            Console.ForegroundColor = ConsoleColor.White; // Reset color after
        }

    }
}
