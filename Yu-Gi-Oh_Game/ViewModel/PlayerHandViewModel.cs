using Prism.Commands;
using Prism.Mvvm;
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
using Yu_Gi_Oh_Game.Model.Hand;

namespace Yu_Gi_Oh_Game.ViewModel
{
    public class PlayerHandViewModel : BindableBase
    {
        private readonly IMediator _mediator;

        public PlayerHandViewModel(IMediator mediator, DuelistModel player)
        {
            _mediator = mediator;

            Player = player;

            PlayerHand = new ObservableCollection<ICard>(Player.HandModel.Hand);

            PlayCard = new DelegateCommand<ICard>(PlayACard);

            Player.HandModel.HandUpdated += Player_HandUpdated;
        }

        public DuelistModel Player { get; }

        public ObservableCollection<ICard> PlayerHand { get; }

        public ICommand PlayCard { get; }

        private void PlayACard(ICard card)
        {
            if (Player.IsMainPhase1 || Player.IsMainPhase2)
            {
                Player.PlayACard(card);
            }
        }

        private void Player_HandUpdated(object? sender, HandEventArgs e)
        {
            if (e.Action == HandAction.Add)
            {
                PlayerHand.Add(e.Card);
            }
            else
            {
                PlayerHand.Remove(e.Card); //todo: this won't tecnically remove the card, just the first instance of it
            }
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
