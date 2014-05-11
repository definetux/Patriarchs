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
using Patriarchs.Model;

namespace Patriarchs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const double CARD_WIDTH = 90;
        const double CARD_HEIGHT = 130;
        const int CARDS_COUNT = 96; // 408

        private Point m_anchorPoint;
        private Point m_currentPoint;
        private bool isDrag = false;
        private BaseDeck baseDeck;
        private GivingDeck givingDeck;
        private List<ToUpperDeck> upperDecks;
        private List<ToLowerDeck> lowerDecks;
        private Card currentCard;
        private IDeck currentDeck;
        private FreeDeck freeCards;


        private TranslateTransform currentTransform;

        public MainWindow( )
        {
            InitializeComponent( );
            BuildBaseDeck( );
            BuildFreeCards( );
            BuildUpperDecks( );
            BuildLowerDecks( );
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

            SetCurrentCard( card );
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
                Canvas.SetZIndex( parent, 0 );
                Canvas.SetZIndex( card, 0 );

                card.OnDropCard( card, new CardLib.EventCardArgs( currentCard.Number, currentCard.Suit ) );
            }
        }

        private void CardCtrl_CardDropped( object sender, CardLib.EventCardArgs e )
        {          
            MessageBox.Show( e.Number + e.Suit );
        }

        private void SetCurrentCard( CardLib.CardCtrl card )
        {
            var parent = card.Parent as Grid;
            switch( parent.Name )
            {
                case "workSpaceGrid":
                    {
                        int cardRow = Grid.GetRow( card );
                        int cardCol = Grid.GetColumn( card );
                        int size = parent.RowDefinitions.Count;
                        currentCard = freeCards.GetFirstCard( false, cardRow * size + cardCol );
                        currentDeck = freeCards;
                    }
                    break;
                case "untouchedDeckPanel":
                    {
                        currentCard = givingDeck.GetFirstCard( false );
                        currentDeck = givingDeck;
                    }
                    break;
                case "acesDeckPanel":
                    {
                        int cardRow = Grid.GetRow( card );
                        currentCard = upperDecks[ cardRow ].GetFirstCard( false );
                        currentDeck = upperDecks[ cardRow ];
                    }
                    break;
                case "kingsDeckPanel":
                    {
                        int cardRow = Grid.GetRow( card );
                        currentCard = lowerDecks[ cardRow ].GetFirstCard( false );
                        currentDeck = lowerDecks[ cardRow ];
                    }
                    break;
            }
        }

        private bool CheckLowerDeck( int row )
        {
            var c = lowerDecks[ row ].GetFirstCard( false );
            return CheckPosition( c.CardControl ) && ( CheckCardNumber( c ) == -1 );

        }

        private bool CheckUpperDeck( int row )
        {
            var c = upperDecks[ row ].GetFirstCard( false );
            return CheckPosition( c.CardControl ) && ( CheckCardNumber( c ) == 1 ) ;
        }

        private int CheckCardNumber( Card card )
        {
            return currentCard.Number - card.Number;
        }

        private bool CheckPosition( IInputElement element )
        {
            Point p = Mouse.GetPosition( element );

            if( p.X > 0 && p.X < CARD_WIDTH && p.Y > 0 && p.Y < CARD_HEIGHT )
                return true;
            else
                return false;
        }

        private void SetCurrentCard( WorkDeck deck )
        {
            if( currentDeck != null )
                currentDeck.RemoveCard( currentCard );
            deck.SetCard( currentCard );
        }

        private void AddToUpper( int row, Grid parent, CardLib.CardCtrl card )
        {
            SetCurrentCard( upperDecks[ row ] );
            parent.Children.Remove( card );
            acesDeckPanel.Children.Add( card );
            Grid.SetRow( card, row );
            Grid.SetColumn( card, row );
        }

        private void AddToLower( int row, Grid parent, CardLib.CardCtrl card )
        {
            SetCurrentCard( lowerDecks[ row ] );
            parent.Children.Remove( card );
            kingsDeckPanel.Children.Add( card );
            Grid.SetRow( card, row );
            Grid.SetColumn( card, row );
        }

        private void AddToResultDeck( int row, Grid parent, CardLib.CardCtrl card )
        {
            if( CheckUpperDeck( row ) )
            {
                AddToUpper( row, parent, card );
            }
            else
                if( CheckLowerDeck( row ) )
                {
                    AddToLower( row, parent, card );
                }
        }

        private void UpdateFreeDeck( )
        {
            if( freeCards.CheckSize( ) == false )
            {
                Card card = null;
                if( givingDeck.GetDeckSize( ) > 0 )
                {
                    card = givingDeck.GetFirstCard( true );
                    untouchedDeckPanel.Children.Remove( card.CardControl );
                }
                else
                    if( baseDeck.GetDeckSize( ) > 0 )
                    {
                        card = baseDeck.GetFirstCard(true);
                        untouchedDeckPanel.Children.Remove(card.CardControl);
                        if( baseDeck.GetDeckSize() != 0 )
                            SetFirstBaseCard( baseDeck.GetFirstCard( false ) );
                    }

                if( card != null )
                {
                    AddToFree( card, -1 );

                    int number = freeCards.GetLastAdded( );
                    workSpaceGrid.Children.Add( card.CardControl );
                    Grid.SetColumn( card.CardControl, number % workSpaceGrid.RowDefinitions.Count );
                    Grid.SetRow( card.CardControl, number / workSpaceGrid.RowDefinitions.Count );
                }
            }
        }

        private void CardCtrl_DropCard( object sender, CardLib.EventCardArgs e )
        {
            var card = sender as CardLib.CardCtrl;
            var cardParent = card.Parent as Grid;
            switch( e.Suit )
            {
                case "Hearts":
                    {
                        AddToResultDeck( ( int )E_SUIT.HEARTS, cardParent, card );
                    }
                    break;
                case "Diamonds":
                    {
                        AddToResultDeck( ( int )E_SUIT.DIAMONDS, cardParent, card );
                    }
                    break;
                case "Clubs":
                    {
                        AddToResultDeck( ( int )E_SUIT.CLUBS, cardParent, card );
                    }
                    break;
                case "Spades":
                    {
                        AddToResultDeck( ( int )E_SUIT.SPADES, cardParent, card );
                    }
                    break;
            };
            UpdateFreeDeck( );
        }

        private void BuildBaseDeck( )
        {
            try
            {
                baseDeck = new BaseDeck( CARDS_COUNT, Properties.Resources.PathToShirts );
            }
            catch( Exception e )
            {
                MessageBox.Show( e.Message );
            }
            givingDeck = new GivingDeck( );

            SetFirstBaseCard( baseDeck.GetFirstCard( false ) );
        }

        private void BuildUpperDecks( )
        {
            upperDecks = new List<ToUpperDeck>( );
            for( int i = 0; i < 4; i++ )
            {
                ToUpperDeck deck = new ToUpperDeck( WorkDeck.Suits[i] );
                upperDecks.Add( deck );
                var card = deck.GetFirstCard( false );
                acesDeckPanel.Children.Add( card.CardControl );
                Grid.SetRow( card.CardControl, i );
            }
        }

        private void BuildLowerDecks( )
        {
            lowerDecks = new List<ToLowerDeck>( );
            for( int i = 0; i < 4; i++ )
            {
                ToLowerDeck deck = new ToLowerDeck( WorkDeck.Suits[ i ] );
                lowerDecks.Add( deck );
                var card = deck.GetFirstCard( false );
                kingsDeckPanel.Children.Add( card.CardControl );
                Grid.SetRow( card.CardControl, i );
            }
        }

        private void BuildFreeCards( )
        {
            freeCards = new FreeDeck( );
            int rows = workSpaceGrid.RowDefinitions.Count;
            for( int i = 0; i < rows * rows; i++ )
            {
                var card = baseDeck.GetFirstCard( true );

                AddToFree( card, i );

                var parent = card.CardControl.Parent as Grid;
                if( parent != null )
                    parent.Children.Remove( card.CardControl );

                workSpaceGrid.Children.Add( card.CardControl );

                Grid.SetColumn( card.CardControl, i % rows );
                Grid.SetRow( card.CardControl, i / rows );
            }
            SetFirstBaseCard( baseDeck.GetFirstCard( false ) );
        }

        private void AddToFree( Card card, int number )
        {
            card.SetPathToImage(Properties.Resources.PathToCards
                                        + '/'
                                        + card.Suit
                                        + '/'
                                        + card.Number.ToString()
                                        + ".png");

            card.CardControl.MouseUp -= untouchedCard_MouseUp;

            card.CardControl.MouseDown += CardCtrl_MouseDown;
            card.CardControl.MouseMove += CardCtrl_MouseMove;
            card.CardControl.MouseUp += CardCtrl_MouseUp;
            card.CardControl.DropCard += CardCtrl_DropCard;

            freeCards.SetCard(card, number);
        }

        private void SetFirstBaseCard( Card untouchedCard )
        {
            untouchedCard.CardControl.MouseUp += untouchedCard_MouseUp;

            var parent = untouchedCard.CardControl.Parent as Grid;
            if( parent != null )
                parent.Children.Remove( untouchedCard.CardControl );

            untouchedDeckPanel.Children.Add( untouchedCard.CardControl );
            Grid.SetRow( untouchedCard.CardControl, 0 );
        }

        void untouchedCard_MouseUp( object sender, MouseButtonEventArgs e )
        {
            var card = baseDeck.GetFirstCard( true );
            card.SetPathToImage( Properties.Resources.PathToCards
                                        + '/'
                                        + card.Suit
                                        + '/'
                                        + card.Number.ToString( )
                                        + ".png" );

            card.CardControl.MouseUp -= untouchedCard_MouseUp;

            card.CardControl.MouseDown += CardCtrl_MouseDown;
            card.CardControl.MouseMove += CardCtrl_MouseMove;
            card.CardControl.MouseUp += CardCtrl_MouseUp;
            card.CardControl.DropCard += CardCtrl_DropCard;

            Grid.SetRow( card.CardControl, 1 );

            givingDeck.SetCard( card );

            var newUntouchedCard = baseDeck.GetFirstCard( false );
            if( newUntouchedCard != null )
            {
                SetFirstBaseCard( newUntouchedCard );
            }
        }

        private void Back_MouseUp( object sender, MouseButtonEventArgs e )
        {
            int count = givingDeck.GetDeckSize( );

            if( count == 0 )
                return;

            for( int i = 0; i < count; i++ )
            {
                var card = givingDeck.GetFirstCard( true );
                baseDeck.SetCard( card );
                Grid.SetRow( card.CardControl, 0 );
                card.CardControl.MouseDown -= CardCtrl_MouseDown;
                card.CardControl.MouseMove -= CardCtrl_MouseMove;
                card.CardControl.MouseUp -= CardCtrl_MouseUp;
                card.CardControl.DropCard -= CardCtrl_DropCard;
            }

            SetFirstBaseCard( baseDeck.GetFirstCard( false ) );
        }
    }
}
