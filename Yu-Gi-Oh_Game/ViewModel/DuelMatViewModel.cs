using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
        private bool _isStandbyPhase;
        private bool _isMainPhase1;
        private bool _isBattlePhase;
        private bool _isMainPhase2;
        private bool _isEndPhase;
        private string _advancePhaseText;
        private bool _canNormalSummonMonster;
        private readonly int[] _deckOrder;
        private static readonly Random _random = new Random();
        private int _cardsLeft;
        ObservableCollection<ICard> _hand = new ObservableCollection<ICard>();
        ObservableCollection<ICard> _playedCards = new ObservableCollection<ICard>();

        public DuelMatViewModel()
        {
            DuelMatModel model = new DuelMatModel();
            Deck = model.Cards;
            _deckOrder = new int[Deck.Count];
            ShuffleDeck(_deckOrder);
            Hand = new ObservableCollection<ICard>
            {
                Deck[_deckOrder[0]],
                Deck[_deckOrder[1]],
                Deck[_deckOrder[2]],
                Deck[_deckOrder[3]],
                Deck[_deckOrder[4]],
            };
            _cardsLeft = _deckOrder.Length - Hand.Count;
            PlayerLifePoints = OpponentLifePoints = 8000;
            IsDrawPhase = true;
            PlayedCards = new ObservableCollection<ICard>();
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

        public List<CardModel> Deck { get; }

        public ObservableCollection<ICard> Hand
        {
            get
            {
                return _hand;
            }
            set
            {
                _hand = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ICard> PlayedCards
        {
            get
            {
                return _playedCards;
            }
            set
            {
                _playedCards = value;
                OnPropertyChanged();
            }
        }

        public bool IsDrawPhase
        {
            get => _isDrawPhase;
            set
            {
                _isDrawPhase = value;
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

        public bool IsMainPhase1
        {
            get => _isMainPhase1;
            set
            {
                _isMainPhase1 = value;
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

        public bool IsMainPhase2
        {
            get => _isMainPhase2;
            set
            {
                _isMainPhase2 = value;
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
                if (_cardsLeft == 0) return;
                Hand.Add(Deck[_deckOrder[_cardsLeft]]);
                OnPropertyChanged(nameof(Hand));
                _cardsLeft--;
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
                CanNormalSummonMonster = true;
                return;
            }
        }

        private void PlayACard(object parameter)
        {
            if (parameter is ICard == false) return;
            ICard card = (ICard)parameter;
            if (card.YuGiOhCardType == CardType.Monster && PlayedCards.Count < 5)
            {
                if (CanNormalSummonMonster == false) return;
                PlayedCards.Add(card);
                OnPropertyChanged(nameof(PlayedCards));
                Hand.Remove(card);
                OnPropertyChanged(nameof(Hand));
                CanNormalSummonMonster = false;
            }
        }

        private void AttackOpponent(object parameter)
        {
            if (parameter is ICard == false) return;
            CardModel card = (CardModel)parameter; //this cast shouldn't be necessary, should use the property to check if it is a monster
            if (card.CanAttack)
            {
                OpponentLifePoints = OpponentLifePoints - card.Attack;
                card.CanAttack = false;
            }
        }

        #endregion

        #region implement INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
