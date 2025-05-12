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


        //Property:
        public int Size
        {
            get { return cards.Count; }
        }

        public List<Card> Cards
        {
            get { return cards; }
        }

        //Constructor:
        public Hand()
        {
            cards = new List<Card>();
        }


        
        //Methods:
        public void AddCard(Card card)
        {
            cards.Add(card);
            SortHandByColor();
        }

        public Card RemoveCard(Card card)
        {
            cards.Remove(card);
            SortHandByColor();

            return card;
        }

        public void DisplayHand()
        {
            for (int i = 0; i < cards.Count; i++)
            {
                Console.ForegroundColor = GetCardColor(cards[i]);
                Console.Write($"{cards[i]}  ");
            }

            Console.ForegroundColor = ConsoleColor.White; // Reset color after
        }

        public void DisplayHiddenHand()
        {
            foreach (Card card in cards)
            {
                Console.Write("[ UNO ] ");
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

        public void SortHandByColor() //Note: This will sort the hand like this: Red, Green, Blue, Yellow, Special Cards
        {
            int minIndex;

            for (int i = 0; i < Size; i++)
            {
                minIndex = i;

                for (int j = i + 1; j < Size; j++)
                {
                    //Comparing cards using the operator
                    if (cards[j] < cards[minIndex])
                    {
                        minIndex = j;
                    }
                }

                //Swicthing values if not sorted properly
                if (minIndex != i)
                {
                    Card temp = cards[i];
                    cards[i] = cards[minIndex];
                    cards[minIndex] = temp;
                }
            }
        }


    }
}
