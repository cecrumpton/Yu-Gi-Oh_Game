using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yu_Gi_Oh_Game.Model.Duelist;

namespace Yu_Gi_Oh_Game.ViewModel
{
    public class OpponentMonstersViewModel : BindableBase
    {
        private readonly DuelistModel _opponent;

        public OpponentMonstersViewModel(DuelistModel opponent)
        {
            _opponent = opponent;
        }
    }
}
