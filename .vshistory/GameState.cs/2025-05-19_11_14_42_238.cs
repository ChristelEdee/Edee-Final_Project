using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edee_Final_Project
{
    internal class GameState
    {
        //Fields:
        private Deck _drawDeck;
        private List<Hand> _playerHands;
        private List<Card> _cardsInMiddle;
        private List<Player> _allPlayers;


        //Constructors:
        public GameState()
        {
            int playerNum = 4;

            _drawDeck = new Deck();
            _playerHands = new List<Hand>();
            _cardsInMiddle = new List<Card>();
            _allPlayers = new List<Player>();

            _drawDeck.Shuffle();
            PutFirstCardInMIddle();

            for (int i = 0; i < playerNum; i++)
            {
                _playerHands.Add(new Hand());
                
            }
        }

        public GameState(List<Player> allPlayers)
        {
            int playerNum = 4;

            
            _drawDeck = new Deck();
            _playerHands = new List<Hand>();
            _cardsInMiddle = new List<Card>();
            _allPlayers = new List<Player>();

            _allPlayers = allPlayers;

            DrawDeck.Shuffle();
            PutFirstCardInMIddle();

            for (int i = 0; i < playerNum; i++)
            {
                PlayerHands.Add(new Hand());

            }
        }



        //Properties:
        public List<Hand> PlayerHands
        {
            get { return _playerHands; }
        }

        public Card CardInMiddle
        {
            get { return _cardsInMiddle[0]; }
        }

        public Deck DrawDeck
        {
            get { return _drawDeck; }
        }

        public List<Player> AllPlayers
        {
            get { return _allPlayers; }
        }



        //Main methods:

        /*  The Deal() method distributes 7 cards to each of the 4 players from the draw deck.
         *  
         *  Parameters: None
         *  Returns: void
         *  
         *  Algorithm:
         *    - Loop through each of the 4 players.
         *    - For each player, draw 7 cards from the deck.
         *    - Add each card to the respective player’s hand.
         */
        public void Deal()
        {
            int cardsPerPlayer = 7;
            int numPlayers = 4;

            for(int i = 0;i < numPlayers;i++)
            {
                for(int j = 0;j < cardsPerPlayer;j++)
                {
                    Card dealtCard = _drawDeck.Draw();
                    _playerHands[i].AddCard(dealtCard);
                }
            }
        }

        /*  The DrawOneCard() method draws and returns one card from the draw deck.
         *  
         *  Parameters: None
         *  Returns: Card (the card that was drawn)
         *  
         *  Algorithm:
         *    - Draw the top card from the draw deck.
         *    - Return the drawn card.
         */
        public Card DrawOneCard()
        {
            Card drawnCard = _drawDeck.Draw();

            return drawnCard;
        }

        /*  The DrawCards() method forces a player to draw cards based on the value of a PlusTwo or PlusFour card.
         *  
         *  Parameters:
         *    - plusCard (Card): The card causing the draw action.
         *    - playerHand (Hand): The hand of the player who must draw cards.
         *  Returns: void
         *  
         *  Algorithm:
         *    - If the card is PlusTwo, draw 2 cards.
         *    - If the card is PlusFour, draw 4 cards.
         *    - Add each drawn card to the specified player's hand.
         */
        public void DrawCards(Card plusCard, Hand playerHand)
        {
            if(plusCard.Rank == Rank.PlusTwo) //+2
            {
                for(int i = 0; i < 2; i++)
                {
                    Card drawnCard = _drawDeck.Draw(); //Card is drawn from the deck
                    playerHand.AddCard(drawnCard); //Card is added to the player's hand
                }
            }
            else if(plusCard.Rank == Rank.PlusFour) //+4
            {
                for(int i = 0; i < 4; i++)
                {
                    Card drawnCard = _drawDeck.Draw(); //Card is drawn from the deck
                    playerHand.AddCard(drawnCard); //Card is added to the player's hand
                }
            }
        }

        /*  The PlayCard() method plays a card, updates the card in the middle, and triggers any card effects (draw or color change).
         *  
         *  Parameters:
         *    - card (Card): The card being played.
         *    - playerHand (Hand): The player playing the card.
         *    - nextPlayerHand (Hand): The next player, in case draw cards apply to them.
         *  Returns: void
         *  
         *  Algorithm:
         *    - Remove the card from the current player’s hand.
         *    - Add the card to the top of the discard pile (_cardsInMiddle).
         *    - If the card is PlusTwo or PlusFour, make the next player draw cards.
         *    - If the card is ChangeColor or PlusFour, prompt the user to choose a color.
         */
        public void PlayCard(Card card, Hand playerHand, Hand nextPlayerHand)
        {
            playerHand.RemoveCard(card);
            _cardsInMiddle.Insert(0, card); //Adding a provided card to the top of the deck

            if(card.Rank == Rank.PlusTwo)
            {
                DrawCards(card, nextPlayerHand);
            }
            if(card.Rank == Rank.PlusFour)
            {
                ChangeColor(card);
                DrawCards(card, nextPlayerHand);           
            }
            if(card.Rank == Rank.ChangeColor)
                ChangeColor(card);
        }

        /*  The DisplayGame() method prints the game board with visible and hidden hands, and the current middle card.
         *  
         *  Parameters:
         *    - playerNum (int): Index of the player whose hand should be visible.
         *    - playersInRound (List<Player>): List of players in the round.
         *  Returns: void
         *  
         *  Algorithm:
         *    - Display the name of each player.
         *    - For the current player, show full hand.
         *    - For others, show a hidden (X) hand.
         *    - Display the card currently in the middle with its color.
         */
        public void DisplayGame(int playerNum, List<Player> playersInRound)
        {
            Console.WriteLine("UNO\n");

            for(int i = 0; i < _playerHands.Count; i++)
            {
                Console.WriteLine($"{playersInRound[i].Name}");

                if (i == playerNum - 1) //Displaying the player's hand (cards visible)
                {
                    _playerHands[i].DisplayHand();
                    Console.WriteLine("\n");

                }
                else //Displaying the other players' hand (cards not visible)
                {
                    _playerHands[i].DisplayHiddenHand();
                    Console.WriteLine("\n");
                }
            }

            Console.WriteLine("\n\n");

            Console.Write("Card in the middle: ");
            Console.ForegroundColor = GetCardColor(CardInMiddle);
            Console.Write($"{CardInMiddle} ");

            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("\n");

        }

        /*  The DisplayGameOver() method shows each player's final hand and hand value at the end of the game.
         *  
         *  Parameters:
         *    - playersInRound (List<Player>): Players in the current round.
         *    - handValueList (List<int>): Their final hand scores.
         *  Returns: void
         *  
         *  Algorithm:
         *    - For each player, display their name, final hand value, and cards in hand.
         */
        public void DisplayGameOver(List<Player> playersInRound, List<int> handValueList)
        {
            for (int i = 0; i < _playerHands.Count; i++)
            {
                Console.WriteLine($"{playersInRound[i].Name}  (Hand Value = {handValueList[i]})");
                _playerHands[i].DisplayHand();
                Console.WriteLine("\n");
            }
        }

        /*  The PutFirstCardInMIddle() method sets the first valid card (not special) as the starting card in the middle.
         *  
         *  Parameters: None
         *  Returns: void
         *  
         *  Algorithm:
         *    - Draw a card from the deck.
         *    - If it is a special card (like Skip or PlusFour), return it to the deck and draw another.
         *    - Repeat until a normal card is found.
         *    - Add it to the middle pile.
         */
        public void PutFirstCardInMIddle()
        {
            Rank[] specialRanks = { Rank.Skip, Rank.Reverse, Rank.PlusTwo, Rank.PlusFour, Rank.ChangeColor };

            Card card = _drawDeck.Draw();

            while (specialRanks.Contains(card.Rank))
            {
                _drawDeck.ReputCardInDeck(card);
                card = DrawOneCard();
            }

            _cardsInMiddle.Add(card);
        }

        /*  The CanYouPlay() method checks if the player has any playable cards.
         *  
         *  Parameters:
         *    - hand (List<Card>): The player's hand.
         *    - count (ref int): Will be updated with how many playable cards are found.
         *  Returns: bool (true if at least one card can be played)
         *  
         *  Algorithm:
         *    - For each card, check if color or rank matches the middle card, or if it is a Wild card.
         *    - If any match, increment the count.
         *    - Return true if count > 0, else false.
         */
        public bool CanYouPlay(List<Card> hand, ref int count)
        {
            for (int i = 0; i < hand.Count; i++)
            {
                if (CardInMiddle.Color == hand[i].Color)
                    count++;
                else if (CardInMiddle.Rank == hand[i].Rank)
                    count++;
                else if (hand[i].Color == Color.Gray)
                    count++;
            }

            if (count > 0)
                return true;

            return false;
        }

        /*  The CanYouPlay() method checks if a single drawn card is playable.
         *  
         *  Parameters:
         *    - cardDrawn (Card): The drawn card.
         *  Returns: bool (true if it can be played)
         *  
         *  Algorithm:
         *    - Check if card matches in color or rank, or is a Wild card.
         *    - Return true if match found, else false.
         */
        public bool CanYouPlay(Card cardDrawn)
        {
            if (CardInMiddle.Color == cardDrawn.Color)
                return true;
            else if (CardInMiddle.Rank == cardDrawn.Rank)
                return true;
            else if (cardDrawn.Color == Color.Gray)
                return true;

            return false;
        }

        /*  The ChangeColor() method prompts the user to choose a color for a Wild or PlusFour card.
         *  
         *  Parameters:
         *    - specialCard (Card): The card whose color needs to be changed.
         *  Returns: void
         *  Exceptions: None
         *  
         *  Algorithm:
         *    - Ask the user to choose a color.
         *    - Validate the input until a correct color is provided.
         *    - Set the chosen color to the special card.
         */
        public void ChangeColor(Card specialCard)
        {          
            Console.Write("\nChoose a color: ");
            Color colorInput = ColorValidation();

            specialCard.Color = colorInput;   
        }

        public int CalculateFinalHandValue(List<Card> hand)
        {
            Rank[] numberedRanks = { Rank.Zero, Rank.One, Rank.Two, Rank.Three, Rank.Four, Rank.Five, Rank.Six,
                            Rank.Seven, Rank.Eight, Rank.Nine};
            int total = 0;

            for (int i = 0; i < hand.Count; i++)
            {
                if (hand[i].Rank == Rank.Skip || hand[i].Rank == Rank.Reverse || hand[i].Rank == Rank.PlusTwo)
                    total -= 20;
                if (hand[i].Rank == Rank.PlusFour || hand[i].Rank == Rank.ChangeColor)
                    total -= 50;
                if (numberedRanks.Contains(hand[i].Rank))
                    total -= (int)hand[i].Rank;
            }

            return total;
        }



        //Extra methods
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

        public Color ColorValidation()
        {
            Color colorInput;
            bool successfulConversion;

            Color[] colors = { Color.Green, Color.Red, Color.Blue, Color.Yellow };

            successfulConversion = Color.TryParse(Console.ReadLine(), out colorInput);

            while (successfulConversion == false || !colors.Contains(colorInput))
            {
                Console.Write($"What you entered wasn't valid. Try again: ");
                successfulConversion = Color.TryParse(Console.ReadLine(), out colorInput);
            }

            return colorInput;
        }
    }


}
