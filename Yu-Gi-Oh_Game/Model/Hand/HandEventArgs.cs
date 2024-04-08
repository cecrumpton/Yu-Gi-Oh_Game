using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yu_Gi_Oh_Game.Model.Hand
{
    public class HandEventArgs : EventArgs
    {
        public ICard Card { get; }
        public HandAction Action { get; }

        public HandEventArgs(ICard card, HandAction action)
        {
            Card = card;
            Action = action;
        }
    }
}
