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
    /// Interaction logic for ShirtsWindow.xaml
    /// </summary>
    public partial class ShirtsWindow : Window
    {
        public ShirtsWindow()
        {
            InitializeComponent();
        }

        private void CardCtrl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var card = sender as CardLib.CardCtrl;
            string[] items = card.ImgSource.ToString().Split('/');
            Properties.Settings.Default.Shirt = items.Last();
            this.Close();
        }
    }
}
