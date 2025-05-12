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


        //Constructor
        public GameState()
        {
            int playerNum = 4;

            _drawDeck = new Deck();
            _playerHands = new List<Hand>();

            _drawDeck.Shuffle();

            for(int i = 0; i < playerNum; i++)
            {
                _playerHands.Add(new Hand());
            }
        }


        //Methods:
        public void Deal()
        {
            int cardsPerPlayer = 7;

            for(int i = 0;i < cardsPerPlayer;i++)
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

        public void OrderHands()
        {
            foreach(Hand hand in _playerHands)
            {
                hand.SortHandByColor();
            }
        }
    }


}
