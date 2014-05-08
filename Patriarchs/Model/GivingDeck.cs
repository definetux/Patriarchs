using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patriarchs.Model
{
    class GivingDeck: IWorkDeck, IDeck
    {
        private List<Card> listOfCards;

        public void SetCard( Card card )
        {
            listOfCards.Add( card );
        }

        public Card GetFirstCard( bool isRemove )
        {
            return listOfCards.First();
        }
    }
}
