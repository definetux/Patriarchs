using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Patriarchs.Model
{
    public class Card
    {
        private int number;
        private string suit;
        private CardLib.CardCtrl control;
        private bool isActive;

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

        public void SetPathToImage( string path )
        {
            Uri imageUri = new Uri( path, UriKind.Relative );
            BitmapImage imageBitmap = new BitmapImage( imageUri );
            control.ImgSource = imageBitmap;
        }
    }
}
