using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        const double CARD_WIDTH = 90;
        const double CARD_HEIGHT = 130;
        const int CARDS_COUNT = 96;
        const int FULL_DECK = 13;
        const int WIN_COUNT = 8;
        const int SCORE_UP = 20;

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
        private bool isDoubleClick;
        private int scores;
        private DateTime currentTime;
        private System.Windows.Threading.DispatcherTimer dispatcherTimer;
        private int fullDecks;
        private List<TransitStation> transitList;
        private int currentStep;
        private int countBaseDeck;

        private TranslateTransform currentTransform;

        /// <summary>
        /// Инициализация окна
        /// </summary>
        public MainWindow( )
        {

            dispatcherTimer = new System.Windows.Threading.DispatcherTimer( );
            dispatcherTimer.Tick += new EventHandler( dispatcherTimer_Tick );
            dispatcherTimer.Interval = new TimeSpan( 0, 0, 1 );
            dispatcherTimer.Start( );

            InitializeComponent( );
            InitGame( );
        }

        /// <summary>
        /// Очки
        /// </summary>
        public int Scores
        {
            get
            {
                return scores;
            }
            set
            {
                scores = value;
                OnPropertyChanged( "Scores" );
            }
        }

        /// <summary>
        /// Количество заполненных результирующих колод
        /// </summary>
        public int FullDeck
        {
            get
            {
                return fullDecks;
            }
            set
            {
                fullDecks = value;
                OnPropertyChanged( "FullDeck" );
                if( fullDecks > 0 )
                {
                    btnSaveGame.IsEnabled = true;
                    btnSaveGame.Background = Brushes.Aquamarine;
                    if( fullDecks == WIN_COUNT )
                    {
                        SaveResult( );
                        WinningWindow winWnd = new WinningWindow( );
                        winWnd.ShowDialog( );

                        ScoresWindow scoresWnd = new ScoresWindow( );
                        scoresWnd.ShowDialog( );
                    }
                }

            }
        }

        /// <summary>
        /// Текущее время
        /// </summary>
        public string Time
        {
            get;
            set;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            currentTime = currentTime.AddSeconds( 1 );
            Time = currentTime.TimeOfDay.ToString();
            OnPropertyChanged( "Time" );
        }   

        /// <summary>
        /// Событие изменения свойств
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Обработчик события изменения свойств
        /// </summary>
        /// <param name="info"></param>
        public void OnPropertyChanged( string info )
        {
            if( PropertyChanged != null )
                PropertyChanged( this, new PropertyChangedEventArgs( info ) );
        }

        private void mnuExit_Click( object sender, RoutedEventArgs e )
        {
            this.Close( );
        }

        /// <summary>
        /// Нажатие мыши на карту
        /// </summary>
        /// <param name="sender"> Объект </param>
        /// <param name="e"> Аргументы </param>
        private void CardCtrl_MouseDown( object sender, MouseButtonEventArgs e )
        {
            CardLib.CardCtrl card = sender as CardLib.CardCtrl;
            
            if( e.ChangedButton == MouseButton.Left && e.ClickCount == 2 && isDoubleClick == false )
            {
                SetCurrentCard( card );
                isDoubleClick = true;
                card.OnDropCard( card, new CardLib.EventCardArgs( currentCard.Number, currentCard.Suit ) );
                return;
            }
            SetCurrentCard( card );
            var element = sender as FrameworkElement;
            m_anchorPoint = e.GetPosition( null );
            element.CaptureMouse( );
            isDrag = true;
            e.Handled = true;
            currentTransform = new TranslateTransform( );
            var parent = card.Parent as Grid;
            Canvas.SetZIndex( parent, 1000 );
            Canvas.SetZIndex( card, 1001 );

        }

        /// <summary>
        /// Движение мыши на карте
        /// </summary>
        /// <param name="sender"> Объект </param>
        /// <param name="e"> Аргументы </param>
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

        /// <summary>
        /// Отжатие мыши на карте
        /// </summary>
        /// <param name="sender"> Объект </param>
        /// <param name="e"> Аргументы </param>
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

                card.OnDropCard( card, new CardLib.EventCardArgs( currentCard.Number, currentCard.Suit ) );

                var parent = card.Parent as Grid;

                Canvas.SetZIndex( parent, 0 );
                Canvas.SetZIndex( card, 0 );

            }

            MP3Player.MP3Player.Play( new WindowInteropHelper( this ).Handle );

        }

        private void CardCtrl_CardDropped( object sender, CardLib.EventCardArgs e )
        {          
            MessageBox.Show( e.Number + e.Suit );
        }

        /// <summary>
        /// Установить текущую карту и колоду
        /// </summary>
        /// <param name="card"></param>
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

        /// <summary>
        /// Проверить колоду от Короля до Туза
        /// </summary>
        /// <param name="row"> Номер строки </param>
        /// <returns> Истина, если карта попала в нужную колоду </returns>
        private bool CheckLowerDeck( int row )
        {
            var c = lowerDecks[ row ].GetFirstCard( false );
            return CheckPosition( c.CardControl ) && ( CheckCardNumber( c ) == -1 );

        }

        /// <summary>
        /// Проверить колоду от Туза до Короля
        /// </summary>
        /// <param name="row"> Номер строки </param>
        /// <returns> Истина, если карта попала в нужную колоду </returns>
        private bool CheckUpperDeck( int row )
        {
            var c = upperDecks[ row ].GetFirstCard( false );
            return CheckPosition( c.CardControl ) && ( CheckCardNumber( c ) == 1 ) ;
        }

        /// <summary>
        /// Проверить номинал брошенной карты
        /// </summary>
        /// <param name="card"> Карта </param>
        /// <returns> |1|, если номинал карты соответствует колоде </returns>
        private int CheckCardNumber( Card card )
        {
            return currentCard.Number - card.Number;
        }

        /// <summary>
        /// Проверка положения мыши над картой
        /// </summary>
        /// <param name="element"> Карта </param>
        /// <returns> Истина, если мышь находится над картой </returns>
        private bool CheckPosition( IInputElement element )
        {
            if( isDoubleClick == true )
                return true;

            Point p = Mouse.GetPosition( element );

            if( p.X > 0 && p.X < CARD_WIDTH && p.Y > 0 && p.Y < CARD_HEIGHT )
                return true;
            else
                return false;
        }

        /// <summary>
        /// Установить текущую карту
        /// </summary>
        /// <param name="deck"> Колода </param>
        private void SetCurrentCard( WorkDeck deck )
        {
            if( currentDeck != null )
            {
                currentDeck.RemoveCard( currentCard );
            }

            deck.SetCard( currentCard );
            
        }

        /// <summary>
        /// Добавить карту в колоду от Туза до Короля
        /// </summary>
        /// <param name="row"> Номер строки таблицы </param>
        /// <param name="parent"> Родительская таблица </param>
        /// <param name="card"> Представление карты </param>
        private void AddToUpper( int row, Grid parent, CardLib.CardCtrl card )
        {
            SetCurrentCard( upperDecks[ row ] );
            int diff = transitList.Count - currentStep;
            transitList.RemoveRange( currentStep, diff );
            AddToTransitList( currentCard, currentDeck );
            transitList[ currentStep - 1 ].NewDeck = upperDecks[ row ];

            IncreaseScores( );
            parent.Children.Remove( card );
            acesDeckPanel.Children.Add( card );

            if( upperDecks[ row ].GetDeckSize( ) == FULL_DECK )
                FullDeck++;
            Grid.SetRow( card, row );
            Grid.SetColumn( card, row );

            transitList[ currentStep - 1 ].NewGridLocation = acesDeckPanel;
            transitList[ currentStep - 1 ].NewGridRow = row;
            transitList[ currentStep - 1 ].NewGridColumn = row;
        }

        /// <summary>
        /// Увеличение очков
        /// </summary>
        private void IncreaseScores( )
        {
            Scores += SCORE_UP;
        }

        /// <summary>
        /// Добавить карту в колоду от Короля до Туза
        /// </summary>
        /// <param name="row"> Номер строки таблицы </param>
        /// <param name="parent"> Родительская таблица </param>
        /// <param name="card"> Представление карты </param>
        private void AddToLower( int row, Grid parent, CardLib.CardCtrl card )
        {
            SetCurrentCard( lowerDecks[ row ] );
            int diff = transitList.Count - currentStep;
            transitList.RemoveRange( currentStep, diff );
            AddToTransitList( currentCard, currentDeck );
            transitList[ currentStep - 1 ].NewDeck = lowerDecks[ row ];

            IncreaseScores( );
            parent.Children.Remove( card );
            kingsDeckPanel.Children.Add( card );


            if( lowerDecks[ row ].GetDeckSize( ) == FULL_DECK )
                FullDeck++;
            Grid.SetRow( card, row );
            Grid.SetColumn( card, row );

            transitList[ currentStep - 1 ].NewGridLocation = kingsDeckPanel;
            transitList[ currentStep - 1 ].NewGridRow = row;
            transitList[ currentStep - 1 ].NewGridColumn = row;
        }

        /// <summary>
        /// Добавить карту в результирующую колоду
        /// </summary>
        /// <param name="row"> Номер строки таблицы </param>
        /// <param name="parent"> Родительская таблица </param>
        /// <param name="card"> Представление карты </param>
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

        /// <summary>
        /// Обновить колоду свободных карт
        /// </summary>
        private void UpdateFreeDeck( )
        {
            if( freeCards.CheckSize( ) == false )
            {
                Card card = null;
                if( givingDeck.GetDeckSize( ) > 0 )
                {
                    int diff = transitList.Count - currentStep;
                    transitList.RemoveRange( currentStep, diff );
                    card = givingDeck.GetFirstCard( true );
                    AddToTransitList( card, givingDeck );
                    untouchedDeckPanel.Children.Remove( card.CardControl );
                }
                else
                    if( baseDeck.GetDeckSize( ) > 0 )
                    {
                        int diff = transitList.Count - currentStep;
                        transitList.RemoveRange( currentStep, diff );
                        card = baseDeck.GetFirstCard(true);
                        AddToTransitList( card, baseDeck );
                        untouchedDeckPanel.Children.Remove(card.CardControl);
                        if( baseDeck.GetDeckSize() != 0 )
                            SetFirstBaseCard( baseDeck.GetFirstCard( false ) );
                    }

                if( card != null )
                {
                    AddToFree( card );
                    transitList[ currentStep - 1 ].NewDeck = freeCards;

                    int number = freeCards.GetLastAdded( );
                    workSpaceGrid.Children.Add( card.CardControl );
                    Grid.SetColumn( card.CardControl, number % workSpaceGrid.RowDefinitions.Count );
                    Grid.SetRow( card.CardControl, number / workSpaceGrid.RowDefinitions.Count );

                    transitList[ currentStep - 1 ].NewGridLocation = workSpaceGrid;
                    transitList[ currentStep - 1 ].NewGridRow = number / workSpaceGrid.RowDefinitions.Count;
                    transitList[ currentStep - 1 ].NewGridColumn = number % workSpaceGrid.RowDefinitions.Count;
                }
            }
        }

        /// <summary>
        /// Обработчка события сброса карты
        /// </summary>
        /// <param name="sender"> Объект </param>
        /// <param name="e"> Аргументы </param>
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
            isDoubleClick = false;
        }

        /// <summary>
        /// Добавить карту в список ходов
        /// </summary>
        /// <param name="card"> Карта </param>
        /// <param name="cardDeck"> Колода карты </param>
        private void AddToTransitList( Card card, IDeck cardDeck )
        {
            transitList.Add( new TransitStation
            {
                Card = card,
                Deck = cardDeck,
                GridColumn = Grid.GetColumn( card.CardControl ),
                GridLocation = ( Grid )card.CardControl.Parent,
                GridRow = Grid.GetRow( card.CardControl ),
                Score = Scores,
                Time = currentTime,
                FullDecks = fullDecks
            } );
            currentStep++;
        }

        /// <summary>
        /// Построить базовую колоду
        /// </summary>
        private void BuildBaseDeck( )
        {
            try
            {
                baseDeck = new BaseDeck( CARDS_COUNT, Properties.Settings.Default.Shirt );
            }
            catch( Exception e )
            {
                MessageBox.Show( e.Message );
            }
            givingDeck = new GivingDeck( );

            SetFirstBaseCard( baseDeck.GetFirstCard( false ) );
        }

        /// <summary>
        /// Построить колоды от Туза до Короля
        /// </summary>
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

        /// <summary>
        /// Построить колоды от Короля до Туза
        /// </summary>
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

        /// <summary>
        /// Построить колоду свободных карт
        /// </summary>
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

        /// <summary>
        /// Добавить карту в колоду свободных карт
        /// </summary>
        /// <param name="card"> Карта </param>
        /// <param name="number"> Номер карты в колоде </param>
        private void AddToFree( Card card, int number = -1 )
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

        /// <summary>
        /// Установить первую карту в базовой колоде активной
        /// </summary>
        /// <param name="untouchedCard"> Карта </param>
        private void SetFirstBaseCard( Card untouchedCard )
        {
            untouchedCard.CardControl.MouseUp -= untouchedCard_MouseUp;
            untouchedCard.CardControl.MouseUp += untouchedCard_MouseUp;

            var parent = untouchedCard.CardControl.Parent as Grid;
            if( parent != null )
                parent.Children.Remove( untouchedCard.CardControl );

            untouchedDeckPanel.Children.Add( untouchedCard.CardControl );
            var par = untouchedCard.CardControl.Parent as Grid;

            Grid.SetRow( untouchedCard.CardControl, 0 );
        }

        /// <summary>
        /// Отжатие мыши над картой базовой колоды
        /// </summary>
        /// <param name="sender"> Объект </param>
        /// <param name="e"> Аргументы </param>
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

            int diff = transitList.Count - currentStep;
            transitList.RemoveRange( currentStep, diff );

            AddToTransitList( card, baseDeck );

            var parent = card.CardControl.Parent as Grid;
            parent.Children.Remove(card.CardControl);
            parent.Children.Add(card.CardControl);

            Grid.SetRow( card.CardControl, 1 );

            givingDeck.SetCard( card );
            transitList[ currentStep - 1 ].NewDeck = givingDeck;

            var newUntouchedCard = baseDeck.GetFirstCard( false );
            if( newUntouchedCard != null )
            {
                SetFirstBaseCard( newUntouchedCard );

                transitList[ currentStep - 1 ].NewGridLocation = untouchedDeckPanel;
                transitList[ currentStep - 1 ].NewGridRow = 1;
                transitList[ currentStep - 1 ].NewGridColumn = 0;
            }
        }

        /// <summary>
        /// Отжатие мыши над элементом восстановления базовой колоды
        /// </summary>
        /// <param name="sender"> Объект </param>
        /// <param name="e"> Аргументы </param>
        private void Back_MouseUp( object sender, MouseButtonEventArgs e )
        {
            countBaseDeck++;

            if (countBaseDeck >= 2)
                return;

            int count = givingDeck.GetDeckSize( );

            if( count == 0 )
                return;

            for( int i = 0; i < count; i++ )
            {
                var card = givingDeck.GetFirstCard( true );
                baseDeck.SetCard( card );
                transitList[ currentStep - 1 ].NewDeck = baseDeck;

                Grid.SetRow( card.CardControl, 0 );

                transitList[ currentStep - 1 ].NewGridLocation = untouchedDeckPanel;
                transitList[ currentStep - 1 ].NewGridRow = 0;
                transitList[ currentStep - 1 ].NewGridColumn = 0;

                card.CardControl.MouseDown -= CardCtrl_MouseDown;
                card.CardControl.MouseMove -= CardCtrl_MouseMove;
                card.CardControl.MouseUp -= CardCtrl_MouseUp;
                card.CardControl.DropCard -= CardCtrl_DropCard;
            }

            SetFirstBaseCard( baseDeck.GetFirstCard( false ) );
        }

        private void mnuRules_Click( object sender, RoutedEventArgs e )
        {
            RulesWindow rulesWnd = new RulesWindow( );
            rulesWnd.ShowDialog( );
        }

        private void mnuAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWnd = new AboutWindow();
            aboutWnd.ShowDialog();
        }

        private void mnuShirtColor_Click(object sender, RoutedEventArgs e)
        {
            ShirtsWindow shirtsWnd = new ShirtsWindow();
            shirtsWnd.ShowDialog();
            baseDeck.ChangeShirt();
        }

        private void mnuBackgroundColor_Click(object sender, RoutedEventArgs e)
        {
            DeskImageWindow deskImageWnd = new DeskImageWindow();
            deskImageWnd.ShowDialog();
            string path = "pack://application:,,," + Properties.Resources.PathToTableImage + Properties.Settings.Default.Desk;
            Uri imageUri = new Uri(path, UriKind.Absolute);
            BitmapImage imageBitmap = new BitmapImage(imageUri);
            Background = new ImageBrush( imageBitmap );
        }

        /// <summary>
        /// Сохранить результат в таблицу рекордов
        /// </summary>
        private void SaveResult( )
        {
            StreamWriter sw =  File.AppendText( "scores.txt" );
            sw.WriteLine( "{0}|{1}|{2}", Properties.Settings.Default.PlayerName, Time, Scores.ToString( ) );
            sw.Close( );
        }

        private void btnSaveGame_Click( object sender, RoutedEventArgs e )
        {
            SaveResult( );
        }

        private void mnuSave_Click( object sender, RoutedEventArgs e )
        {
            SaveResult( );
        }

        private void mnuScores_Click( object sender, RoutedEventArgs e )
        {
            ScoresWindow scoresWnd = new ScoresWindow( );
            scoresWnd.ShowDialog( );
        }

        private void btnBack_Click( object sender, RoutedEventArgs e )
        {
            GoBack( );
        }

        /// <summary>
        /// Перейти к предыдущему ходу
        /// </summary>
        private void GoBack( )
        {
            if( currentStep == 0 )
                return;

            var oldStation = transitList[ currentStep - 1 ];
            currentTime = oldStation.Time;
            Scores = oldStation.Score;
            FullDeck = oldStation.FullDecks;
            if( oldStation.Deck is FreeDeck )
                oldStation.Deck.SetCard( oldStation.Card, oldStation.GridLocation.RowDefinitions.Count * oldStation.GridRow + oldStation.GridColumn );
            else
                oldStation.Deck.SetCard( oldStation.Card, oldStation.Deck.GetLastAdded() );
            if( oldStation.NewDeck != null )
                oldStation.NewDeck.RemoveCard( oldStation.Card );
            else
                MessageBox.Show( "test" );

            var oldGrid = oldStation.GridLocation;
            var newGrid = oldStation.Card.CardControl.Parent as Grid;

            newGrid.Children.Remove( oldStation.Card.CardControl );

            oldGrid.Children.Add( oldStation.Card.CardControl );
            Grid.SetColumn( oldStation.Card.CardControl, oldStation.GridColumn );
            Grid.SetRow( oldStation.Card.CardControl, oldStation.GridRow );

            if( oldStation.Deck is BaseDeck )
            {
                oldStation.Card.CardControl.MouseDown -= CardCtrl_MouseDown;
                oldStation.Card.CardControl.MouseMove -= CardCtrl_MouseMove;
                oldStation.Card.CardControl.MouseUp -= CardCtrl_MouseUp;
                oldStation.Card.CardControl.DropCard -= CardCtrl_DropCard;

                oldStation.Card.CardControl.MouseUp -= untouchedCard_MouseUp;
                oldStation.Card.CardControl.MouseUp += untouchedCard_MouseUp;
            }
            else
            {
                oldStation.Card.CardControl.MouseUp -= untouchedCard_MouseUp;

                oldStation.Card.CardControl.MouseDown -= CardCtrl_MouseDown;
                oldStation.Card.CardControl.MouseMove -= CardCtrl_MouseMove;
                oldStation.Card.CardControl.MouseUp -= CardCtrl_MouseUp;
                oldStation.Card.CardControl.DropCard -= CardCtrl_DropCard;
                oldStation.Card.CardControl.MouseDown += CardCtrl_MouseDown;
                oldStation.Card.CardControl.MouseMove += CardCtrl_MouseMove;
                oldStation.Card.CardControl.MouseUp += CardCtrl_MouseUp;
                oldStation.Card.CardControl.DropCard += CardCtrl_DropCard;
            }

            currentStep--;
           // transitList.RemoveAt( currentStep );
        }

        /// <summary>
        /// Инициализация игры
        /// </summary>
        private void InitGame( )
        {
            ClearParams( );

            MP3Player.MP3Player.OpenPlayer( Environment.CurrentDirectory + "\\Sounds\\card_flip2.mp3" );
            MP3Player.MP3Player.SetVolume( 100 );

            string path = "pack://application:,,," + Properties.Resources.PathToTableImage + Properties.Settings.Default.Desk;
            Uri imageUri = new Uri( path, UriKind.Absolute );
            BitmapImage imageBitmap = new BitmapImage( imageUri );
            Background = new ImageBrush( imageBitmap );

            path = Properties.Settings.Default.PlayerImage;
            if( path != String.Empty )
                imgPhoto.Source = new BitmapImage( new Uri( path ) );
            else
                imgPhoto.Source = new BitmapImage( new Uri( Properties.Resources.PathToPlayerImage, UriKind.Relative ) );
            tbName.Text = Properties.Settings.Default.PlayerName;

            BuildBaseDeck( );
            BuildFreeCards( );
            BuildUpperDecks( );
            BuildLowerDecks( );
        }

        /// <summary>
        /// Очистка стола от карт
        /// </summary>
        private void ClearTable( )
        {
            int deckSize = freeCards.GetDeckSize();
            for( int i = 0; i < deckSize; i++ )
                workSpaceGrid.Children.Remove( freeCards.GetFirstCard( true, 0 ).CardControl );

            deckSize = baseDeck.GetDeckSize( );
            for( int i = 0; i < deckSize; i++ )
                untouchedDeckPanel.Children.Remove( baseDeck.GetFirstCard( true ).CardControl );

            deckSize = givingDeck.GetDeckSize( );
            for( int i = 0; i < deckSize; i++ )
                untouchedDeckPanel.Children.Remove( givingDeck.GetFirstCard( true ).CardControl );

            for( int i = 0; i < upperDecks.Count; i++ )
            {
                deckSize = upperDecks[i].GetDeckSize( );
                for( int j = 0; j < deckSize; j++ )
                    acesDeckPanel.Children.Remove( upperDecks[ i ].GetFirstCard( true ).CardControl );

                deckSize = lowerDecks[i].GetDeckSize( );
                for( int j = 0; j < deckSize; j++ )
                    kingsDeckPanel.Children.Remove( lowerDecks[ i ].GetFirstCard( true ).CardControl );
            }

        }

        /// <summary>
        /// Очистка параметров игры
        /// </summary>
        private void ClearParams( )
        {
            Scores = -150;
            fullDecks = 0;
            countBaseDeck = 0;
            isDoubleClick = false;
            currentTime = new DateTime( );

            transitList = new List<TransitStation>( );
            currentStep = 0;
        }

        private void btnNewGame_Click( object sender, RoutedEventArgs e )
        {
            SetNewGame( );
        }

        /// <summary>
        /// Установить новую игру
        /// </summary>
        private void SetNewGame( )
        {
            ClearTable( );
            ClearParams( );
            InitGame( );
        }

        private void mnuNewGame_Click( object sender, RoutedEventArgs e )
        {
            SetNewGame( );
        }

        private void mnuCancel_Click( object sender, RoutedEventArgs e )
        {
            GoBack( );
        }

        private void btnRestartGame_Click( object sender, RoutedEventArgs e )
        {
            RestartGame( );
        }

        /// <summary>
        /// Восстановить игру сначала
        /// </summary>
        private void RestartGame( )
        {
            ClearTable( );
            ClearParams( );
            baseDeck.SetReserve( );
            BuildFreeCards( );
            BuildUpperDecks( );
            BuildLowerDecks( );
        }

        private void mnuRestart_Click( object sender, RoutedEventArgs e )
        {
            RestartGame( );
        }

        private void btnForward_Click( object sender, RoutedEventArgs e )
        {
            GoNext( );
        }

        /// <summary>
        /// Перейти к следующему ходу
        /// </summary>
        private void GoNext( )
        {
            if( currentStep >= transitList.Count )
                return;
          
            var newStation = transitList[ currentStep ];

            Scores = newStation.Score;
            currentTime = newStation.Time;
            FullDeck = newStation.FullDecks;
            if( newStation.NewDeck is FreeDeck )
                newStation.NewDeck.SetCard( newStation.Card, newStation.NewGridLocation.RowDefinitions.Count * newStation.NewGridRow + newStation.NewGridColumn );
            else
                newStation.NewDeck.SetCard( newStation.Card, newStation.Deck.GetLastAdded( ) );
            if( newStation.Deck != null )
                newStation.Deck.RemoveCard( newStation.Card );
            else
                MessageBox.Show( "test" );
            var oldGrid = newStation.GridLocation;
            var newGrid = newStation.NewGridLocation;

            oldGrid.Children.Remove( newStation.Card.CardControl );

            newGrid.Children.Add( newStation.Card.CardControl );
            Grid.SetColumn( newStation.Card.CardControl, newStation.NewGridColumn );
            Grid.SetRow(newStation.Card.CardControl, newStation.NewGridRow);

            if( newStation.NewDeck is BaseDeck )
            {
                newStation.Card.CardControl.MouseDown -= CardCtrl_MouseDown;
                newStation.Card.CardControl.MouseMove -= CardCtrl_MouseMove;
                newStation.Card.CardControl.MouseUp -= CardCtrl_MouseUp;
                newStation.Card.CardControl.DropCard -= CardCtrl_DropCard;

                newStation.Card.CardControl.MouseUp -= untouchedCard_MouseUp;
                newStation.Card.CardControl.MouseUp += untouchedCard_MouseUp;
            }
            else
            {
                newStation.Card.SetPathToImage(Properties.Resources.PathToCards
                                        + '/'
                                        + newStation.Card.Suit
                                        + '/'
                                        + newStation.Card.Number.ToString()
                                        + ".png");

                newStation.Card.CardControl.MouseUp -= untouchedCard_MouseUp;

                newStation.Card.CardControl.MouseDown -= CardCtrl_MouseDown;
                newStation.Card.CardControl.MouseMove -= CardCtrl_MouseMove;
                newStation.Card.CardControl.MouseUp -= CardCtrl_MouseUp;
                newStation.Card.CardControl.DropCard -= CardCtrl_DropCard;
                newStation.Card.CardControl.MouseDown += CardCtrl_MouseDown;
                newStation.Card.CardControl.MouseMove += CardCtrl_MouseMove;
                newStation.Card.CardControl.MouseUp += CardCtrl_MouseUp;
                newStation.Card.CardControl.DropCard += CardCtrl_DropCard;
            }
            currentStep++;
            
        }

        private void mnuRestore_Click( object sender, RoutedEventArgs e )
        {
            GoNext( );
        }
    }
}
