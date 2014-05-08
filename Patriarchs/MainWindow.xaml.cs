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

namespace Patriarchs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Point m_anchorPoint;
        Point m_currentPoint;
        bool isDrag = false;

        private TranslateTransform currentTransform;

        public MainWindow( )
        {
            InitializeComponent( );
        }

        private void mnuExit_Click( object sender, RoutedEventArgs e )
        {
            this.Close( );
        }

        private void CardCtrl_MouseDown( object sender, MouseButtonEventArgs e )
        {
            var element = sender as FrameworkElement;
            m_anchorPoint = e.GetPosition( null );
            element.CaptureMouse( );
            isDrag = true;
            e.Handled = true;
            currentTransform = new TranslateTransform( );
            CardLib.CardCtrl card = sender as CardLib.CardCtrl;
            var parent = card.Parent as Grid;
            Canvas.SetZIndex( parent, 1000 );
            Canvas.SetZIndex( card, 1001 );
        }

        private void CardCtrl_MouseMove( object sender, MouseEventArgs e )
        {
            if( isDrag )
            {
                CardLib.CardCtrl card = sender as CardLib.CardCtrl;

                m_currentPoint = e.GetPosition( null );

                currentTransform.X += m_currentPoint.X - m_anchorPoint.X;
                currentTransform.Y += ( m_currentPoint.Y - m_anchorPoint.Y );



                ( ( CardLib.CardCtrl )sender ).RenderTransform = currentTransform;
                m_anchorPoint = m_currentPoint;
            }
        }

        private void CardCtrl_MouseUp( object sender, MouseButtonEventArgs e )
        {
            if( isDrag )
            {
                var element = sender as FrameworkElement;
                element.ReleaseMouseCapture( );
                isDrag = false;
                e.Handled = true;
                currentTransform.X = 0;
                currentTransform.Y = 0;

                CardLib.CardCtrl card = sender as CardLib.CardCtrl;
                var parent = card.Parent as Grid;
                Canvas.SetZIndex( parent, 1 );
                Canvas.SetZIndex( card, 2 );

                
                card.OnDropCard( card, new CardLib.EventCardArgs( 2, "Hearts" ) );
            }
        }

        private void CardCtrl_CardDropped( object sender, CardLib.EventCardArgs e )
        {          
            MessageBox.Show( e.Number + e.Suit );
        }

        private void CardCtrl_DropCard_1( object sender, CardLib.EventCardArgs e )
        {
            var card = sender as CardLib.CardCtrl;
            var cardParent = card.Parent as Grid;
            
            switch( e.Suit )
            {
                case "Hearts":
                    {
                        kingDeckFirst.OnCardDroped( card, e );
                        cardParent.Children.Remove( card );
                        kingsDeckPanel.Children.Add( card );
                        Grid.SetRow( card, 0 );
                        Grid.SetColumn( card, 0 );
                    }
                    break;
                case "Clubs":
                    {
                        kingDeckThird.OnCardDroped( card, e );
                        cardParent.Children.Remove( card );
                        kingsDeckPanel.Children.Add( card );
                        Grid.SetRow( card, 2 );
                        Grid.SetColumn( card, 0 );
                    }
                    break;
                case "Diamonds":
                    {
                        kingDeckSecond.OnCardDroped( card, e );
                        cardParent.Children.Remove( card );
                        kingsDeckPanel.Children.Add( card );
                        Grid.SetRow( card, 1 );
                        Grid.SetColumn( card, 0 );
                    }
                    break;
                case "Spades":
                    {
                        kingDeckFourth.OnCardDroped( card, e );
                        cardParent.Children.Remove( card );
                        kingsDeckPanel.Children.Add( card );
                        Grid.SetRow( card, 3 );
                        Grid.SetColumn( card, 0 );
                    }
                    break;
            };
            
        }
    }
}
