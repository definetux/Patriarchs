using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patriarchs.Model
{
    interface IDeck
    {
        Card GetFirstCard( bool isRemove, int number );

        int GetLastAdded( );

        int GetDeckSize( );

        void RemoveCard( Card card );

        void SetCard( Card card, int number );

    }
}
