using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using Yu_Gi_Oh_Game.Model.MagicCards;
using Yu_Gi_Oh_Game.Model.MonsterCards;

namespace Yu_Gi_Oh_Game.Model
{
    public class DuelMatModel
    {
        public List<ICard> Cards { get; }
        public List<ICard> OpponentCards { get; }
        public DuelMatModel()
        {

        }
    }
}
