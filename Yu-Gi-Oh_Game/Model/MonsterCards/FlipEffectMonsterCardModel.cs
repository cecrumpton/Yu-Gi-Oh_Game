using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yu_Gi_Oh_Game.ViewModel;

namespace Yu_Gi_Oh_Game.Model.MonsterCards
{
    public abstract class FlipEffectMonsterCardModel : IMonsterCard
    {
        public CardType YuGiOhCardType => CardType.Monster;
        public MonsterCardType MonsterType => MonsterCardType.Effect;

        public abstract string Name { get; }
        public abstract int Attack { get; }
        public abstract int Defense { get; }
        public abstract bool CanAttack { get; set; }
        public abstract string EffectText { get; set; }
        public MonsterMode Mode { get; set; }
        public abstract void ResolveEffect(DuelistModel duelist, DuelMatViewModel vm, DuelistModel opponent);
    }
}
