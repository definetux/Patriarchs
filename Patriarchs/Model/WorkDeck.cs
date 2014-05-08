using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patriarchs.Model
{
    class WorkDeck: IDeck, IWorkDeck
    {
        private string suit;

        private List<Card> listOfCards;

        public Card GetFirstCard( bool isRemove )
        {
            return listOfCards.First( );
        }

        public void SetCard( Card card )
        {
            throw new NotImplementedException( );
        }
    }
}
