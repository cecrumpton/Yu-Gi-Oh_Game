using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Yu_Gi_Oh_Game.Model.MagicCards;
using Yu_Gi_Oh_Game.Other;
using Yu_Gi_Oh_Game.ViewModel;

namespace Yu_Gi_Oh_Game.Model
{
    public class DuelistModel : INotifyPropertyChanged
    {
        private int _lifePoints;
        private bool _isDrawPhase;
        private bool _isStandbyPhase;
        private bool _isMainPhase1;
        private bool _isBattlePhase;
        private bool _isMainPhase2;
        private bool _isEndPhase;
        private string _advancePhaseText;
        private bool _canNormalSummonMonster;

        public DuelistModel(IDeckModel deckModel)
        {
            Deck = deckModel.Deck;
            CardsLeft = Deck.Count - 1;
            Hand = new ObservableCollection<ICard>();
            PlayedMonsterCards = new ObservableCollection<MonsterCardModel>();
            PlayedMagicAndTrapCards = new ObservableCollection<ICard>();
        }

        #region properties
        public ICommand AdvancePhase { get; }
        public ICommand PlayCard { get; }
        public ICommand Attack { get; }

        public int LifePoints
        {
            get => _lifePoints;
            set
            {
                _lifePoints = value;
                OnPropertyChanged();
            }
        }
        public List<ICard> Deck { get; }
        public int CardsLeft { get; set; }

        public ObservableCollection<ICard> Hand { get; set; }

        //TODO: make an IMonsterCard that inherits ICard
        public ObservableCollection<MonsterCardModel> PlayedMonsterCards { get; }

        public ObservableCollection<ICard> PlayedMagicAndTrapCards { get; }


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

        #region methods

        public void DrawCard(int numberOfCards)
        {
            for (int i = 0; i < numberOfCards; i++)
            {
                if (CardsLeft <= 0) return; //at some point make this to where the player loses the game
                Hand.Add(Deck[CardsLeft]);
                CardsLeft--;
            }            
        }

        public void ShuffleDeck()
        {
            for (int n = Deck.Count - 1; n > 0; --n)
            {
                int r = Random.Shared.Next(n + 1);
                (Deck[r], Deck[n]) = (Deck[n], Deck[r]);
            }
        }

        //when implementing chains I can remove the await and async out of this method.
        public async void PlayACard(ICard card, DuelMatViewModel vm, DuelistModel opponent)
        {
            if (card.YuGiOhCardType == CardType.Monster && PlayedMonsterCards.Count < 5)
            {
                if (CanNormalSummonMonster == false) return;
                Hand.Remove(card);
                PlayedMonsterCards.Add((MonsterCardModel)card);
                CanNormalSummonMonster = false;
            }
            if (card.YuGiOhCardType == CardType.Magic)
            {
                MagicCardModel magicCard = (MagicCardModel)card;
                Hand.Remove(magicCard);
                PlayedMagicAndTrapCards.Add(magicCard);
                await Task.Delay(2000);
                magicCard.ResolveEffect(this, vm, opponent);
                if (magicCard.IsContinuous == false)
                    PlayedMagicAndTrapCards.Remove(magicCard);
            }
        }

        //TODO: these to methods can be condensed down in to one, similar to the draw cards method
        public void AttackOpponent(MonsterCardModel monsterCard, int opponentLifePoints)
        {
            //if (parameter is MonsterCardModel == false) return;
            //MonsterCardModel card = (MonsterCardModel)parameter; //this cast shouldn't be necessary, should use the property to check if it is a monster
            if (monsterCard.CanAttack)
            {
                opponentLifePoints = opponentLifePoints - monsterCard.Attack;
                monsterCard.CanAttack = false;
            }
        }

        ////TODO: these to methods can be condensed down in to one, similar to the draw cards method
        //private void AttackOpponent(object parameter)
        //{
        //    if (parameter is MonsterCardModel == false) return;
        //    MonsterCardModel card = (MonsterCardModel)parameter; //this cast shouldn't be necessary, should use the property to check if it is a monster
        //    if (card.CanAttack)
        //    {
        //        OpponentLifePoints = OpponentLifePoints - card.Attack;
        //        card.CanAttack = false;
        //    }
        //}

        //private void AttackPlayer(object parameter)
        //{
        //    if (parameter is MonsterCardModel == false) return;
        //    MonsterCardModel card = (MonsterCardModel)parameter; //this cast shouldn't be necessary, should use the property to check if it is a monster
        //    if (card.CanAttack)
        //    {
        //        PlayerLifePoints = PlayerLifePoints - card.Attack;
        //        card.CanAttack = false;
        //    }
        //}
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
