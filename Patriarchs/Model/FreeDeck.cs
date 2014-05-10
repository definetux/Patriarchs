using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patriarchs.Model
{
    class FreeDeck: IDeck, IWorkDeck
    {
        const int FREE_CARDS_COUNT = 9;

        private List<Card> listOfCard;
        private int lastAddedNumber;

        public FreeDeck()
        {
            listOfCard = new List<Card>();
            lastAddedNumber = 0;

            for (int i = 0; i < FREE_CARDS_COUNT; i++)
                listOfCard.Add( null );
        }

        public Card GetFirstCard(bool isRemove, int number)
        {
            if (listOfCard.Count != 0)
            {
                Card firstCard = listOfCard[ number ];
                if (isRemove == true)
                    listOfCard.Remove(firstCard);
                return firstCard;
            }
            else
                return null;
        }

        public int GetDeckSize()
        {
            return listOfCard.Count;
        }

        public void RemoveCard(Card card)
        {
            if (listOfCard.Count != 0)
                for( int i = 0; i < FREE_CARDS_COUNT; i++ )
                    if( listOfCard[ i ] == card )
                        listOfCard[ i ] = null;
        }

        public void SetCard( Card card, int number = -1 ) 
        {
            if( number != -1 )
            {
                listOfCard[ number ] = card;
                lastAddedNumber = number;
            }
            else
            {
                for(int i = 0; i < FREE_CARDS_COUNT; i++ )
                    if( listOfCard[ i ] == null )
                    {
                        listOfCard[ i ] = card;
                        lastAddedNumber = i;
                        break;
                    }
            }

        }

        public int GetLastAdded( )
        {
            return lastAddedNumber;
        }

        public bool CheckSize()
        {
            foreach( var item in listOfCard )
            {
                if ( item == null )
                    return false;
            }
            return true;
        }
    }
}
