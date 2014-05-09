using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patriarchs.Model
{
    class ToLowerDeck: WorkDeck
    {
        public ToLowerDeck( string suit )
            : base( suit )
        {
            int number = 13;
            string pathToImage = Properties.Resources.PathToCards
                                    + '/'
                                    + suit
                                    + '/'
                                    + number.ToString( )
                                    + ".png";
            listOfCards.Add( new Card( number, suit, pathToImage ) );
        }
    }
}
