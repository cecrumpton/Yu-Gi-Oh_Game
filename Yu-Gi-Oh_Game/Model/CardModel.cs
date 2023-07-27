using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yu_Gi_Oh_Game.Model
{
    public class CardModel
    {
        public string Name { get; }
        public int Attack { get; }
        public int Defense { get; }
        public CardModel(string name, int attack, int defense)
        {
            Name = name;
            Attack = attack;
            Defense = defense;
        }
    }
}
