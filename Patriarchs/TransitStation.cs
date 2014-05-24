using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Patriarchs.Model;

namespace Patriarchs
{
    class TransitStation
    {
        public Grid GridLocation
        {
            get;
            set;
        }

        public int GridRow
        {
            get;
            set;
        }

        public int GridColumn
        {
            get;
            set;
        }

        public Card Card
        {
            get;
            set;
        }

        public IDeck Deck
        {
            get;
            set;
        }

        public DateTime Time
        {
            get;
            set;
        }

        public int Score
        {
            get;
            set;
        }

        public IDeck NewDeck
        {
            get;
            set;
        }

        public int FullDecks
        {
            get;
            set;
        }
    }
}
