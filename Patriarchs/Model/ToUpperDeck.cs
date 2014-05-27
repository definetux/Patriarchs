using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patriarchs.Model
{
    /// <summary>
    /// Колода от Туза до Короля
    /// </summary>
    class ToUpperDeck: WorkDeck
    {
        /// <summary>
        /// Инициализация колоды
        /// </summary>
        /// <param name="suit"></param>
        public ToUpperDeck( string suit )
            : base( suit )
        {
            int number = 1;
            string pathToImage = Properties.Resources.PathToCards
                                    + '/'
                                    + suit
                                    +'/'
                                    +number.ToString()
                                    + ".png";
            listOfCards.Add( new Card( number, suit, pathToImage ) );
        }
    }
}
