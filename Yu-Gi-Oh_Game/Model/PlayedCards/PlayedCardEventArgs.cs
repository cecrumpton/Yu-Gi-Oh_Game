using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yu_Gi_Oh_Game.Model.PlayedCards
{
    public class PlayedCardEventArgs : EventArgs
    {
        public ICard Card { get; }
        public PlayedCardAction Action { get; }
        public PlayedCardEventArgs(ICard card, PlayedCardAction action)
        {
            Card = card;
            Action = action;
        }
    }
}
