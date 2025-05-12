using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edee_Final_Project
{
    internal class Deck
    {
        //Field:
        private List<Card> _deck;

        public int CardsLeft
        {
            get { return _deck.Count; }
        }

        //Constructor:
        public Deck()
        {

        }
    }
}
