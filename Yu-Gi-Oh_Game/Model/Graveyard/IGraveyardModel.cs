using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yu_Gi_Oh_Game.Model.Hand;

namespace Yu_Gi_Oh_Game.Model.Graveyard
{
    public interface IGraveyardModel
    {
        public event EventHandler<GraveyardEventArgs> GraveyardUpdated;
        public IEnumerable<ICard> Graveyard { get; }
        public void AddCard(ICard card);
        public void RemoveCard(ICard card);
        public ICard GetCard(int index);
    }
}
