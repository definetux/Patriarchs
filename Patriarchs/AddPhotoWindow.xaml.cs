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
using System.Windows.Shapes;
using Microsoft.Win32;

namespace Patriarchs
{
    /// <summary>
    /// Interaction logic for AddPhotoWindow.xaml
    /// </summary>
    public partial class AddPhotoWindow : Window
    {
        public AddPhotoWindow( )
        {
            InitializeComponent( );
        }

        private void rbtnCurrentImage_Checked( object sender, RoutedEventArgs e )
        {
            tbLoadImage.IsEnabled = false;
            btnLoad.IsEnabled = false;
        }

        private void rbtnLoadImage_Checked( object sender, RoutedEventArgs e )
        {
            tbLoadImage.IsEnabled = true;
            btnLoad.IsEnabled = true;
        }

        private void btnLoad_Click( object sender, RoutedEventArgs e )
        {
            OpenFileDialog dialog = new OpenFileDialog( );
            dialog.Filter = "JPEG files (*.jpg)|*.jpg|BMP files (*.bmp)|*.bmp";
            dialog.InitialDirectory = @"C:\";
            dialog.Title = "Выберите изображение.";
            if( dialog.ShowDialog( ) == true )
            {
                imgPhoto.Source = new BitmapImage( new Uri( dialog.FileName ) );
                tbLoadImage.Text = dialog.FileName;
            }             
        }

        private void Next_Click( object sender, RoutedEventArgs e )
        {
            if( rbtnLoadImage.IsChecked == true )
                Properties.Settings.Default.PlayerImage = tbLoadImage.Text;

            if( tbName.Text != String.Empty )
                Properties.Settings.Default.PlayerName = tbName.Text;

            this.Close( );
        }
    }
}
