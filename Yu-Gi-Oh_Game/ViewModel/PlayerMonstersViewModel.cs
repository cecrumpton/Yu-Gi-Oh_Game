using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Yu_Gi_Oh_Game.Model;
using Yu_Gi_Oh_Game.Model.Duelist;
using Yu_Gi_Oh_Game.Model.GameState;
using Yu_Gi_Oh_Game.Model.MagicCards;
using Yu_Gi_Oh_Game.Model.MonsterCards;
using Yu_Gi_Oh_Game.Model.PlayedCards;

namespace Yu_Gi_Oh_Game.ViewModel
{
    public class PlayerMonstersViewModel : BindableBase
    {
        private readonly DuelistModel _player;
        private readonly GameState _gameState;

        public PlayerMonstersViewModel(DuelistModel player, GameState gameState)
        {
            _player = player;
            _gameState = gameState;
            
            PlayerMonsterCards = new ObservableCollection<IMonsterCard>(_player.PlayedCardsModel.PlayedMonsterCards);

            _player.PropertyChanged += Player_PropertyChanged;
            _player.PlayedCardsModel.PlayedCardUpdated += Player_PlayCardUpdated;

            SelectAttacker = new DelegateCommand<IMonsterCard>(SelectAttackingMonster);
        }

        public ObservableCollection<IMonsterCard> PlayerMonsterCards { get; }
        public ICommand SelectAttacker { get; }

        //TODO: everything should be handled by the event method below
        private void Player_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(PlayerMonsterCards));
        }

        private void Player_PlayCardUpdated(object? sender, PlayedCardEventArgs e)
        {
            if (e.Card.YuGiOhCardType == CardType.Monster)
            {
                if (e.Card is not IMonsterCard card) return; //what's the point of this? It looks like an unnecessary double check
                if (e.Action == PlayedCardAction.AddMonster)
                    PlayerMonsterCards.Add(card);
                if (e.Action == PlayedCardAction.RemoveMonster)
                    PlayerMonsterCards.Remove(card);
            }
        }

        private void SelectAttackingMonster(IMonsterCard card)
        {
            if (_player.IsBattlePhase && card.CanAttack)
            {
                _gameState.StartAttack(card);
            }

            if (_gameState.IsAttackInProgress && _gameState.AttackingMonster == card)
            {
                _gameState.CancelAttack();
            }
        }
    }
}
