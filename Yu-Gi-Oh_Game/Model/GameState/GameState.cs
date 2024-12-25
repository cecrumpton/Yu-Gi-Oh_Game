using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yu_Gi_Oh_Game.Model.Duelist;
using Yu_Gi_Oh_Game.Model.MonsterCards;

namespace Yu_Gi_Oh_Game.Model.GameState
{
    public class GameState
    {
        public bool IsAttackInProgress { get; private set; }
        public IMonsterCard? AttackingMonster { get; private set; }
        public DuelistModel Player { get; }
        public DuelistModel Opponent { get; }

        public GameState(DuelistModel player, DuelistModel opponent)
        {
            Player = player;
            Opponent = opponent;
        }

        public void StartAttack(IMonsterCard attackingMonster)
        {
            if (!attackingMonster.CanAttack)
                throw new InvalidOperationException("This monster cannot attack.");

            AttackingMonster = attackingMonster;
            IsAttackInProgress = true;
        }

        public void CancelAttack()
        {
            AttackingMonster = null;
            IsAttackInProgress = false;
        }

        public void ExecuteAttack(IMonsterCard? target)
        {
            if (AttackingMonster == null)
                throw new InvalidOperationException("No attack is in progress.");

            if (target != null)
            {
                // Handle target monster destruction or battle logic
            }
            else
            {
                // Direct attack
                Opponent.LifePoints -= AttackingMonster.AttackPoints;
            }

            AttackingMonster.CanAttack = false;
            CancelAttack();
        }
    }

}
