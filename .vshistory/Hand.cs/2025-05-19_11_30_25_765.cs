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


        //Properties:
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

        /*  The AddCard() method adds a new card to the hand and then sorts the hand by color.
         *  
         *  Parameters: Card card - the card to add to the hand.
         *  Returns: None
         *  
         *  Algorithm:
         *    - Add the card to the cards list.
         *    - Call SortHandByColor() to reorder the hand based on color priority.
         */
        public void AddCard(Card card)
        {
            cards.Add(card);
            SortHandByColor();
        }

        /*  The RemoveCard() method removes a specified card from the hand, sorts the hand by color, and returns the removed card.
         *  
         *  Parameters: Card card - the card to remove from the hand.
         *  Returns: Card - the removed card.
         *  
         *  Algorithm:
         *    - Remove the card from the cards list.
         *    - Call SortHandByColor() to reorder the hand.
         *    - Return the removed card.
         */
        public Card RemoveCard(Card card)
        {
            cards.Remove(card);
            SortHandByColor();

            return card;
        }

        /*  The DisplayHand() method prints each card in the hand to the console, coloring each card's text according to its color.
         *  
         *  Parameters: None
         *  Returns: None
         *  
         *  Algorithm:
         *    - Loop through each card in the cards list.
         *    - For each card, get its corresponding console color using GetCardColor().
         *    - Set console foreground color to the card's color.
         *    - Print the card followed by spacing.
         *    - After printing all cards, reset console color to white.
         */
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
