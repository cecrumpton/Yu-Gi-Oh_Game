using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yu_Gi_Oh_Game.ViewModel;

namespace Yu_Gi_Oh_Game.Model
{
    public abstract class MagicCardModel : ICard
    {
        public CardType YuGiOhCardType => CardType.Magic;
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract bool IsContinuous { get; }
        public abstract void ResolveEffect(DuelMatViewModel vm);
    }
}
