using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yu_Gi_Oh_Game.Model
{
    public class CardModel : ICard
    {
        public CardType YuGiOhCardType { get; }
        public string Name { get; }
        public int Attack { get; }
        public int Defense { get; }
        public bool CanAttack { get; set; }
        public CardModel(CardType type, string name, int attack, int defense)
        {
            YuGiOhCardType = type;
            Name = name;
            Attack = attack;
            Defense = defense;
            CanAttack = true;
        }
    }
}
