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
            //DuelMatModel modelPlayer = new DuelMatModel();
            //DuelMatModel modelOpponent = new DuelMatModel();
            Player = new DuelistModel(new DeckModel());
            Opponent = new DuelistModel(new OpponentDeckModel());
            //DuelistModel player = new DuelistModel();
            //DuelistModel opponent = new DuelistModel();

            //Deck = modelPlayer.Cards;
            //OpponentDeck = modelOpponent.OpponentCards;

            //DeckOrder = new int[Deck.Count];
            //OpponentDeckOrder = new int[OpponentDeck.Count];

            //ShuffleDeck(DeckOrder);
            //ShuffleDeck(OpponentDeckOrder);
            //Deck = ShuffleDeck2(Deck);
            //OpponentDeck = ShuffleDeck2(OpponentDeck);

            //PlayerCardsLeft = Deck.Count - 1;
            //OpponentCardsLeft = OpponentDeck.Count - 1;

            //Hand = new ObservableCollection<ICard>();
            //OpponentHand = new ObservableCollection<ICard>();

            //DrawCards(5, true);
            //DrawCards(5, false);
            //DrawCards(5, true);
            //DrawCards(5, false);
            //Player.DrawCards(5);
            //Opponent.DrawCards(5);
            Deck = ShuffleDeck2(Player.Deck);
            OpponentDeck = ShuffleDeck2(Opponent.Deck);
            DrawCards(Player, 5);
            DrawCards(Opponent, 5);

            PlayerLifePoints = OpponentLifePoints = 8000;

            PlayerDrawPhase = true;

            PlayerMonsterCards = new ObservableCollection<MonsterCardModel>();
            OpponentMonsterCards = new ObservableCollection<MonsterCardModel>();

            PlayerMagicAndTrapCards = new ObservableCollection<ICard>();
            OpponentMagicAndTrapCards = new ObservableCollection<ICard>();

            AdvancePhase = new RelayCommand(AdvanceTurnPhase, CheckIfPlayerTurn); //will eventaully check if it is player's turn

            PlayCard = new RelayCommand(PlayerPlayACard, CheckMainPhase);

            Attack = new RelayCommand(AttackOpponent, CheckBattlePhase);

            AdvancePhaseText = "Draw";
        }

        #region Properties

        public DuelistModel Player { get; }
        public DuelistModel Opponent { get;}
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

        public List<ICard> Deck { get; }
        public List<ICard> OpponentDeck { get; }

        public ObservableCollection<ICard> PlayerHand { get => Player.Hand; }
        public ObservableCollection<ICard> OpponentHand { get => Opponent.Hand; }

        public ObservableCollection<MonsterCardModel> PlayerMonsterCards { get; }
        public ObservableCollection<MonsterCardModel> OpponentMonsterCards { get; }


        public ObservableCollection<ICard> PlayerMagicAndTrapCards { get; }
        public ObservableCollection<ICard> OpponentMagicAndTrapCards { get; }

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
        //maybe this can be made private since it generally shouldn't be accessible other than for pot of greed?
        //It may actually be best that pot of greed can't access this method, and instead the draw card method should be placed somewhere in a common action method?
        //TODO: fix this so thatthe boolean isn't necessary. There needs to be a PlayerModel class for User and Opponent and that'll reduce a ton of repeated code.
        //public void DrawCards(int numberOfCards, bool isPlayer)
        //{
        //    if (isPlayer)
        //    {
        //        for (int i = 0; i < numberOfCards; i++)
        //        {
        //            if (CardsLeft < 0) return;
        //            Hand.Add(Deck[DeckOrder[CardsLeft]]);
        //            CardsLeft--;
        //        }
        //    }
        //    else
        //    {
        //        for (int i = 0; i < numberOfCards; i++)
        //        {
        //            if (OpponentCardsLeft < 0) return;
        //            OpponentHand.Add(OpponentDeck[OpponentDeckOrder[OpponentCardsLeft]]);
        //            OpponentCardsLeft--;
        //        }
        //    }
        //}

        public void DrawCards(int numberOfCards, bool isPlayer)
        {
            if (isPlayer)
            {
                for (int i = 0; i < numberOfCards; i++)
                {
                    if (PlayerCardsLeft < 0) return;
                    PlayerHand.Add(Deck[PlayerCardsLeft]);
                    PlayerCardsLeft--;
                }
            }
            else
            {
                for (int i = 0; i < numberOfCards; i++)
                {
                    if (OpponentCardsLeft < 0) return;
                    OpponentHand.Add(OpponentDeck[OpponentCardsLeft]);
                    OpponentCardsLeft--;
                }
            }
        }

        public void DrawCards(DuelistModel duelist, int numberOfCards)
        {
            for (int i = 0; i < numberOfCards; i++)
            {
                if (duelist.CardsLeft <= 0) return;
                duelist.CardsLeft--;
                duelist.Hand.Add(Deck[duelist.CardsLeft]);
            }
        }

        #endregion

        #region PrivateMethods

        //private void ShuffleDeck(int[] deck)
        //{
        //    for (int i = 0; i < Deck.Count; i++)
        //    {
        //        deck[i] = i;
        //    }

        //    for (int n = deck.Length - 1; n > 0; --n)
        //    {
        //        int k = _random.Next(n + 1);
        //        int temp = deck[n];
        //        deck[n] = deck[k];
        //        deck[k] = temp;
        //    }
        //}

        //TODO: move this somewhere else
        //private static readonly object syncLock = new object();

        private List<ICard> ShuffleDeck2(List<ICard> deck)
        {
            for (int n = deck.Count - 1; n > 0; --n)
            {
                int k = Random.Shared.Next(n + 1);
                (deck[k], deck[n]) = (deck[n], deck[k]);
            }
            return deck;
        }

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
                DrawCards(Player, 1);
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
                DrawCards(Opponent, 1);
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

        //TODO: this will be the method to replace the ones below
        //private async void PlayACard(DuelistModel duelist, object parameter)
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

        //when implementing chains I can remove the await and async out of this method.
        //TODO: these to methods can be condensed down in to one, similar to the draw cards method
        private async void PlayerPlayACard(object parameter)
        {
            if (parameter is ICard == false) return;
            ICard card = (ICard)parameter;
            if (card.YuGiOhCardType == CardType.Monster && PlayerMonsterCards.Count < 5)
            {
                if (PlayerCanNormalSummonMonster == false) return;
                PlayerMonsterCards.Add((MonsterCardModel)card);
                PlayerHand.Remove(card);
                PlayerCanNormalSummonMonster = false;
            }
            if (card.YuGiOhCardType == CardType.Magic)
            {
                MagicCardModel magicCard = (MagicCardModel) card;
                PlayerHand.Remove(magicCard);
                PlayerMagicAndTrapCards.Add(magicCard);
                await Task.Delay(2000);
                magicCard.ResolveEffect(Player, this);
                if(magicCard.IsContinuous == false)
                    PlayerMagicAndTrapCards.Remove(magicCard);
            }
        }

        private void OpponentPlayACard(object parameter)
        {
            if (parameter is ICard == false) return;
            ICard card = (ICard)parameter;
            if (card.YuGiOhCardType == CardType.Monster && OpponentMonsterCards.Count < 5)
            {
                if (OpponentCanNormalSummonMonster == false) return;
                OpponentMonsterCards.Add((MonsterCardModel)card);
                OpponentHand.Remove(card);
                OpponentCanNormalSummonMonster = false;
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
