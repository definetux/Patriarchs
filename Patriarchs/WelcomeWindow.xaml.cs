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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Patriarchs
{
    /// <summary>
    /// Interaction logic for WelcomeWindow.xaml
    /// </summary>
    public partial class WelcomeWindow : Window
    {
        public WelcomeWindow( )
        {
            InitializeComponent( );
            string path = Environment.CurrentDirectory + "\\Sounds\\intro.mp3";
            if( MP3Player.MP3Player.OpenPlayer( path ) == false )
                return;
            if( MP3Player.MP3Player.Play( new WindowInteropHelper(this).Handle ) == false )
                return;
        }

        private void Button_Click_1( object sender, RoutedEventArgs e )
        {
            MainWindow mainWnd = new MainWindow( );
            mainWnd.Show( );
            this.Close( );
        }
    }
}
