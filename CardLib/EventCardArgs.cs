using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardLib
{
    public class EventCardArgs
    {
        private int number;
        private string suit;

        public EventCardArgs( int number, string suit )
        {
            this.number = number;
            this.suit = suit;
        }

        public int Number
        {
            get
            {
                return number;
            }
        }

        public string Suit
        {
            get
            {
                return suit;
            }
        }
    }
}
