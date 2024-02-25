using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Yu_Gi_Oh_Game.Model.Duelist;
using Yu_Gi_Oh_Game.ViewModel;

namespace Yu_Gi_Oh_Game.Model.MagicCards
{
    public class PotOfGreed : IMagicTrapCard //TODO: Have a MagicCard abstract class...?
    {
        public CardType YuGiOhCardType => CardType.Magic;
        public string Name => "Pot of Greed";
        public string Description => "Draw 2 cards.";
        public bool IsContinuous => false;
        public void ResolveEffect(DuelistModel duelist, DuelMatViewModel vm, DuelistModel opponent)
        {
            duelist.DrawCard(2);
        }
    }
}
