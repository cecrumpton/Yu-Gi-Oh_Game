using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
using Yu_Gi_Oh_Game.Model.Deck;
using Yu_Gi_Oh_Game.Model.Duelist;
using Yu_Gi_Oh_Game.Model.Graveyard;
using Yu_Gi_Oh_Game.Model.Hand;
using Yu_Gi_Oh_Game.Model.MagicCards;
using Yu_Gi_Oh_Game.Model.MonsterCards;
using Yu_Gi_Oh_Game.Model.PlayedCards;

namespace Yu_Gi_Oh_Game.ViewModel
{
    public class DuelMatViewModel : BindableBase
    {
        private string _advancePhaseText;
        private bool _isFirstTurn;
        private bool _canAttackTarget;
        private IMonsterCard _attackingMonsterCard;
        private bool _isAttackDeclared;
        private readonly ObservableCollection<ICard> _deck;
        private readonly ObservableCollection<ICard> _hand;

        public DuelMatViewModel()
        {
            Player = new DuelistModel(new DeckModel(), new HandModel(), new PlayedCardsModel(), new GraveyardModel());
            Opponent = new DuelistModel(new OpponentDeckModel(), new HandModel(), new PlayedCardsModel(), new GraveyardModel());
            
            PlayerHand = new ObservableCollection<ICard>(Player.HandModel.Hand);
            PlayerMonsterCards = new ObservableCollection<IMonsterCard>(Player.PlayedCardsModel.PlayedMonsterCards);
            PlayerMagicAndTrapCards = new ObservableCollection<IMagicTrapCard>(Player.PlayedCardsModel.PlayedMagicTrapCards);

            OpponentHand = new ObservableCollection<ICard>(Opponent.HandModel.Hand);
            OpponentMonsterCards = new ObservableCollection<IMonsterCard>(Opponent.PlayedCardsModel.PlayedMonsterCards);
            OpponentMagicAndTrapCards = new ObservableCollection<IMagicTrapCard>(Opponent.PlayedCardsModel.PlayedMagicTrapCards);

            Player.HandModel.HandUpdated += Player_HandUpdated;
            Player.PlayedCardsModel.PlayedCardUpdated += Player_PlayCardUpdated;
            Player.PropertyChanged += Player_PropertyChanged;

            Opponent.HandModel.HandUpdated += Opponent_HandUpdated;
            Opponent.PlayedCardsModel.PlayedCardUpdated += Opponent_PlayCardUpdated;
            Opponent.PropertyChanged += Opponent_PropertyChanged;

            Player.ShuffleDeck();
            Opponent.ShuffleDeck();

            Player.DrawCards(5);
            Opponent.DrawCards(5);

            Random random = new Random();
            var turn = random.Next(0, 2);

            #if DEBUG
                turn = 0;
            #endif

            if (turn == 0)
            {
                PlayerDrawPhase = true;
                AdvancePhaseText = "Draw";
            }
            else
            {
                OpponentDrawPhase = true;
                AdvancePhaseText = "Opponent's Turn";
                ExecuteAIOpponent();
            }

            _isFirstTurn = true;

            AdvancePhase = new DelegateCommand(AdvanceTurnPhase);
            PlayCard = new DelegateCommand<ICard>(PlayACard);
            Attack = new DelegateCommand<IMonsterCard>(AttackOpponent);
            AttackTarget = new DelegateCommand<IMonsterCard>(AttackOpponentCard);
        }

        #region Properties

        public DuelistModel Player { get; }
        public DuelistModel Opponent { get; }
        public ICommand AdvancePhase { get; }
        public ICommand PlayCard { get; }
        public ICommand Attack { get; }
        public ICommand AttackTarget { get; }

        public int PlayerLifePointsDisplay
        {
            get => Player.LifePoints;
            set
            {
                Player.LifePoints = value;
                RaisePropertyChanged();
            }
        }

        public int OpponentLifePointsDisplay
        {
            get => Opponent.LifePoints;
            set
            {
                Opponent.LifePoints = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ICard> Deck { get; }
        public ObservableCollection<ICard> OpponentDeck { get; }
        public ObservableCollection<ICard> PlayerHand { get; }
        public ObservableCollection<ICard> OpponentHand { get; }
        public ObservableCollection<IMonsterCard> PlayerMonsterCards { get ; }
        public ObservableCollection<IMonsterCard> OpponentMonsterCards { get; }
        public ObservableCollection<IMagicTrapCard> PlayerMagicAndTrapCards { get; }
        public ObservableCollection<IMagicTrapCard> OpponentMagicAndTrapCards { get; }

        public bool IsPlayerTurn
        {
            get => PlayerDrawPhase || PlayerStandbyPhase || PlayerMainPhase1 || 
                PlayerBattlePhase || PlayerMainPhase2 || PlayerEndPhase;
        }

        public bool IsOpponentTurn
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
                RaisePropertyChanged();
            }
        }

        public bool OpponentDrawPhase
        {
            get => Opponent.IsDrawPhase;
            set
            {
                Opponent.IsDrawPhase = value;
                RaisePropertyChanged();
            }
        }

        public bool PlayerStandbyPhase
        {
            get => Player.IsStandbyPhase;
            set
            {
                Player.IsStandbyPhase = value;
                RaisePropertyChanged();
            }
        }

        public bool OpponentStandbyPhase
        {
            get => Opponent.IsStandbyPhase;
            set
            {
                Opponent.IsStandbyPhase = value;
                RaisePropertyChanged();
            }
        }

        public bool PlayerMainPhase1
        {
            get => Player.IsMainPhase1;
            set
            {
                Player.IsMainPhase1 = value;
                RaisePropertyChanged();
            }
        }

        public bool OpponentMainPhase1
        {
            get => Opponent.IsMainPhase1;
            set
            {
                Opponent.IsMainPhase1 = value;
                RaisePropertyChanged();
            }
        }

        public bool PlayerBattlePhase
        {
            get => Player.IsBattlePhase;
            set
            {
                Player.IsBattlePhase = value;
                RaisePropertyChanged();
            }
        }

        public bool OpponentBattlePhase
        {
           get => Opponent.IsBattlePhase;
            set
            {
                Opponent.IsBattlePhase = value;
                RaisePropertyChanged();
            }
        }

        public bool PlayerMainPhase2
        {
            get => Player.IsMainPhase2;
            set
            {
                Player.IsMainPhase2 = value;
                RaisePropertyChanged();
            }
        }

        public bool OpponentMainPhase2
        {
            get => Opponent.IsMainPhase2;
            set
            {
                Opponent.IsMainPhase2 = value;
                RaisePropertyChanged();
            }
        }

        public bool PlayerEndPhase
        {
            get => Player.IsEndPhase;
            set
            {
                Player.IsEndPhase = value;
                RaisePropertyChanged();
            }
        }

        public bool OpponentEndPhase
        {
            get => Opponent.IsEndPhase;
            set
            {
                Opponent.IsEndPhase = value;
                RaisePropertyChanged();
            }
        }

        public bool PlayerCanNormalSummonMonster
        {
            get => Player.CanNormalSummonMonster;
            set
            {
                Player.CanNormalSummonMonster = value;
                RaisePropertyChanged();
            }
        }

        public bool OpponentCanNormalSummonMonster
        {
            get => Opponent.CanNormalSummonMonster;
            set
            {
                Opponent.CanNormalSummonMonster = value;
                RaisePropertyChanged();
            }
        }

        public string AdvancePhaseText
        {
            get => _advancePhaseText;
            set
            {
                _advancePhaseText = value;
                RaisePropertyChanged();
            }
        }

        public bool CanAttackTarget
        {
            get => _canAttackTarget;
            set
            {
                _canAttackTarget = value;
                RaisePropertyChanged();
            }
        }

        public IMonsterCard? AttackingMonsterCard
        {
            get => _attackingMonsterCard;
            set
            {
                _attackingMonsterCard = value;
                RaisePropertyChanged();
            }
        }

        public bool IsAttackDeclared //This is currently used for Dark Elf, but can also be used for trap card activation
        {
            get => _isAttackDeclared;
            set
            {
                _isAttackDeclared = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region PublicMethods

        #endregion

        #region PrivateMethods

        private void AdvanceTurnPhase()
        {
            if (IsPlayerTurn)
            {
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
            }
        }

        private async void ExecuteDrawAndStandbyPhase(DuelistModel duelist)
        {
            duelist.DrawCards(1);
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
        }

        private void ExecuteMainPhase2(DuelistModel duelist)
        {
            duelist.IsBattlePhase = false;
            duelist.IsMainPhase2 = true;
            AdvancePhaseText = "End Turn";
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
            Opponent.DrawCards(1);
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
                //TODO: iterate through each monster card instead of just the first monster
                //foreach (var monsterCard in OpponentMonsterCards)
                //{
                //    monsterCard.CanAttack = true;
                //    AttackPlayer(monsterCard);
                //    await Task.Delay(2000);
                //}

                OpponentMonsterCards[0].CanAttack = true;
                AttackPlayer(OpponentMonsterCards[0]);
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
        }

        private void PlayACard(ICard card)
        {
            if (PlayerMainPhase1 || PlayerMainPhase2)
            {
                Player.PlayACard(card, Opponent);
            }
        }

        //This is here to handle AI logic
        private void OpponentPlayACard(object parameter)
        {
            if (parameter is ICard == false) return;
            ICard card = (ICard)parameter;
            Opponent.PlayACard(card, Player);
        }

        //TODO: these to methods can be condensed down in to one, similar to the draw cards method
        //TODO: should the duielist model handle this logic, or should it be handled in the duel mat view model/duel mat model, or split between the two?
        private void AttackOpponent(IMonsterCard card)
        {
            if(PlayerBattlePhase)
            {
                if (card.CanAttack)
                {
                    if (OpponentMonsterCards.Count() == 0)
                    {
                        AttackingMonsterCard = card;
                        IsAttackDeclared = true;
                        OpponentLifePointsDisplay = OpponentLifePointsDisplay - card.Attack;
                        card.CanAttack = false;
                        AttackingMonsterCard = null;
                        IsAttackDeclared = false;
                    }
                    else
                    {
                        AttackingMonsterCard = card;
                        CanAttackTarget = true;
                    }
                }
            }
        }

        private void AttackOpponentCard(IMonsterCard cardToAttack)
        {
            if (CanAttackTarget)
            {
                if (AttackingMonsterCard == null) return;
                IsAttackDeclared = true;
                if (AttackingMonsterCard.Attack > cardToAttack.Attack)
                {
                    Opponent.PlayedCardsModel.RemoveMonsterCard(cardToAttack);
                    Opponent.GraveyardModel.AddCard(cardToAttack);
                    OpponentLifePointsDisplay -= (AttackingMonsterCard.Attack - cardToAttack.Attack);
                }
                if (AttackingMonsterCard.Attack < cardToAttack.Attack)
                {
                    Player.PlayedCardsModel.RemoveMonsterCard(AttackingMonsterCard);
                    Player.GraveyardModel.AddCard(AttackingMonsterCard);
                    PlayerLifePointsDisplay -= (cardToAttack.Attack - AttackingMonsterCard.Attack);
                }
                if (AttackingMonsterCard.Attack == cardToAttack.Attack)
                {
                    Player.PlayedCardsModel.RemoveMonsterCard(AttackingMonsterCard);
                    Player.GraveyardModel.AddCard(AttackingMonsterCard);
                    Opponent.PlayedCardsModel.RemoveMonsterCard(cardToAttack);
                    Opponent.GraveyardModel.AddCard(cardToAttack);
                }

                AttackingMonsterCard.CanAttack = false;
                AttackingMonsterCard = null;
                IsAttackDeclared = false;
                CanAttackTarget = false;
            }
        }

        private void AttackPlayer(IMonsterCard card)
        {
            if (card.CanAttack)
            {
                if (PlayerMonsterCards.Count() == 0)
                {
                    PlayerLifePointsDisplay = PlayerLifePointsDisplay - card.Attack;
                    card.CanAttack = false;
                }
                else
                {
                    AttackingMonsterCard = card;
                    AttackPlayerCard(PlayerMonsterCards[0]);
                }
            }
        }

        private void AttackPlayerCard(IMonsterCard cardToAttack)
        {
            if (AttackingMonsterCard == null) return;
            if (AttackingMonsterCard.Attack > cardToAttack.Attack)
            {
                Player.PlayedCardsModel.RemoveMonsterCard(cardToAttack);
                Player.GraveyardModel.AddCard(cardToAttack);
                PlayerLifePointsDisplay -= (AttackingMonsterCard.Attack - cardToAttack.Attack);
            }
            if (AttackingMonsterCard.Attack < cardToAttack.Attack)
            {
                Opponent.PlayedCardsModel.RemoveMonsterCard(AttackingMonsterCard);
                Opponent.GraveyardModel.AddCard(AttackingMonsterCard);
                OpponentLifePointsDisplay -= (cardToAttack.Attack - AttackingMonsterCard.Attack);
            }
            if (AttackingMonsterCard.Attack == cardToAttack.Attack)
            {
                Opponent.PlayedCardsModel.RemoveMonsterCard(AttackingMonsterCard);
                Opponent.GraveyardModel.AddCard(AttackingMonsterCard);
                Player.PlayedCardsModel.RemoveMonsterCard(cardToAttack);
                Player.GraveyardModel.AddCard(cardToAttack);
            }

            AttackingMonsterCard.CanAttack = false;
            AttackingMonsterCard = null;
            CanAttackTarget = false;
        }

        private void Player_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(PlayerDrawPhase));
            RaisePropertyChanged(nameof(PlayerStandbyPhase));
            RaisePropertyChanged(nameof(PlayerMainPhase1));
            RaisePropertyChanged(nameof(PlayerBattlePhase));
            RaisePropertyChanged(nameof(PlayerMainPhase2));
            RaisePropertyChanged(nameof(PlayerEndPhase));
            RaisePropertyChanged(nameof(PlayerLifePointsDisplay));
            RaisePropertyChanged(nameof(PlayerMonsterCards));
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

        private async void Player_PlayCardUpdated(object? sender, PlayedCardEventArgs e)
        {
            if (e.Card.YuGiOhCardType == CardType.Monster)
            {
                if (e.Card is not IMonsterCard card) return;
                if (e.Action == PlayedCardAction.AddMonster)
                    PlayerMonsterCards.Add(card);
                if (e.Action == PlayedCardAction.RemoveMonster)
                    PlayerMonsterCards.Remove(card);
            }
            else if (e.Card.YuGiOhCardType == CardType.Magic)
            {
                if (e.Card is not IMagicTrapCard card) return;
                PlayerMagicAndTrapCards.Add(card);
                await Task.Delay(2000);
                card.ResolveEffect(Player, Opponent);
                if (card.IsContinuous == false)
                    PlayerMagicAndTrapCards.Remove(card);
            }
        }

        private void Opponent_HandUpdated(object? sender, HandEventArgs e)
        {
            if (e.Action == HandAction.Add)
                OpponentHand.Add(e.Card);
            else
                OpponentHand.Remove(e.Card); //todo: this won't tecnically remove the card, just the first instance of it
        }

        private async void Opponent_PlayCardUpdated(object? sender, PlayedCardEventArgs e)
        {
            if (e.Card.YuGiOhCardType == CardType.Monster)
            {
                if (e.Card is not IMonsterCard card) return;
                if(e.Action == PlayedCardAction.AddMonster)
                    OpponentMonsterCards.Add(card);
                if (e.Action == PlayedCardAction.RemoveMonster)
                    OpponentMonsterCards.Remove(card);
            }
            else if (e.Card.YuGiOhCardType == CardType.Magic)
            {
                if (e.Card is not IMagicTrapCard card) return;
                OpponentMagicAndTrapCards.Add(card);
                await Task.Delay(2000);
                card.ResolveEffect(Player, Opponent);
                if (card.IsContinuous == false)
                    OpponentMagicAndTrapCards.Remove(card);
            }
        }

        private void Opponent_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(OpponentDrawPhase));
            RaisePropertyChanged(nameof(OpponentStandbyPhase));
            RaisePropertyChanged(nameof(OpponentMainPhase1));
            RaisePropertyChanged(nameof(OpponentBattlePhase));
            RaisePropertyChanged(nameof(OpponentMainPhase2));
            RaisePropertyChanged(nameof(OpponentEndPhase));
            RaisePropertyChanged(nameof(OpponentLifePointsDisplay));
            RaisePropertyChanged(nameof(OpponentMonsterCards));
        }

        #endregion

    }
}
