using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yu_Gi_Oh_Game.Model.MonsterCards
{
    public class NormalMonsterCardModel : IMonsterCard
    {
        public CardType YuGiOhCardType => CardType.Monster;
        public string Name { get; }
        public int Attack { get; }
        public int Defense { get; }
        public bool CanAttack { get; set; }
        public MonsterMode Mode { get; set; }

        public NormalMonsterCardModel(string name, int attack, int defense)
        {
            Name = name;
            Attack = attack;
            Defense = defense;
            CanAttack = true;
        }
    }
}
