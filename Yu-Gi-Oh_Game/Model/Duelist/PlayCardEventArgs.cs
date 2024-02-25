using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yu_Gi_Oh_Game.Model.Duelist
{
    public class PlayCardEventArgs : EventArgs
    {
        public ICard Card { get; }
        public PlayCardEventArgs(ICard card) 
        {
            Card = card;
        }
    }
}
