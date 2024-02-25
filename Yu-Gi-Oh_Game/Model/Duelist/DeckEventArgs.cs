using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yu_Gi_Oh_Game.Model.Duelist
{
    public class DeckEventArgs
    {
        public IEnumerable<ICard> Deck { get; }
        public DeckAction Action { get; }

        public DeckEventArgs(IEnumerable<ICard> deck, DeckAction action)
        {
            Deck = deck;
            Action = action;
        }
    }
}
