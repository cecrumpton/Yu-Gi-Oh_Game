using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Yu_Gi_Oh_Game.Model;
using Yu_Gi_Oh_Game.Model.Duelist;
using Yu_Gi_Oh_Game.Model.Hand;

namespace Yu_Gi_Oh_Game.ViewModel
{
    public class PlayerHandViewModel : BindableBase
    {
        private readonly DuelistModel _player;

        public PlayerHandViewModel(DuelistModel player)
        {
            _player = player;

            PlayerHand = new ObservableCollection<ICard>(_player.HandModel.Hand);

            PlayCard = new DelegateCommand<ICard>(PlayACard);

            _player.HandModel.HandUpdated += Player_HandUpdated;
        }


        public ObservableCollection<ICard> PlayerHand { get; }

        public ICommand PlayCard { get; }


        private void PlayACard(ICard card)
        {
            if (_player.IsMainPhase1 || _player.IsMainPhase2)
            {
                _player.PlayACard(card);
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
    }

}
