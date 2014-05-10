using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patriarchs.Model
{
    enum E_SUIT
    {
        HEARTS,
        DIAMONDS,
        CLUBS,
        SPADES
    }
    abstract class WorkDeck: IDeck, IWorkDeck
    {
        public static string[] Suits = { "Hearts", "Diamonds", "Clubs", "Spades" };

        protected string suit;

        protected List<Card> listOfCards;

        public WorkDeck( string suit )
        {
            this.suit = suit;
            listOfCards = new List<Card>( );
        }

        public Card GetFirstCard( bool isRemove, int number = 0 )
        {
            var card = listOfCards.First();
            if( isRemove == true )
                listOfCards.Remove( card );
            return card;
        }

        public void SetCard( Card card, int number = 0 )
        {
            listOfCards.Insert( 0, card );
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
