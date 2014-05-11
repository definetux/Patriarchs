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
    /// Interaction logic for RulesWindow.xaml
    /// </summary>
    public partial class RulesWindow : Window
    {
        public RulesWindow( )
        {
            InitializeComponent( );
        }

        private void Label_Enter(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Help;
            var block = sender as TextBlock;
            block.TextDecorations = TextDecorations.Underline;
            block.Foreground = Brushes.Brown;
        }

        private void Label_Leave(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
            var block = sender as TextBlock;
            block.TextDecorations = null;
            block.Foreground = Brushes.Blue;
        }
    }
}
