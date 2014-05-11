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

namespace Patriarchs
{
    /// <summary>
    /// Interaction logic for DeskImageWindow.xaml
    /// </summary>
    public partial class DeskImageWindow : Window
    {
        public DeskImageWindow()
        {
            InitializeComponent();
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            var image = sender as Image;
            image.Margin = new Thickness(5);
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            var image = sender as Image;
            image.Margin = new Thickness(10);
        }

        private void CardCtrl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var card = sender as Image;
            string[] items = card.Source.ToString().Split('/');
            Properties.Settings.Default.Desk = items.Last();
            this.Close();
        }
    }
}
