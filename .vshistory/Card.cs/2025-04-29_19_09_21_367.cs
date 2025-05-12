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


        //Properties (Readonly):
        public Color Color
        {
            get { return _color; }
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
                card = "[ ⟳ ]";
            if (this.Rank == Rank.Skip)
                card = "[ ⤬ ]";
            if (this.Rank == Rank.ChangeColor)
                card = "[ ◈ ]";
            if (this.Rank == Rank.PlusTwo)
                card = "[ +2 ]";
            if (this.Rank == Rank.PlusFour)
                card = "[ +4 ]";
            else
                card = $"[ {(int)this.Rank} ]";


            return card;
        }
    }
}
