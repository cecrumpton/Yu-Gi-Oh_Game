using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yu_Gi_Oh_Game.Model.Hand;

namespace Yu_Gi_Oh_Game.Model.Graveyard
{
    public class GraveyardModel : IGraveyardModel
    {
        private readonly List<ICard> _graveyard;

        public GraveyardModel()
        {
            _graveyard = new List<ICard>();
        }

        public event EventHandler<GraveyardEventArgs> GraveyardUpdated;
        public IEnumerable<ICard> Graveyard { get => _graveyard; }

        public void AddCard(ICard card)
        {
            _graveyard.Add(card);
            OnGraveyardUpdated(card, GraveyardAction.Add);
        }

        public void RemoveCard(ICard card)
        {
            _graveyard.Remove(card);
            OnGraveyardUpdated(card, GraveyardAction.Remove);
        }

        public ICard GetCard(int index)
        {
            return _graveyard[index];
        }

        private void OnGraveyardUpdated(ICard card, GraveyardAction action)
        {
            GraveyardUpdated?.Invoke(this, new GraveyardEventArgs(card, action));
        }

    }
}
