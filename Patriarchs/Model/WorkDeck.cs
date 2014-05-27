using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patriarchs.Model
{
    /// <summary>
    /// Масти
    /// </summary>
    enum E_SUIT
    {
        HEARTS,
        DIAMONDS,
        CLUBS,
        SPADES
    }

    /// <summary>
    /// Абстрактный класс рабочей колоды
    /// </summary>
    abstract class WorkDeck: IDeck
    {
        public static string[] Suits = { "Hearts", "Diamonds", "Clubs", "Spades" };

        protected string suit;

        protected List<Card> listOfCards;

        /// <summary>
        /// Инициализация колоды
        /// </summary>
        /// <param name="suit"></param>
        public WorkDeck( string suit )
        {
            this.suit = suit;
            listOfCards = new List<Card>( );
        }

        /// <summary>
        /// Получить первую карту в колоде
        /// </summary>
        /// <param name="isRemove"> Флаг удаления карты </param>
        /// <param name="number"> Номер карты в колоде </param>
        /// <returns></returns>
        public Card GetFirstCard( bool isRemove, int number = 0 )
        {
            var card = listOfCards.First();
            if( isRemove == true )
                listOfCards.Remove( card );
            return card;
        }

        /// <summary>
        /// Установить карту в колоду
        /// </summary>
        /// <param name="card"> Карта </param>
        /// <param name="number"> Номер карты в колоде </param>
        public void SetCard( Card card, int number = 0 )
        {
            listOfCards.Insert( 0, card );
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
        /// Получить номер последней добавленной карты
        /// </summary>
        /// <returns></returns>
        public int GetLastAdded( )
        {
            return GetDeckSize( ) - 1;
        }
    }
}
