using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yu_Gi_Oh_Game.Model.Deck;

namespace Yu_Gi_Oh_Game.Model.Hand
{
    public interface IHandModel
    {
        public event EventHandler<HandEventArgs> HandUpdated;
        public IEnumerable<ICard> Hand { get; }
        public void AddCard(ICard card);
        public void RemoveCard(ICard card);
        public ICard GetCard(int index);
    }
}
