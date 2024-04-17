using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yu_Gi_Oh_Game.Model.Duelist;
using Yu_Gi_Oh_Game.ViewModel;

namespace Yu_Gi_Oh_Game.Model.MagicCards
{
    public class Ookazi : IMagicTrapCard //TODO: Have a MagicCard abstract class...?
    {
        public CardType YuGiOhCardType => CardType.Magic;
        public string Name => "Ookazi";
        public string Description => "Inflict 800 points of direct damage to your opponent's life points.";
        public bool IsContinuous => false;
        public void ResolveEffect(DuelistModel duelist, DuelistModel opponent)
        {
            opponent.LifePoints -= 800;
        }
    }
}
