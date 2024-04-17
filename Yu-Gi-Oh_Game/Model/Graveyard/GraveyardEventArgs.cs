using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yu_Gi_Oh_Game.Model.Hand;

namespace Yu_Gi_Oh_Game.Model.Graveyard
{
    public class GraveyardEventArgs : EventArgs
    {
        public ICard Card { get; }
        public GraveyardAction Action { get; }

        public GraveyardEventArgs(ICard card, GraveyardAction action)
        {
            Card = card;
            Action = action;
        }
    }
}
