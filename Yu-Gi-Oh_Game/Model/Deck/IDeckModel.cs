using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Yu_Gi_Oh_Game.Model.Deck
{
    public interface IDeckModel
    {
        public event EventHandler<DeckEventArgs> DeckUpdated;
        public IEnumerable<ICard> Deck { get; }
        public int CardsLeft { get; }
        public void Shuffle();
        public void AddCards(IEnumerable<ICard> cards);
        public void RemoveCard(int index);
        public ICard GetCard(int index);
    }
}
