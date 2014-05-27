using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Patriarchs.Model
{
    /// <summary>
    /// Карта
    /// </summary>
    public class Card
    {
        private int number;
        private string suit;
        private CardLib.CardCtrl control;
        private bool isActive;

        /// <summary>
        /// Инициализация карты
        /// </summary>
        /// <param name="number"> Номинал карты </param>
        /// <param name="suit"> Масть </param>
        /// <param name="image"> Изображение лицевой стороны </param>
        public Card( int number, string suit, string image )
        {
            this.number = number;
            this.suit = suit;
            isActive = false;
            control = new CardLib.CardCtrl( );

            Uri imageUri = new Uri( image, UriKind.Relative );
            BitmapImage imageBitmap = new BitmapImage( imageUri );
            control.ImgSource = imageBitmap;
        }

        /// <summary>
        /// Номинал карты
        /// </summary>
        public int Number
        {
            get
            {
                return number;
            }
            set
            {
                number = value;
            }
        }

        /// <summary>
        /// Масть карты
        /// </summary>
        public string Suit
        {
            get
            {
                return suit;
            }
            set
            {
                suit = value;
            }
        }

        /// <summary>
        /// Представление карты
        /// </summary>
        public CardLib.CardCtrl CardControl
        {
            get
            {
                return control;
            }
            set
            {
                control = value;
            }
        }

        /// <summary>
        /// Активность карты
        /// </summary>
        public bool IsActive
        {
            get
            {
                return isActive;
            }
            set
            {
                isActive = value;
            }
        }

        /// <summary>
        /// Установить лицевую сторону карты
        /// </summary>
        /// <param name="path"></param>
        public void SetPathToImage( string path )
        {
            Uri imageUri = new Uri( path, UriKind.Relative );
            BitmapImage imageBitmap = new BitmapImage( imageUri );
            control.ImgSource = imageBitmap;
        }
    }
}
