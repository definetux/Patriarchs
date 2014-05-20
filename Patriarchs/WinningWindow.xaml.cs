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
    /// Interaction logic for WinningWindow.xaml
    /// </summary>
    public partial class WinningWindow : Window
    {
        public WinningWindow( )
        {
            InitializeComponent( );
        }

        private void btn_showScores_Click( object sender, RoutedEventArgs e )
        {
            this.Close( );
        }
    }
}
