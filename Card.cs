using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edee_Final_Project
{
    internal class Card
    {
        //Fields:
        private Color _color;
        private Rank _rank;


        //Properties:
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }
        
        public Rank Rank
        {
            get { return _rank; }
        }


        //Constructor:
        public Card(Color color, Rank rank)
        {
            _color = color;
            _rank = rank;
        }


        //Override methods
        public override string ToString()
        {                             
            string card = "";

            if (this.Rank == Rank.Reverse)
                card = "[ \u27f3 ]";
            if (this.Rank == Rank.Skip)
                card = "[ \u292c ]";
            if (this.Rank == Rank.ChangeColor)
                card = "[ \u25c8 ]";
            if (this.Rank == Rank.PlusTwo)
                card = "[ +2 ]";
            if (this.Rank == Rank.PlusFour)
                card = "[ +4 ]";
            else if((int)this.Rank >= 0 && (int)this.Rank <= 9)
                card = $"[ {(int)this.Rank} ]";

            return card;
        }


        //Overloaded operators:
        public static bool operator > (Card card1, Card card2)
        {
            return (int)card1.Color > (int)card2.Color;
        }

        public static bool operator < (Card card1, Card card2)
        {
            return (int)card1.Color < (int)card2.Color;
        }


    }
}
