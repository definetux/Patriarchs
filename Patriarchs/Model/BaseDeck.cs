using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patriarchs.Model
{
    class BaseDeck: IDeck, IWorkDeck
    {
        private List<Card> listOfCard;
        private string shirts;

        public BaseDeck( int count, string shirts )
        {
            listOfCard = new List<Card>( );
            this.shirts = shirts;
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
            if( listOfCard.Count != 0 )
            {
                Card firstCard = listOfCard.Last( );
                if( isRemove == true )
                    listOfCard.Remove( firstCard );
                return firstCard;
            }
            else
                return null;
        }

        public void SetCard( Card card )
        {
            card.SetPathToImage( "/Images/Cards/Shirts/" + shirts );
            listOfCard.Add( card );
        }


        public int GetDeckSize( )
        {
            return listOfCard.Count;
        }


        public void RemoveCard( Card card )
        {
            if( listOfCard.Count != 0 )
                listOfCard.Remove( card );
        }
    }
}
