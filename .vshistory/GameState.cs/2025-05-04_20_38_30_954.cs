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


        //Constructor
        public GameState()
        {
            int playerNum = 4;
            Random random = new Random();

            _drawDeck = new Deck();
            _playerHands = new List<Hand>();
            _cardsInMiddle = new List<Card>();

            _drawDeck.Shuffle();

            for(int i = 0; i < playerNum; i++)
            {
                _playerHands.Add(new Hand());
                
            }
        }

        //Properties
        public List<Hand> PlayerHands
        {
            get { return _playerHands; }
        }

        public List<Card> CardsInMiddle
        {
            get { return _cardsInMiddle; }
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

        public void PlayCard(Card card, Hand playerHand)
        {
            playerHand.RemoveCard(card);
            _cardsInMiddle.Insert(0, card); //Adding a provided card to the top of the deck
        }

        public void OrderHands()
        {
            foreach(Hand hand in _playerHands)
            {
                hand.SortHandByColor();
            }
        }

        public void DisplayGame()
        {
            Console.WriteLine("UNO\n");

            for(int i = 0; i < _playerHands.Count; i++)
            {
                Console.WriteLine($"Player {i + 1}");

                if (i == 0)
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

            Console.Write("Card in the middle: ");

            if( _cardsInMiddle.Count > 0 )
            {
                Console.ForegroundColor = GetCardColor(_cardsInMiddle[0]);
                Console.WriteLine(_cardsInMiddle[0]);
                Console.ForegroundColor = ConsoleColor.White;
            }


            Console.WriteLine();
            Console.WriteLine($"There are still {_drawDeck.CardsLeft} cards left in the deck");

        }

        public void DisplayGame2()
        {
            Console.WriteLine("UNO\n");

            

            Console.Write("Card in the middle: ");

            if (_cardsInMiddle.Count > 0)
            {
                Console.ForegroundColor = GetCardColor(_cardsInMiddle[0]);
                Console.WriteLine(_cardsInMiddle[0]);
                Console.ForegroundColor = ConsoleColor.White;
            }


            Console.WriteLine();
            Console.WriteLine($"There are still {_drawDeck.CardsLeft} cards left in the deck");

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
    }


}
