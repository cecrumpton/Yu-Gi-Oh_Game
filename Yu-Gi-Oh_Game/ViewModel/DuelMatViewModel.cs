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
        private bool _isMainPhase1;
        private bool _isBattlePhase;
        private bool _isEndPhase;
        private readonly int[] _deckOrder;
        private static readonly Random _random = new Random();
        private int _cardsLeft;
        ObservableCollection<CardModel> _hand = new ObservableCollection<CardModel>();
        ObservableCollection<CardModel> _playedCards = new ObservableCollection<CardModel>();

        public DuelMatViewModel()
        {
            DuelMatModel model = new DuelMatModel();
            Deck = model.Cards;
            _deckOrder = new int[Deck.Count];
            ShuffleDeck(_deckOrder);
            Hand = new ObservableCollection<CardModel>
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
            PlayedCards = new ObservableCollection<CardModel>();
            DrawCard = new RelayCommand(DrawACard, CheckDrawPhase);
            PlayCard = new RelayCommand(PlayACard, CheckMainPhase1);
            Attack = new RelayCommand(AttackOpponent, CheckBattlePhase);
        }

        #region Properties

        public ICommand DrawCard { get; }
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

        public ObservableCollection<CardModel> Hand
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

        public ObservableCollection<CardModel> PlayedCards
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
            get
            {
                return _isDrawPhase;
            }
            set
            {
                _isDrawPhase = value;
                OnPropertyChanged();
            }
        }

        public bool IsMainPhase1
        {
            get
            {
                return _isMainPhase1;
            }
            set
            {
                _isMainPhase1 = value;
                OnPropertyChanged();
            }
        }

        public bool IsBattlePhase
        {
            get
            {
                return _isBattlePhase;
            }
            set
            {
                _isBattlePhase = value;
                OnPropertyChanged();
            }
        }

        public bool IsEndPhase
        {
            get
            {
                return _isEndPhase;
            }
            set
            {
                _isEndPhase = value;
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

        private bool CheckDrawPhase(object parameter)
        {
            return IsDrawPhase;
        }

        private bool CheckMainPhase1(object parameter)
        {
            return IsMainPhase1;
        }

        private bool CheckBattlePhase(object parameter)
        {
            return IsBattlePhase;
        }

        private void DrawACard(object parameter)
        {
            if (_cardsLeft == 0) return;
            Hand.Add(Deck[_deckOrder[_cardsLeft]]);
            OnPropertyChanged(nameof(Hand));
            _cardsLeft--;
            IsDrawPhase = false;
            if(PlayedCards.Count >= 5)
                IsBattlePhase = true;
            IsMainPhase1 = true;
        }

        private void PlayACard(object parameter)
        {
            if (parameter is CardModel == false) return;
            CardModel card = (CardModel)parameter;
            PlayedCards.Add(card);
            OnPropertyChanged(nameof(PlayedCards));
            Hand.Remove(card);
            OnPropertyChanged(nameof(Hand));
            IsMainPhase1 = false;
            IsBattlePhase = true;
        }

        private void AttackOpponent(object parameter)
        {
            if (parameter is CardModel == false) return;
            CardModel card = (CardModel)parameter;
            OpponentLifePoints = OpponentLifePoints - card.Attack;
            IsBattlePhase = false;
            IsEndPhase = true;
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
