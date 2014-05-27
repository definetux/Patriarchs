using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patriarchs.Model
{
    /// <summary>
    /// Колода свободных карт
    /// </summary>
    class FreeDeck: IDeck
    {
        const int FREE_CARDS_COUNT = 9;

        private List<Card> listOfCard;
        private int lastAddedNumber;

        /// <summary>
        /// Инициализцаия колоды
        /// </summary>
        public FreeDeck()
        {
            listOfCard = new List<Card>();
            lastAddedNumber = 0;

            for (int i = 0; i < FREE_CARDS_COUNT; i++)
                listOfCard.Add( null );
        }

        /// <summary>
        /// Получить карту в колоде
        /// </summary>
        /// <param name="isRemove"> Флаг удаления карты </param>
        /// <param name="number"> Номер карты </param>
        /// <returns></returns>
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

        /// <summary>
        /// Получить размер колоды
        /// </summary>
        /// <returns></returns>
        public int GetDeckSize()
        {
            return listOfCard.Count;
        }

        /// <summary>
        /// Удалить карту из колоды
        /// </summary>
        /// <param name="card"></param>
        public void RemoveCard(Card card)
        {
            if (listOfCard.Count != 0)
                for( int i = 0; i < FREE_CARDS_COUNT; i++ )
                    if( listOfCard[ i ] == card )
                        listOfCard[ i ] = null;
        }

        /// <summary>
        /// Установить карту в колоде
        /// </summary>
        /// <param name="card"> Карта </param>
        /// <param name="number"> Номер карты в колоде </param>
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

        /// <summary>
        /// Вернуть номер последней добавленной карты
        /// </summary>
        /// <returns> Номер карты </returns>
        public int GetLastAdded( )
        {
            return lastAddedNumber;
        }

        /// <summary>
        /// Проверить размер колоды
        /// </summary>
        /// <returns> Истина если колода заполнена </returns>
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
