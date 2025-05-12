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
            _drawDeck.Shuffle();

            for(int i = 0; i < playerNum; i++)
            {
                _playerHands.Add(new Hand());
            }
        }
    }


}
