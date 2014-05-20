using System;
using System.Collections.Generic;
using System.IO;
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

    class Record
    {
        public String Name { get; set; }

        public String Time { get; set; }

        public String Score { get; set; }

        public Record( string name, string time, string score )
        {
            this.Name = name;
            this.Time = time;
            this.Score = score;
        }
    }
    /// <summary>
    /// Interaction logic for ScoresWindow.xaml
    /// </summary>
    public partial class ScoresWindow : Window
    {
        public ScoresWindow( )
        {
            InitializeComponent( );

            List<Record> records = new List<Record>();

            string[ ] lists = File.ReadAllLines( "scores.txt" );

            foreach( var item in lists )
            {
                string[] recordStr = item.Split('|');
                Record record = new Record( recordStr[ 0 ], recordStr[ 1 ], recordStr[ 2 ] );
                records.Add( record );
            }


            gridScores.ItemsSource = records;
        }

        private void Next_Click( object sender, RoutedEventArgs e )
        {
            this.Close( );
        }
    }
}
