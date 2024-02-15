using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yu_Gi_Oh_Game.ViewModel;

namespace Yu_Gi_Oh_Game.Model.MagicCards
{
    public class Ookazi : MagicCardModel
    {
        public override string Name => "Ookazi";

        public override string Description => "Inflict 800 points of direct damage to your opponent's life points.";

        public override bool IsContinuous => false;

        public override void ResolveEffect(DuelistModel duelist, DuelMatViewModel vm, DuelistModel opponent)
        {
            //if (duelist == vm.Player)
            //    vm.OpponentLifePoints -= 800;
            //else
            //    vm.PlayerLifePoints -= 800;
            opponent.LifePoints -= 800;
        }
    }
}
