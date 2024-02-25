using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yu_Gi_Oh_Game.Model.Duelist;
using Yu_Gi_Oh_Game.ViewModel;

namespace Yu_Gi_Oh_Game.Model.MagicCards
{
    public interface IMagicTrapCard : ICard
    {
        public CardType YuGiOhCardType { get; }
        public string Name { get; }
        public string Description { get; }
        public bool IsContinuous { get; }
        public void ResolveEffect(DuelistModel duelist, DuelMatViewModel vm, DuelistModel opponent);
    }
}
