using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Yu_Gi_Oh_Game.ViewModel;

namespace Yu_Gi_Oh_Game.Model.MonsterCards.EffectMonsters
{
    public class Dark_Elf : EffectMonsterCardModel, INotifyPropertyChanged
    {
        private bool _canAttack;

        public override string Name => "Dark Elf";//any way I can do nameof(Dark_Elf).RemoveUnderscoresFromString() and make it universal so I don't have to re-create the name for every effect monster?;
        public override int Attack => 2000;
        public override int Defense => 800;
        public override bool CanAttack
        {
            get
            {
                return _canAttack;
            }
            set
            {
                _canAttack = value;
                OnPropertyChanged();
            }
        }

        public override string EffectText => "This card cost 1000 of your life points to attack.";

        public override void ResolveEffect(DuelistModel duelist, DuelMatViewModel vm, DuelistModel opponent)
        {
            if (CanAttack == false)
            {
                duelist.LifePoints -= 1000;
            }
        }

        public Dark_Elf()
        {
            CanAttack = true;
            this.PropertyChanged += this_propertyChanged;
        }

        private void this_propertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        #region implement INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        //this may need to have a method that updates all properties in case I need to call it.
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
