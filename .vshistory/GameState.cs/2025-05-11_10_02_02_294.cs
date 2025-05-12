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


        //Constructor
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

        //Properties
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


        //Methods:
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

        public Card DrawOneCard()
        {
            Card drawnCard = _drawDeck.Draw();

            return drawnCard;
        }

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

        public void OrderHands()
        {
            foreach(Hand hand in _playerHands)
            {
                hand.SortHandByColor();
            }
        }

        public void DisplayGame(int playerNum, List<Player> playersInRound)
        {
            Console.WriteLine("UNO\n");

            for(int i = 0; i < _playerHands.Count; i++)
            {
                Console.WriteLine($"{playersInRound[i].Name}");

                if (i == playerNum - 1)
                {
                    _playerHands[i].DisplayHand();
                    Console.WriteLine("\n");

                }
                else
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
