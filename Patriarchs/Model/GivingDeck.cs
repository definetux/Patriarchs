using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patriarchs.Model
{
    class GivingDeck: IWorkDeck, IDeck
    {
        private List<Card> listOfCards;

        public GivingDeck( )
        {
            listOfCards = new List<Card>( );
        }

        public void SetCard( Card card, int number = 0 )
        {
            listOfCards.Add( card );
        }

        public Card GetFirstCard( bool isRemove, int number = 0 )
        {
            var card = listOfCards.Last( );
            if( isRemove == true )
            {
                listOfCards.Remove( card );
            }
            return card;
        }


        public int GetDeckSize( )
        {
            return listOfCards.Count;
        }


        public void RemoveCard( Card card )
        {
            if( listOfCards.Count != 0 )
                listOfCards.Remove( card );
        }
    }
}
