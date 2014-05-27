using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patriarchs.Model
{
    /// <summary>
    /// Базовая колода
    /// </summary>
    class BaseDeck: IDeck
    {
        private List<Card> listOfCard;
        private List<Card> reserveList;
        private string shirts;
        private Random rand;

        /// <summary>
        /// Инициализация колоды
        /// </summary>
        /// <param name="count"> Количество карт </param>
        /// <param name="shirts"> Путь к изображению рубашки карты</param>
        public BaseDeck( int count, string shirts )
        {
            listOfCard = new List<Card>( );
            reserveList = new List<Card>( );
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

            

            for( int i = 0; i < count; i++ )
            {
                int deckNumber = rand.Next( sequences.Count );

                int emptySeq = 0;

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
                reserveList.Add( new Card( number, suit, pathToImage ) );
            }

            listOfCard.First( ).IsActive = true;
        }

        /// <summary>
        /// Установить резервную колоду
        /// </summary>
        public void SetReserve( )
        {
            listOfCard.RemoveRange( 0, listOfCard.Count );

            int size = reserveList.Count;

            for( int i = 0; i < size; i++ )
            {
                listOfCard.Add( new Card( reserveList[ i ].Number, reserveList[ i ].Suit, reserveList[ i ].CardControl.ImgSource.ToString( ).Split(',')[3] ) );
            }
        }

        /// <summary>
        /// Получить первую карту колоды
        /// </summary>
        /// <param name="isRemove"> Флаг удаление карты из колоды </param>
        /// <param name="number"> Номер карты в колоде </param>
        /// <returns> Первая карта </returns>
        public Card GetFirstCard( bool isRemove, int number = 0 )
        {
            if( listOfCard.Count != 0 )
            {
                Card firstCard = listOfCard.First( );
                if( isRemove == true )
                    listOfCard.Remove( firstCard );
                return firstCard;
            }
            else
                return null;
        }

        /// <summary>
        /// Добавить карту в колоду 
        /// </summary>
        /// <param name="card"> Карта </param>
        /// <param name="number"> Номер в колоде </param>
        public void SetCard( Card card, int number = -1 )
        {
            if( number != -1 )
            {
                listOfCard.Insert(number, card);
            }
            else
            { 
                listOfCard.Add( card );
            }
            card.SetPathToImage( Properties.Resources.FullPathToShirts + shirts );
        }

        /// <summary>
        /// Вернуть размер колоды
        /// </summary>
        /// <returns> Размер колоды </returns>
        public int GetDeckSize( )
        {
            return listOfCard.Count;
        }

        /// <summary>
        /// Удалить карту из колоды
        /// </summary>
        /// <param name="card"> Карта </param>
        public void RemoveCard( Card card )
        {
            if( listOfCard.Count != 0 )
                listOfCard.Remove( card );
        }

        /// <summary>
        /// Генерация колоды
        /// </summary>
        /// <param name="start"> Стартовый номер карты </param>
        /// <param name="finish"> Финишный номер карты </param>
        /// <returns> Список карт </returns>
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

        /// <summary>
        /// Изменить рубашку колоды
        /// </summary>
        public void ChangeShirt()
        {
            shirts = Properties.Settings.Default.Shirt;
            foreach (var item in listOfCard)
            {
                item.SetPathToImage(Properties.Resources.FullPathToShirts + shirts);
            }
        }

        /// <summary>
        /// Вернуть номер последней добавленной карты
        /// </summary>
        /// <returns> Номер карты </returns>
        public int GetLastAdded( )
        {
            return 0;
        }
    }
}
