using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yu_Gi_Oh_Game.Model.Deck
{
    public class DeckEventArgs
    {
        public DeckAction Action { get; }

        public DeckEventArgs(DeckAction action)
        {
            Action = action;
        }
    }
}
