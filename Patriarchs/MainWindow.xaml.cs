﻿using System;
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
        Point m_anchorPoint;
        Point m_currentPoint;
        bool isDrag = false;
        BaseDeck baseDeck;
        GivingDeck givingDeck;
        List<ToUpperDeck> upperDecks;
        List<ToLowerDeck> lowerDecks;
        List<Card>  freeCards;
        Card currentCard;
        IDeck currentDeck;


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
                        //kingDeckFirst.OnCardDroped( card, e );
                        //cardParent.Children.Remove( card );
                        //kingsDeckPanel.Children.Add( card );
                        //Grid.SetRow( card, 0 );
                        //Grid.SetColumn( card, 0 );
                    }
                    break;
                case "Clubs":
                    {
                        //kingDeckThird.OnCardDroped( card, e );
                        //cardParent.Children.Remove( card );
                        //kingsDeckPanel.Children.Add( card );
                        //Grid.SetRow( card, 2 );
                        //Grid.SetColumn( card, 0 );
                    }
                    break;
                case "Diamonds":
                    {
                        //kingDeckSecond.OnCardDroped( card, e );
                        //cardParent.Children.Remove( card );
                        //kingsDeckPanel.Children.Add( card );
                        //Grid.SetRow( card, 1 );
                        //Grid.SetColumn( card, 0 );
                    }
                    break;
                case "Spades":
                    {
                        //kingDeckFourth.OnCardDroped( card, e );
                        //cardParent.Children.Remove( card );
                        //kingsDeckPanel.Children.Add( card );
                        //Grid.SetRow( card, 3 );
                        //Grid.SetColumn( card, 0 );
                    }
                    break;
            };
            
        }

        private void BuildBaseDeck( )
        {
            baseDeck = new BaseDeck( 12, Properties.Resources.PathToShirts );
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
            freeCards = new List<Card>( );
            int rows = workSpaceGrid.RowDefinitions.Count;
            for( int i = 0; i < rows * rows; i++ )
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
                card.CardControl.DropCard += CardCtrl_DropCard_1;

                var parent = card.CardControl.Parent as Grid;
                if( parent != null )
                    parent.Children.Remove( card.CardControl );

                workSpaceGrid.Children.Add( card.CardControl );


                Grid.SetColumn( card.CardControl, i % rows );
                Grid.SetRow( card.CardControl, i / rows );
            }
            SetFirstBaseCard( baseDeck.GetFirstCard( false ) );
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
            card.CardControl.DropCard += CardCtrl_DropCard_1;

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
            for( int i = 0; i < count; i++ )
            {
                var card = givingDeck.GetFirstCard( true );
                baseDeck.SetCard( card );
                Grid.SetRow( card.CardControl, 0 );
                card.CardControl.MouseDown -= CardCtrl_MouseDown;
                card.CardControl.MouseMove -= CardCtrl_MouseMove;
                card.CardControl.MouseUp -= CardCtrl_MouseUp;
                card.CardControl.DropCard -= CardCtrl_DropCard_1;
            }

            SetFirstBaseCard( baseDeck.GetFirstCard( false ) );
        }
    }
}
