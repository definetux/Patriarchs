using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CardLib
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class CardCtrl : UserControl
    {
        /// <summary>
        /// Событие сброса карты
        /// </summary>
        public event EventHandler<EventCardArgs> CardDropped;

        /// <summary>
        /// Событие, карта брошена
        /// </summary>
        public event EventHandler<EventCardArgs> DropCard;

        /// <summary>
        /// Инициализация карты
        /// </summary>
        public CardCtrl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Путь к изображению
        /// </summary>
        public ImageSource ImgSource
        {
            get
            {
                return img.Source;

            }
            set
            {
                img.Source = value;
            }
        }

        /// <summary>
        /// Обработчик сброса карты
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnCardDroped( object sender, EventCardArgs e )
        {
            EventHandler<EventCardArgs> handler = CardDropped;
            if( handler != null )
            {
                handler( sender, e );
            }
        }

        /// <summary>
        /// Обработчик события, карта брошена
        /// </summary>
        /// <param name="sender"> Объект </param>
        /// <param name="e"> Параметры объекта </param>
        public void OnDropCard( object sender, EventCardArgs e )
        {
            EventHandler<EventCardArgs> handler = DropCard;
            if( handler != null )
            {
                handler( sender, e );
            }
        }

        private void img_MouseEnter( object sender, MouseEventArgs e )
        {
            cardBorder.BorderThickness = new Thickness( 2 );
            cardBorder.BorderBrush = Brushes.Gold;
            this.Margin = new Thickness( 0, 8, 0, 8 );
        }

        private void img_MouseLeave( object sender, MouseEventArgs e )
        {
            cardBorder.BorderThickness = new Thickness( 0 );
            this.Margin = new Thickness( 0, 10, 0, 10 );
        }
    }
}
