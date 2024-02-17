using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Yu_Gi_Oh_Game.View;
using Yu_Gi_Oh_Game.ViewModel;

namespace Yu_Gi_Oh_Game.Model.MonsterCards.EffectMonsters
{
    public class Dark_Elf : EffectMonsterCardModel
    {
        private DuelistModel _duelist;
        private DuelMatViewModel _vm;
        private DuelistModel _opponent;
        private IMonsterCard _attackingMonsterCard;

        public override string Name => "Dark Elf";//any way I can do nameof(Dark_Elf).RemoveUnderscoresFromString() and make it universal so I don't have to re-create the name for every effect monster?;
        public override int Attack => 2000;
        public override int Defense => 800;
        public override bool CanAttack { get; set; }

        public override string EffectText => "This card cost 1000 of your life points to attack.";

        public Dark_Elf()
        {
            CanAttack = true;
        }

        public override void InitializeCardEffect(DuelistModel duelist, DuelMatViewModel vm, DuelistModel opponent)
        {
            _duelist = duelist;
            _vm = vm;
            _opponent = opponent;
            _vm.PropertyChanged += _vm_PropertyChanged;            
        }

        private void _vm_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(_vm.IsAttackDeclared))
            {
                ResolveEffect();
            }
        }

        public override void ResolveEffect() //is it possible a threading issue can happen here? This needs to be sorted out when I implement trap cards
        {
            if (_vm.AttackingMonsterCard == this)
            {
                _duelist.LifePoints -= 1000;
            }
        }
    }
}
