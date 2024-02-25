using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yu_Gi_Oh_Game.Model.Duelist;

namespace Yu_Gi_Oh_Game.Model.Deck
{
    public interface IDeckModel
    {
        public event EventHandler<DeckEventArgs> DeckUpdated;
        public IEnumerable<ICard> Deck { get; }
        public int CardsLeft { get; }
        public void Shuffle();
    }
}
