using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patriarchs.Model
{
    /// <summary>
    /// Колода отходов
    /// </summary>
    class GivingDeck: IDeck
    {
        private List<Card> listOfCards;

        /// <summary>
        /// Инициализация колоды
        /// </summary>
        public GivingDeck( )
        {
            listOfCards = new List<Card>( );
        }

        /// <summary>
        /// Установить карту 
        /// </summary>
        /// <param name="card"> Карта </param>
        /// <param name="number"> Номер карты в колоде </param>
        public void SetCard( Card card, int number = 0 )
        {
            listOfCards.Add( card );
        }

        /// <summary>
        /// Получить первую карту в колоде
        /// </summary>
        /// <param name="isRemove"> Флаг удаления карты </param>
        /// <param name="number"> Номер карты </param>
        /// <returns> Карта </returns>
        public Card GetFirstCard( bool isRemove, int number = 0 )
        {
            var card = listOfCards.Last( );
            if( isRemove == true )
            {
                listOfCards.Remove( card );
            }
            return card;
        }

        /// <summary>
        /// Получить размер колоды
        /// </summary>
        /// <returns> Размер колоды </returns>
        public int GetDeckSize( )
        {
            return listOfCards.Count;
        }

        /// <summary>
        /// Удалить карту из колоды 
        /// </summary>
        /// <param name="card"> Карта </param>
        public void RemoveCard( Card card )
        {
            if( listOfCards.Count != 0 )
                listOfCards.Remove( card );
        }

        /// <summary>
        /// Вернуть номер последней добавленной карты
        /// </summary>
        /// <returns> Номер карты </returns>
        public int GetLastAdded( )
        {
            return GetDeckSize( ) - 1;
        }
    }
}
