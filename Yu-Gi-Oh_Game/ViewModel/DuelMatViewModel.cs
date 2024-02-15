using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Yu_Gi_Oh_Game.Model;
using Yu_Gi_Oh_Game.Other;

namespace Yu_Gi_Oh_Game.ViewModel
{
    public class DuelMatViewModel : INotifyPropertyChanged
    {
        private string _advancePhaseText;
        private bool _isFirstTurn;

        public DuelMatViewModel()
        {
            Player = new DuelistModel(new DeckModel());
            Opponent = new DuelistModel(new OpponentDeckModel());
            Player.PropertyChanged += Player_PropertyChanged;
            Opponent.PropertyChanged += Opponent_PropertyChanged;

            Player.ShuffleDeck();
            Opponent.ShuffleDeck();

            Player.DrawCard(5);
            Opponent.DrawCard(5);

            PlayerLifePoints = OpponentLifePoints = 8000;

            //PlayerDrawPhase = true;
            //AdvancePhaseText = "Draw";

            OpponentDrawPhase = true;
            AdvancePhaseText = "Opponent's Turn";

            _isFirstTurn = true;

            AdvancePhase = new RelayCommand(AdvanceTurnPhase, CheckIfPlayerTurn); //will eventaully check if it is player's turn

            PlayCard = new RelayCommand(PlayerPlayACard, CheckMainPhase);

            Attack = new RelayCommand(AttackOpponent, CheckBattlePhase);
        }

        #region Properties

        public DuelistModel Player { get; }
        public DuelistModel Opponent { get; }
        public ICommand AdvancePhase { get; }
        public ICommand PlayCard { get; }
        public ICommand Attack { get; }

        public int PlayerLifePoints
        {
            get => Player.LifePoints;
            set
            {
                Player.LifePoints = value;
                OnPropertyChanged();
            }
        }

        public int OpponentLifePoints
        {
            get => Opponent.LifePoints;
            set
            {
                Opponent.LifePoints = value;
                OnPropertyChanged();
            }
        }

        public List<ICard> Deck { get => Player.Deck; }
        public List<ICard> OpponentDeck { get => Opponent.Deck; }

        public ObservableCollection<ICard> PlayerHand { get => Player.Hand; }
        public ObservableCollection<ICard> OpponentHand { get => Opponent.Hand; }

        public ObservableCollection<MonsterCardModel> PlayerMonsterCards { get => Player.PlayedMonsterCards; }
        public ObservableCollection<MonsterCardModel> OpponentMonsterCards { get => Opponent.PlayedMonsterCards; }


        public ObservableCollection<ICard> PlayerMagicAndTrapCards { get => Player.PlayedMagicAndTrapCards; }
        public ObservableCollection<ICard> OpponentMagicAndTrapCards { get => Opponent.PlayedMagicAndTrapCards; }

        public bool PlayerTurn
        {
            get => PlayerDrawPhase || PlayerStandbyPhase || PlayerMainPhase1 || 
                PlayerBattlePhase || PlayerMainPhase2 || PlayerEndPhase;
        }

        public bool OpponentTurn
        {
            get => OpponentDrawPhase || OpponentStandbyPhase || OpponentMainPhase1 ||
                OpponentBattlePhase || OpponentMainPhase2 || OpponentEndPhase;
        }

        public bool PlayerDrawPhase
        {
            get => Player.IsDrawPhase;
            set
            {
                Player.IsDrawPhase = value;
                OnPropertyChanged();
            }
        }

        public bool OpponentDrawPhase
        {
            get => Opponent.IsDrawPhase;
            set
            {
                Opponent.IsDrawPhase = value;
                OnPropertyChanged();
            }
        }

        public bool PlayerStandbyPhase
        {
            get => Player.IsStandbyPhase;
            set
            {
                Player.IsStandbyPhase = value;
                OnPropertyChanged();
            }
        }

        public bool OpponentStandbyPhase
        {
            get => Opponent.IsStandbyPhase;
            set
            {
                Opponent.IsStandbyPhase = value;
                OnPropertyChanged();
            }
        }

        public bool PlayerMainPhase1
        {
            get => Player.IsMainPhase1;
            set
            {
                Player.IsMainPhase1 = value;
                OnPropertyChanged();
            }
        }

        public bool OpponentMainPhase1
        {
            get => Opponent.IsMainPhase1;
            set
            {
                Opponent.IsMainPhase1 = value;
                OnPropertyChanged();
            }
        }

        public bool PlayerBattlePhase
        {
            get => Player.IsBattlePhase;
            set
            {
                Player.IsBattlePhase = value;
                OnPropertyChanged();
            }
        }

        public bool OpponentBattlePhase
        {
           get => Opponent.IsBattlePhase;
            set
            {
                Opponent.IsBattlePhase = value;
                OnPropertyChanged();
            }
        }

        public bool PlayerMainPhase2
        {
            get => Player.IsMainPhase2;
            set
            {
                Player.IsMainPhase2 = value;
                OnPropertyChanged();
            }
        }

        public bool OpponentMainPhase2
        {
            get => Opponent.IsMainPhase2;
            set
            {
                Opponent.IsMainPhase2 = value;
                OnPropertyChanged();
            }
        }

        public bool PlayerEndPhase
        {
            get => Player.IsEndPhase;
            set
            {
                Player.IsEndPhase = value;
                OnPropertyChanged();
            }
        }

        public bool OpponentEndPhase
        {
            get => Opponent.IsEndPhase;
            set
            {
                Opponent.IsEndPhase = value;
                OnPropertyChanged();
            }
        }

        public bool PlayerCanNormalSummonMonster
        {
            get => Player.CanNormalSummonMonster;
            set
            {
                Player.CanNormalSummonMonster = value;
                OnPropertyChanged();
            }
        }

        public bool OpponentCanNormalSummonMonster
        {
            get => Opponent.CanNormalSummonMonster;
            set
            {
                Opponent.CanNormalSummonMonster = value;
                OnPropertyChanged();
            }
        }

        public string AdvancePhaseText
        {
            get => _advancePhaseText;
            set
            {
                _advancePhaseText = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region PublicMethods

        #endregion

        #region PrivateMethods

        //will eventaully check if it is player's turn
        private bool CheckIfPlayerTurn(object parameter)
        {
            return true;
        }

        private bool CheckMainPhase(object parameter)
        {
            return PlayerMainPhase1 || PlayerMainPhase2;
        }

        private bool CheckBattlePhase(object parameter)
        {
            return PlayerBattlePhase;
        }

        private async void AdvanceTurnPhase(object parameter)
        {
            //too much going on here, break down in to smaller methods
            if (PlayerDrawPhase)
            {
                ExecuteDrawAndStandbyPhase(Player);
                return;
            }
            if (PlayerMainPhase1)
            {
                if (_isFirstTurn == false)
                    ExecuteBattlePhase(Player);
                else
                    ExecuteEndPhase(Player, Opponent);
                return;
            }
            if (PlayerBattlePhase)
            {
                ExecuteMainPhase2(Player);
                return;
            }
            if (PlayerMainPhase2)
            {
                ExecuteEndPhase(Player, Opponent);
                return;
            }
            if (OpponentDrawPhase)
            {
                ExecuteAIOpponent();
                return;
            }
        }

        private async void ExecuteDrawAndStandbyPhase(DuelistModel duelist)
        {
            if (duelist.CardsLeft < 0) return; //at some point make this to where the player loses the game
            duelist.DrawCard(1);
            duelist.IsDrawPhase = false;
            duelist.IsStandbyPhase = true;
            await Task.Delay(2000);
            duelist.IsStandbyPhase = false;
            duelist.IsMainPhase1 = true;
            if (_isFirstTurn == false)
                AdvancePhaseText = "Start Battle Phase";
            else
                AdvancePhaseText = "End Turn";
            duelist.CanNormalSummonMonster = true;
            return;
        }

        private void ExecuteBattlePhase(DuelistModel duelist)
        {
            duelist.IsMainPhase1 = false;
            duelist.IsBattlePhase = true;
            foreach (var monsterCard in duelist.PlayedMonsterCards)
            {
                monsterCard.CanAttack = true;
            }
            AdvancePhaseText = "Start Main Phase 2";
            return;
        }

        private void ExecuteMainPhase2(DuelistModel duelist)
        {
            duelist.IsBattlePhase = false;
            duelist.IsMainPhase2 = true;
            AdvancePhaseText = "End Turn";
            return;
        }

        private async void ExecuteEndPhase(DuelistModel duelist, DuelistModel opponent)
        {
            if(_isFirstTurn == false)
                duelist.IsMainPhase2 = false;
            else
                duelist.IsMainPhase1 = false;
            duelist.IsEndPhase = true;
            await Task.Delay(2000);
            duelist.IsEndPhase = false;
            if (_isFirstTurn == true)
                _isFirstTurn = false;
            AdvancePhaseText = "Opponent's Turn";
            await Task.Delay(1000);
            opponent.IsDrawPhase = true;

            //here temporarily uitil logic can be used for both human and ai
            ExecuteAIOpponent();
        }

        private async void ExecuteAIOpponent()
        {
            //AI Logic begins here, this will eventually need to be removed
            if (Opponent.CardsLeft < 0) return; //at some point make this to where the opponent loses the game
            await Task.Delay(2000);
            //DrawCards(1, false);
            Opponent.DrawCard(1);
            //DrawCards(Opponent, 1);
            await Task.Delay(2000);
            OpponentDrawPhase = false;
            OpponentStandbyPhase = true;
            await Task.Delay(2000);
            OpponentStandbyPhase = false;
            OpponentMainPhase1 = true;
            await Task.Delay(2000);
            OpponentCanNormalSummonMonster = true;
            var cardToPlay = OpponentHand[Random.Shared.Next(5)];
            OpponentPlayACard(cardToPlay);
            await Task.Delay(2000);
            OpponentMainPhase1 = false;
            if(_isFirstTurn == false)
            {
                OpponentBattlePhase = true;
                foreach (var monsterCard in OpponentMonsterCards)
                {
                    monsterCard.CanAttack = true;
                    AttackPlayer(monsterCard);
                    await Task.Delay(2000);
                }
                await Task.Delay(2000);
                OpponentBattlePhase = false;
                OpponentMainPhase2 = true;
                await Task.Delay(2000);
                OpponentMainPhase2 = false;
            }
            OpponentEndPhase = true;
            await Task.Delay(2000);
            OpponentEndPhase = false;
            if(_isFirstTurn)
                _isFirstTurn = false;
            PlayerDrawPhase = true;
            AdvancePhaseText = "Draw";
            return;
        }

        //when implementing chains I can remove the await and async out of this method.
        //TODO: these to methods can be condensed down in to one, similar to the draw cards method
        //TODO: can I replace object parameter with ICard card?
        private void PlayerPlayACard(object parameter)
        {
            if (parameter is ICard == false) return;
            ICard card = (ICard)parameter;
            Player.PlayACard(card, this, Opponent);
        }

        private void OpponentPlayACard(object parameter)
        {
            if (parameter is ICard == false) return;
            ICard card = (ICard)parameter;
            Opponent.PlayACard(card, this, Player);
        }

        //TODO: these to methods can be condensed down in to one, similar to the draw cards method
        //TODO: should the duielist model handle this logic, or should it be handled in the duel mat view model/duel mat model, or split between the two?
        private void AttackOpponent(object parameter)
        {
            if (parameter is MonsterCardModel == false) return;
            MonsterCardModel card = (MonsterCardModel)parameter; //this cast shouldn't be necessary, should use the property to check if it is a monster
            if (card.CanAttack)
            {
                OpponentLifePoints = OpponentLifePoints - card.Attack;
                card.CanAttack = false;
            }
            //if (parameter is MonsterCardModel == false) return;
            //MonsterCardModel card = (MonsterCardModel)parameter; //this cast shouldn't be necessary, should use the property to check if it is a monster
            //Player.AttackOpponent(card, OpponentLifePoints);
        }

        private void AttackPlayer(object parameter)
        {
            if (parameter is MonsterCardModel == false) return;
            MonsterCardModel card = (MonsterCardModel)parameter; //this cast shouldn't be necessary, should use the property to check if it is a monster
            if (card.CanAttack)
            {
                PlayerLifePoints = PlayerLifePoints - card.Attack;
                card.CanAttack = false;
            }
            //if (parameter is MonsterCardModel == false) return;
            //MonsterCardModel card = (MonsterCardModel)parameter; //this cast shouldn't be necessary, should use the property to check if it is a monster
            //Opponent.AttackOpponent(card, PlayerLifePoints);
        }


        private void Player_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(PlayerDrawPhase));
            OnPropertyChanged(nameof(PlayerStandbyPhase));
            OnPropertyChanged(nameof(PlayerMainPhase1));
            OnPropertyChanged(nameof(PlayerBattlePhase));
            OnPropertyChanged(nameof(PlayerMainPhase2));
            OnPropertyChanged(nameof(PlayerEndPhase));
            OnPropertyChanged(nameof(PlayerLifePoints));
        }
        private void Opponent_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(OpponentDrawPhase));
            OnPropertyChanged(nameof(OpponentLifePoints));
        }

        #endregion

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
