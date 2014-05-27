using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Patriarchs.Model;

namespace Patriarchs
{
    /// <summary>
    /// Класс состояния игры
    /// </summary>
    class TransitStation
    {
        /// <summary>
        /// Текущая таблица
        /// </summary>
        public Grid GridLocation
        {
            get;
            set;
        }

        /// <summary>
        /// Текущая строка
        /// </summary>
        public int GridRow
        {
            get;
            set;
        }

        /// <summary>
        /// Текущая колонка
        /// </summary>
        public int GridColumn
        {
            get;
            set;
        }

        /// <summary>
        /// Новая строка 
        /// </summary>
        public int NewGridRow
        {
            get;
            set;
        }

        /// <summary>
        /// Новая колонка
        /// </summary>
        public int NewGridColumn
        {
            get;
            set;
        }

        /// <summary>
        /// Новая таблица
        /// </summary>
        public Grid NewGridLocation
        {
            get;
            set;
        }

        /// <summary>
        /// Карта
        /// </summary>
        public Card Card
        {
            get;
            set;
        }

        /// <summary>
        /// Колода карты
        /// </summary>
        public IDeck Deck
        {
            get;
            set;
        }

        /// <summary>
        /// Время игры
        /// </summary>
        public DateTime Time
        {
            get;
            set;
        }

        /// <summary>
        /// Очки игрока
        /// </summary>
        public int Score
        {
            get;
            set;
        }

        /// <summary>
        /// Новая колода карты
        /// </summary>
        public IDeck NewDeck
        {
            get;
            set;
        }

        /// <summary>
        /// Количество заполненных результирующих колод
        /// </summary>
        public int FullDecks
        {
            get;
            set;
        }
    }
}
