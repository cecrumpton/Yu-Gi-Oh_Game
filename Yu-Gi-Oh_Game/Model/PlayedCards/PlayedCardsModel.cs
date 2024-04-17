using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yu_Gi_Oh_Game.Model.Graveyard;
using Yu_Gi_Oh_Game.Model.Hand;
using Yu_Gi_Oh_Game.Model.MagicCards;
using Yu_Gi_Oh_Game.Model.MonsterCards;

namespace Yu_Gi_Oh_Game.Model.PlayedCards
{
    public class PlayedCardsModel : IPlayedCardsModel
    {
        private readonly List<IMonsterCard> _playedMonsterCards;
        private readonly List<IMagicTrapCard> _playedMagicTrapCards;

        public PlayedCardsModel()
        {
            _playedMonsterCards = new List<IMonsterCard>();
            _playedMagicTrapCards = new List<IMagicTrapCard>();
        }

        public event EventHandler<PlayedCardEventArgs> PlayedCardUpdated;

        public IEnumerable<IMonsterCard> PlayedMonsterCards { get => _playedMonsterCards; }
        public IEnumerable<IMagicTrapCard> PlayedMagicTrapCards { get => _playedMagicTrapCards; }

        public void AddMonsterCard(IMonsterCard card)
        {
            _playedMonsterCards.Add(card);
            OnPlayedCardUpdated(card, PlayedCardAction.AddMonster);
        }

        public void RemoveMonsterCard(IMonsterCard card)
        {
            _playedMonsterCards.Remove(card);
            OnPlayedCardUpdated(card, PlayedCardAction.RemoveMonster);
        }

        public IMonsterCard GetMonsterCard(int index)
        {
            return _playedMonsterCards[index];
        }

        public void AddMagicTrapCard(IMagicTrapCard card)
        {
            _playedMagicTrapCards.Add(card);
            OnPlayedCardUpdated(card, PlayedCardAction.AddMagicTrap);
        }

        public void RemoveMagicTrapCard(IMagicTrapCard card)
        {
            _playedMagicTrapCards.Remove(card);
            OnPlayedCardUpdated(card, PlayedCardAction.RemoveMagicTrap);
        }

        public IMagicTrapCard GetMagicTrapCard(int index)
        {
            return _playedMagicTrapCards[index];
        }

        private void OnPlayedCardUpdated(ICard card, PlayedCardAction action)
        {
            PlayedCardUpdated?.Invoke(this, new PlayedCardEventArgs(card, action));
        }
    }
}
