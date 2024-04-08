using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yu_Gi_Oh_Game.Model.MonsterCards
{
    public interface IMonsterCard : ICard
    {
        public MonsterCardType MonsterType { get; }
        public int Attack { get; }
        public int Defense { get; }
        public bool CanAttack { get; set; }
        public MonsterMode Mode { get; set; }
    }
}
