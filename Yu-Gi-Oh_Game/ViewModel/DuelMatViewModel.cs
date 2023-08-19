using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
        private int _playerLifePoints;
        private int _opponentLifePoints;
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
        private static readonly Random _random = new Random();

        public DuelMatViewModel()
        {
            DuelMatModel model = new DuelMatModel();
            Deck = model.Cards;
            OpponentDeck = model.Cards;

            DeckOrder = new int[Deck.Count];
            OpponentDeckOrder = new int[Deck.Count];

            ShuffleDeck(DeckOrder);
            ShuffleDeck(OpponentDeckOrder);

            CardsLeft = DeckOrder.Length -1;
            OpponentCardsLeft = OpponentDeckOrder.Length -1;

            Hand = new ObservableCollection<ICard>();
            OpponentHand = new ObservableCollection<ICard>();

            DrawCards(5, true);
            DrawCards(5, false);

            PlayerLifePoints = OpponentLifePoints = 8000;

            IsDrawPhase = true;

            PlayedMonsterCards = new ObservableCollection<MonsterCardModel>();
            OpponentMonsterCards = new ObservableCollection<MonsterCardModel>();

            PlayedMagicAndTrapCards = new ObservableCollection<ICard>();
            OpponentMagicAndTrapCards = new ObservableCollection<ICard>();

            AdvancePhase = new RelayCommand(AdvanceTurnPhase, CheckIfPlayerTurn); //will eventaully check if it is player's turn
            
            PlayCard = new RelayCommand(PlayACard, CheckMainPhase);
            
            Attack = new RelayCommand(AttackOpponent, CheckBattlePhase);
            
            AdvancePhaseText = "Draw";
        }

        #region Properties

        public ICommand AdvancePhase { get; }
        public ICommand PlayCard { get; }
        public ICommand Attack { get; }

        public int PlayerLifePoints
        {
            get => _playerLifePoints;
            set
            {
                _playerLifePoints = value;
                OnPropertyChanged();
            }
        }

        public int OpponentLifePoints
        {
            get => _opponentLifePoints;
            set
            {
                _opponentLifePoints = value;
                OnPropertyChanged();
            }
        }

        public List<ICard> Deck { get; }
        public List<ICard> OpponentDeck { get; }

        public ObservableCollection<ICard> Hand { get; }
        public ObservableCollection<ICard> OpponentHand { get; }

        public ObservableCollection<MonsterCardModel> PlayedMonsterCards { get; }
        public ObservableCollection<MonsterCardModel> OpponentMonsterCards { get; }


        public ObservableCollection<ICard> PlayedMagicAndTrapCards { get; }
        public ObservableCollection<ICard> OpponentMagicAndTrapCards { get; }


        public bool IsDrawPhase
        {
            get => _isDrawPhase;
            set
            {
                _isDrawPhase = value;
                OnPropertyChanged();
            }
        }

        public bool OpponentDrawPhase
        {
            get => _opponentDrawPhase;
            set
            {
                _opponentDrawPhase = value;
                OnPropertyChanged();
            }
        }

        public bool IsStandbyPhase
        {
            get => _isStandbyPhase;
            set
            {
                _isStandbyPhase = value;
                OnPropertyChanged();
            }
        }

        public bool OpponentStandbyPhase
        {
            get => _opponentStandbyPhase;
            set
            {
                _opponentStandbyPhase = value;
                OnPropertyChanged();
            }
        }

        public bool IsMainPhase1
        {
            get => _isMainPhase1;
            set
            {
                _isMainPhase1 = value;
                OnPropertyChanged();
            }
        }

        public bool OpponentMainPhase1
        {
            get => _opponentMainPhase1;
            set
            {
                _opponentMainPhase1 = value;
                OnPropertyChanged();
            }
        }

        public bool IsBattlePhase
        {
            get => _isBattlePhase;
            set
            {
                _isBattlePhase = value;
                OnPropertyChanged();
            }
        }

        public bool OpponentBattlePhase
        {
           get => _opponentBattlePhase;
            set
            {
                _opponentBattlePhase = value;
                OnPropertyChanged();
            }
        }

        public bool IsMainPhase2
        {
            get => _isMainPhase2;
            set
            {
                _isMainPhase2 = value;
                OnPropertyChanged();
            }
        }

        public bool OpponentMainPhase2
        {
            get => _opponentMainPhase2;
            set
            {
                _opponentMainPhase2 = value;
                OnPropertyChanged();
            }
        }

        public bool IsEndPhase
        {
            get => _isEndPhase;
            set
            {
                _isEndPhase = value;
                OnPropertyChanged();
            }
        }

        public bool OpponentEndPhase
        {
            get => _opponentEndPhase;
            set
            {
                _opponentEndPhase = value;
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

        public bool CanNormalSummonMonster
        {
            get => _canNormalSummonMonster;
            set
            {
                _canNormalSummonMonster = value;
                OnPropertyChanged();
            }
        }

        public bool CanOpponentNormalSummonMonster
        {
            get => _canNormalSummonMonster;
            set
            {
                _canNormalSummonMonster = value;
                OnPropertyChanged();
            }
        }

        public int[] DeckOrder { get; }
        public int[] OpponentDeckOrder { get; }

        public int CardsLeft { get; set; }
        public int OpponentCardsLeft { get; set; }

        #endregion

        #region PublicMethods
        //maybe this can be made private since it generally shouldn't be accessible other than for pot of greed?
        //It may actually be best that pot of greed can't access this method, and instead the draw card method should be placed somewhere in a common action method?
        //TODO: fix this so thatthe boolean isn't necessary. There needs to be a PlayerModel class for User and Opponent and that'll reduce a ton of repeated code.
        public void DrawCards(int numberOfCards, bool isPlayer)
        {
            if (isPlayer)
            {
                for (int i = 0; i < numberOfCards; i++)
                {
                    if (CardsLeft < 0) return;
                    Hand.Add(Deck[DeckOrder[CardsLeft]]);
                    CardsLeft--;
                }
            }
            else
            {
                for (int i = 0; i < numberOfCards; i++)
                {
                    if (OpponentCardsLeft < 0) return;
                    OpponentHand.Add(OpponentDeck[OpponentDeckOrder[OpponentCardsLeft]]);
                    OpponentCardsLeft--;
                }
            }
        }
        #endregion

        #region PrivateMethods

        private void ShuffleDeck(int[] deck)
        {
            for (int i = 0; i < Deck.Count; i++)
            {
                deck[i] = i;
            }

            for (int n = deck.Length - 1; n > 0; --n)
            {
                int k = _random.Next(n + 1);
                int temp = deck[n];
                deck[n] = deck[k];
                deck[k] = temp;
            }
        }

        private bool CheckIfPlayerTurn(object parameter)
        {
            return true;
        }

        private bool CheckMainPhase(object parameter)
        {
            return IsMainPhase1 || IsMainPhase2;
        }

        private bool CheckBattlePhase(object parameter)
        {
            return IsBattlePhase;
        }

        private async void AdvanceTurnPhase(object parameter)
        {
            //too much going on here, break down in to smaller methods
            if (IsDrawPhase)
            {
                if (CardsLeft < 0) return; //at some point make this to where the player loses the game
                DrawCards(1, true);
                IsDrawPhase = false;
                IsStandbyPhase = true;
                await Task.Delay(2000);
                IsStandbyPhase = false;
                IsMainPhase1 = true;
                AdvancePhaseText = "Start Battle Phase";
                CanNormalSummonMonster = true;
                return;
            }
            if (IsMainPhase1)
            {
                IsMainPhase1 = false;
                IsBattlePhase = true;
                foreach(var monsterCard in PlayedMonsterCards) 
                {
                    monsterCard.CanAttack = true;
                }
                AdvancePhaseText = "Start Main Phase 2";
                return;
            }
            if (IsBattlePhase)
            {
                IsBattlePhase = false;
                IsMainPhase2 = true;
                AdvancePhaseText = "End Turn";
                return;
            }
            if (IsMainPhase2)
            {
                IsMainPhase2 = false;
                IsEndPhase = true;
                AdvancePhaseText = "Opponent's Turn";

                //Start of logic for opponent's turn
                await Task.Delay(2000);
                IsEndPhase = false;
                await Task.Delay(1000);
                OpponentDrawPhase = true;

                //AI Logic begins here, this will eventually need to be removed
                if (OpponentCardsLeft < 0) return; //at some point make this to where the opponent loses the game
                await Task.Delay(2000);
                DrawCards(1, false);
                await Task.Delay(2000);
                OpponentDrawPhase = false;
                OpponentStandbyPhase = true;
                await Task.Delay(2000);
                OpponentStandbyPhase = false;
                OpponentMainPhase1 = true;
                await Task.Delay(2000);
                CanOpponentNormalSummonMonster = true;
                Random random = new Random();
                var cardToPlay = OpponentHand[random.Next(5)];
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
                IsDrawPhase = true;
                AdvancePhaseText = "Draw";
                return;
            }
            if (IsEndPhase)
            {

            }
        }

        //when implementing chains I can remove the await and async out of this method.
        //TODO: these to methods can be condensed down in to one, similar to the draw cards method
        private async void PlayACard(object parameter)
        {
            if (parameter is ICard == false) return;
            ICard card = (ICard)parameter;
            if (card.YuGiOhCardType == CardType.Monster && PlayedMonsterCards.Count < 5)
            {
                if (CanNormalSummonMonster == false) return;
                PlayedMonsterCards.Add((MonsterCardModel)card);
                Hand.Remove(card);
                CanNormalSummonMonster = false;
            }
            if(card.YuGiOhCardType == CardType.Magic)
            {
                MagicCardModel magicCard = (MagicCardModel) card;
                Hand.Remove(magicCard);
                PlayedMagicAndTrapCards.Add(magicCard);
                await Task.Delay(2000);
                magicCard.ResolveEffect(this);
                if(magicCard.IsContinuous == false)
                    PlayedMagicAndTrapCards.Remove(magicCard);
            }
        }

        private void OpponentPlayACard(object parameter)
        {
            if (parameter is ICard == false) return;
            ICard card = (ICard)parameter;
            if (card.YuGiOhCardType == CardType.Monster && OpponentMonsterCards.Count < 5)
            {
                if (CanOpponentNormalSummonMonster == false) return;
                OpponentMonsterCards.Add((MonsterCardModel)card);
                OpponentHand.Remove(card);
                CanOpponentNormalSummonMonster = false;
            }
        }

        //TODO: these to methods can be condensed down in to one, similar to the draw cards method
        private void AttackOpponent(object parameter)
        {
            if (parameter is MonsterCardModel == false) return;
            MonsterCardModel card = (MonsterCardModel)parameter; //this cast shouldn't be necessary, should use the property to check if it is a monster
            if (card.CanAttack)
            {
                OpponentLifePoints = OpponentLifePoints - card.Attack;
                card.CanAttack = false;
            }
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
