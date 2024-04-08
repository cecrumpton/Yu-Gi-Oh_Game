using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yu_Gi_Oh_Game.Model.Hand;
using Yu_Gi_Oh_Game.Model.MagicCards;
using Yu_Gi_Oh_Game.Model.MonsterCards;

namespace Yu_Gi_Oh_Game.Model.PlayedCards
{
    public interface IPlayedCardsModel
    {
        public event EventHandler<PlayedCardEventArgs> PlayedCardUpdated;
        public IEnumerable<IMonsterCard> PlayedMonsterCards { get; }
        public IEnumerable<IMagicTrapCard> PlayedMagicTrapCards { get; }
        public void AddMonsterCard(IMonsterCard card);
        public void RemoveMonsterCard(IMonsterCard card);
        public IMonsterCard GetMonsterCard(int index);
        public void AddMagicTrapCard(IMagicTrapCard card);
        public void RemoveMagicTrapCard(IMagicTrapCard card);
        public IMagicTrapCard GetMagicTrapCard(int index);
    }
}
