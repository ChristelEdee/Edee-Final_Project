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
            _deck = new List<Card>();

            Color[] colors = { Color.Green, Color.Red, Color.Blue, Color.Yellow, Color.Gray };
            Rank[] ranks = { Rank.Zero, Rank.One, Rank.Two, Rank.Three, Rank.Four, Rank.Five, Rank.Six, 
                            Rank.Seven, Rank.Eight, Rank.Nine, Rank.Skip, Rank.Reverse, Rank.PlusTwo, Rank.PlusTwo };

            foreach(Color color in colors)
            {
                foreach(Rank rank in ranks)
                {
                    _deck.Add(new Card(color, rank));
                }
            }
        }


        //Methods:

    }
}
