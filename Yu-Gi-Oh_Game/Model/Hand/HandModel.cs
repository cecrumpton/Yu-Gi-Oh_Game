using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Yu_Gi_Oh_Game.Model.Deck;

namespace Yu_Gi_Oh_Game.Model.Hand
{
    public class HandModel : IHandModel
    {
        private readonly List<ICard> _hand;

        public HandModel()
        {
            _hand = new List<ICard>();
        }

        public event EventHandler<HandEventArgs> HandUpdated;

        public IEnumerable<ICard> Hand { get => _hand; }

        public void AddCard(ICard card)
        {
            _hand.Add(card);
            OnHandUpdated(card, HandAction.Add);
        }

        public void RemoveCard(ICard card)
        {
            _hand.Remove(card);
            OnHandUpdated(card, HandAction.Remove);
        }

        public ICard GetCard(int index)
        {
            return _hand[index];
        }

        private void OnHandUpdated(ICard card, HandAction action)
        {
            HandUpdated?.Invoke(this, new HandEventArgs(card, action));
        }
    }
}
