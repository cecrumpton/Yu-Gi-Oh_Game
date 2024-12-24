using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Yu_Gi_Oh_Game.Mediators;
using Yu_Gi_Oh_Game.Model;
using Yu_Gi_Oh_Game.Model.Duelist;

namespace Yu_Gi_Oh_Game.ViewModel
{
    public class PlayerHandViewModel
    {
        private readonly IMediator _mediator;

        public PlayerHandViewModel(IMediator mediator)
        {
            _mediator = mediator;

            PlayCard = new DelegateCommand<ICard>(PlayACard);
        }

        public ObservableCollection<ICard> PlayerHand { get; set; } = new ObservableCollection<ICard>();

        public ICommand PlayCard { get; }

        private void PlayACard(ICard card)
        {
            //if (PlayerMainPhase1 || PlayerMainPhase2)
            //{
            //    Player.PlayACard(card, Opponent);
            //}
        }
        //public PlayerHandViewModel(IMediator mediator)
        //{
        //    _mediator = mediator;

        //    PlayCard = new RelayCommand<ICard>(card =>
        //    {
        //        PlayerHand.Remove(card);
        //        _mediator.Notify(this, "CardPlayed", card);
        //    });
        //}
    }

}
