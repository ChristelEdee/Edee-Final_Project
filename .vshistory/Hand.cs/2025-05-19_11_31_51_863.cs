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

        /*  The DisplayHiddenHand() method prints a placeholder "[ UNO ]" for each card in the hand without revealing card details.
         *  
         *  Parameters: None
         *  Returns: None
         *  
         *  Algorithm:
         *    - Loop through each card in the cards list.
         *    - For each card, print the string "[ UNO ] " to the console.
         */
        public void DisplayHiddenHand()
        {
            foreach (Card card in cards)
            {
                Console.Write("[ UNO ] ");
            }
        }

        /*  The GetCardColor() method returns the ConsoleColor that corresponds to a card's color.
         *  Special cards (like +4 or ChangeColor) default to DarkGray.
         *  
         *  Parameters: Card card - the card whose color is being checked.
         *  Returns: ConsoleColor - the console color representing the card's color.
         *  
         *  Algorithm:
         *    - Initialize color to DarkGray (default for special cards).
         *    - If card color is Red, set color to Red.
         *    - If card color is Green, set color to Green.
         *    - If card color is Blue, set color to Blue.
         *    - If card color is Yellow, set color to Yellow.
         *    - Return the determined ConsoleColor.
         */
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

        /*  The SortHandByColor() method sorts the hand of cards by color priority using selection sort.
         *  The sorting order is: Red, Green, Blue, Yellow, then Special Cards.
         *  
         *  Parameters: None
         *  Returns: None
         *  
         *  Algorithm:
         *    - For each index i from 0 to Size - 1:
         *      - Set minIndex to i.
         *      - For each index j from i + 1 to Size - 1:
         *        - Compare cards[j] and cards[minIndex] using the overloaded < operator.
         *        - If cards[j] is less than cards[minIndex], update minIndex to j.
         *      - If minIndex is different from i, swap cards[i] and cards[minIndex].
         *    - Result: cards list is sorted by color priority.
         */
        public void SortHandByColor()
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
