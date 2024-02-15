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
using Yu_Gi_Oh_Game.Model;
using Yu_Gi_Oh_Game.Other;

namespace Yu_Gi_Oh_Game.ViewModel
{
    public class DuelMatViewModel : INotifyPropertyChanged
    {
        private bool _isDrawPhase;
        private bool _opponentDrawPhase;
        private bool _isStandbyPhase;
        private bool _opponentStandbyPhase;
        private bool _isMainPhase1;
        private bool _opponentMainPhase1;
        private bool _isBattlePhase;
        private bool _opponentBattlePhase;
        private bool _isMainPhase2;
        private bool _opponentMainPhase2;
        private bool _isEndPhase;
        private bool _opponentEndPhase;
        private string _advancePhaseText;
        private bool _canNormalSummonMonster;
        private bool _canOpponentNormalSummonMonster;

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
            //Deck = ShuffleDeck2(Player.Deck);
            //OpponentDeck = ShuffleDeck2(Opponent.Deck);
            //DrawCards(Player, 5);
            //DrawCards(Opponent, 5);

            PlayerLifePoints = OpponentLifePoints = 8000;

            PlayerDrawPhase = true;

            //PlayerMonsterCards = new ObservableCollection<MonsterCardModel>();
            //OpponentMonsterCards = new ObservableCollection<MonsterCardModel>();

            //PlayerMagicAndTrapCards = new ObservableCollection<ICard>();
            //OpponentMagicAndTrapCards = new ObservableCollection<ICard>();

            AdvancePhase = new RelayCommand(AdvanceTurnPhase, CheckIfPlayerTurn); //will eventaully check if it is player's turn

            PlayCard = new RelayCommand(PlayerPlayACard, CheckMainPhase);

            Attack = new RelayCommand(AttackOpponent, CheckBattlePhase);

            AdvancePhaseText = "Draw";
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

        //public int[] DeckOrder { get; }
        //public int[] OpponentDeckOrder { get; }

        public int PlayerCardsLeft
        {
            get => Player.CardsLeft;
            set
            {
                Player.CardsLeft = value;
                OnPropertyChanged();
            }
        }

        public int OpponentCardsLeft
        {
            get => Opponent.CardsLeft;
            set
            {
                Opponent.CardsLeft = value;
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
                if (PlayerCardsLeft < 0) return; //at some point make this to where the player loses the game
                //DrawCards(1, true);
                //DrawCards(Player, 1);
                Player.DrawCard(1);
                PlayerDrawPhase = false;
                PlayerStandbyPhase = true;
                await Task.Delay(2000);
                PlayerStandbyPhase = false;
                PlayerMainPhase1 = true;
                AdvancePhaseText = "Start Battle Phase";
                PlayerCanNormalSummonMonster = true;
                return;
            }
            if (PlayerMainPhase1)
            {
                PlayerMainPhase1 = false;
                PlayerBattlePhase = true;
                foreach(var monsterCard in PlayerMonsterCards) 
                {
                    monsterCard.CanAttack = true;
                }
                AdvancePhaseText = "Start Main Phase 2";
                return;
            }
            if (PlayerBattlePhase)
            {
                PlayerBattlePhase = false;
                PlayerMainPhase2 = true;
                AdvancePhaseText = "End Turn";
                return;
            }
            if (PlayerMainPhase2)
            {
                PlayerMainPhase2 = false;
                PlayerEndPhase = true;
                AdvancePhaseText = "Opponent's Turn";

                //Start of logic for opponent's turn
                await Task.Delay(2000);
                PlayerEndPhase = false;
                await Task.Delay(1000);
                OpponentDrawPhase = true;

                //AI Logic begins here, this will eventually need to be removed
                if (OpponentCardsLeft < 0) return; //at some point make this to where the opponent loses the game
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
                OpponentEndPhase = true;
                await Task.Delay(2000);
                OpponentEndPhase = false;
                PlayerDrawPhase = true;
                AdvancePhaseText = "Draw";
                return;
            }
            if (PlayerEndPhase)
            {

            }
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
            if (e.PropertyName == nameof(Player.LifePoints))
                OnPropertyChanged(nameof(PlayerLifePoints));
        }
        private void Opponent_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Opponent.LifePoints))
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
