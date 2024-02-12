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
            //DuelMatModel model = new DuelMatModel();
            //Deck = model.Cards;
            Deck = deckModel.Deck;
            CardsLeft = Deck.Count - 1;
            //ShuffleDeck(Deck);
            Hand = new ObservableCollection<ICard>();
        }

        //private List<ICard> ShuffleDeck(List<ICard> deck)
        //{
        //    for (int n = deck.Count - 1; n > 0; --n)
        //    {
        //        int k = Random.Shared.Next(n + 1);
        //        (deck[k], deck[n]) = (deck[n], deck[k]);
        //    }
        //    return deck;
        //}

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

        public ObservableCollection<ICard> Hand { get; }

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
                if (CardsLeft <= 0) return;
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

        //public void DrawCards(DuelistModel duelist, int numberOfCards)
        //{
        //    for (int i = 0; i < numberOfCards; i++)
        //    {
        //        if (duelist.CardsLeft <= 0) return;
        //        duelist.CardsLeft--;
        //        duelist.Hand.Add(Deck[duelist.CardsLeft]); //Deck is different
        //    }
        //}

        //when implementing chains I can remove the await and async out of this method.
        //TODO: fix magicCard.ResolveEffect()
        public async void PlayACard(ICard card)
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
                magicCard.ResolveEffect(this);
                if (magicCard.IsContinuous == false)
                    PlayedMagicAndTrapCards.Remove(magicCard);
            }
        }

        ////when implementing chains I can remove the await and async out of this method.
        ////TODO: these to methods can be condensed down in to one, similar to the draw cards method
        //private async void PlayerPlayACard(object parameter)
        //{
        //    if (parameter is ICard == false) return;
        //    ICard card = (ICard)parameter;
        //    if (card.YuGiOhCardType == CardType.Monster && PlayerMonsterCards.Count < 5)
        //    {
        //        if (PlayerCanNormalSummonMonster == false) return;
        //        PlayerMonsterCards.Add((MonsterCardModel)card);
        //        PlayerHand.Remove(card);
        //        PlayerCanNormalSummonMonster = false;
        //    }
        //    if (card.YuGiOhCardType == CardType.Magic)
        //    {
        //        MagicCardModel magicCard = (MagicCardModel)card;
        //        PlayerHand.Remove(magicCard);
        //        PlayerMagicAndTrapCards.Add(magicCard);
        //        await Task.Delay(2000);
        //        magicCard.ResolveEffect(Player, this);
        //        if (magicCard.IsContinuous == false)
        //            PlayerMagicAndTrapCards.Remove(magicCard);
        //    }
        //}

        //private void OpponentPlayACard(object parameter)
        //{
        //    if (parameter is ICard == false) return;
        //    ICard card = (ICard)parameter;
        //    if (card.YuGiOhCardType == CardType.Monster && OpponentMonsterCards.Count < 5)
        //    {
        //        if (OpponentCanNormalSummonMonster == false) return;
        //        OpponentMonsterCards.Add((MonsterCardModel)card);
        //        OpponentHand.Remove(card);
        //        OpponentCanNormalSummonMonster = false;
        //    }
        //}

        //TODO: these to methods can be condensed down in to one, similar to the draw cards method
        private void AttackOpponent(MonsterCardModel monsterCard, int opponentLifePoints)
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
