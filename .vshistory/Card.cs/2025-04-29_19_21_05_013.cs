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


        //Override methods
        public override string ToString()
        {
            Console.OutputEncoding = Encoding.UTF8; //Have to put this to get the suit symbols                               
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
                             
            ConsoleColor fontColor = GetCardColor(this);

            return card;
        }
    }
}
