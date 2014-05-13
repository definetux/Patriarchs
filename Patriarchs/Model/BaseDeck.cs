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
        private Random rand;

        public BaseDeck( int count, string shirts )
        {
            listOfCard = new List<Card>( );
            this.shirts = shirts;
            string pathToImage = Properties.Resources.FullPathToShirts + shirts;

            rand = new Random( );

            List<List<int>> sequences = new List<List<int>>( );

            for( int i = 0; i < 8; i++ )
            {
                if( i % 2 == 0 )
                {
                    sequences.Add( GenerateDeck( 1, 13 ) );
                }
                else
                {
                    sequences.Add( GenerateDeck( 2, 14 ) );
                }
            }

            int emptySeq = 0;

            for( int i = 0; i < count; i++ )
            {
                int deckNumber = rand.Next( sequences.Count );

                foreach( var item in sequences )
                {
                    if( item.Count == 0 )
                        emptySeq++;
                }

                while( emptySeq != 8 &&  sequences[ deckNumber ].Count == 0 )
                {
                    deckNumber++;
                    deckNumber %= sequences.Count;
                }

                int number = sequences[ deckNumber ].First( );

                sequences[ deckNumber ].Remove( number );
                string suit = "";
                switch( deckNumber )
                {
                    case 0:
                    case 1:
                        suit = WorkDeck.Suits[ 0 ];
                        break;
                    case 2:
                    case 3:
                        suit = WorkDeck.Suits[ 1 ];
                        break;
                    case 4:
                    case 5:
                        suit = WorkDeck.Suits[ 2 ];
                        break;
                    case 6:
                    case 7:
                        suit = WorkDeck.Suits[ 3 ];
                        break;
                }

                listOfCard.Add( new Card( number, suit, pathToImage ) );
            }

            listOfCard.First( ).IsActive = true;
        }

        public Card GetFirstCard( bool isRemove, int number = 0 )
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

        public void SetCard( Card card, int number = 0 )
        {
            card.SetPathToImage( Properties.Resources.FullPathToShirts + shirts );
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

        private List<int> GenerateDeck( int start, int finish )
        {
            List<int> startSeq = new List<int>();
            List<int> resultSeq = new List<int>();

            for( int i = start; i < finish; i++ )
                startSeq.Add( i );

            while( startSeq.Count != 0 )
            {
                int item = rand.Next( startSeq.Count );
                resultSeq.Add( startSeq[ item ] );
                startSeq.RemoveAt( item );
            }

            return resultSeq;
        }

        public void ChangeShirt()
        {
            shirts = Properties.Settings.Default.Shirt;
            foreach (var item in listOfCard)
            {
                item.SetPathToImage(Properties.Resources.FullPathToShirts + shirts);
            }
        }
    }
}
