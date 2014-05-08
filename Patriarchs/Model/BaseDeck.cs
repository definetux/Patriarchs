using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patriarchs.Model
{
    class BaseDeck: IDeck
    {
        private List<Card> listOfCard;

        public BaseDeck( int count, string shirts )
        {
            listOfCard = new List<Card>( );
            Random rand = new Random( );
            for( int i = 0; i < count; i++ )
            {
                int number = rand.Next( 13 ) + 2;
                string suit = "Hearts";
                string pathToImage = "/Images/Cards/Shirts/" + shirts;

                listOfCard.Add( new Card( number, suit, pathToImage ) );
            }

            listOfCard.First( ).IsActive = true;
        }

        public Card GetFirstCard( bool isRemove )
        {
            if( listOfCard != null )
            {
                Card firstCard = listOfCard.First( );
                if( isRemove == true )
                    listOfCard.RemoveAt( 0 );
                return firstCard;
            }
            else
                return null;
        }
    }
}
