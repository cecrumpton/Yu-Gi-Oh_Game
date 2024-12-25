using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Yu_Gi_Oh_Game.Model.Duelist;
using Yu_Gi_Oh_Game.Model.GameState;
using Yu_Gi_Oh_Game.Model.MonsterCards;

namespace Yu_Gi_Oh_Game.ViewModel
{
    public class OpponentMonstersViewModel : BindableBase
    {
        private readonly DuelistModel _opponent;
        private readonly GameState _gameState;

        public OpponentMonstersViewModel(DuelistModel opponent, GameState gameState)
        {
            _opponent = opponent;
            _gameState = gameState;

            AttackTarget = new DelegateCommand<IMonsterCard>(AttackOpponentCard);
        }

        public ICommand AttackTarget { get; }

        private void AttackOpponentCard(IMonsterCard targetCard)
        {
            if (_gameState.IsAttackInProgress)
            {
                _gameState.ExecuteAttack(targetCard);
            }
        }
    }
}
