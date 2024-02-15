using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Yu_Gi_Oh_Game.ViewModel;

namespace Yu_Gi_Oh_Game.Model.MagicCards
{
    public class PotOfGreed : MagicCardModel
    {
        public override string Name => "Pot of Greed";
        public override string Description => "Draw 2 cards.";
        public override bool IsContinuous => false;
        public override void ResolveEffect(DuelistModel duelist, DuelMatViewModel vm, DuelistModel opponent)
        {
            duelist.DrawCard(2);
        }
    }
}
