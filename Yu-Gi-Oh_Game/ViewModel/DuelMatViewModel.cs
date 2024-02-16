using Prism.Commands;
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

namespace Yu_Gi_Oh_Game.ViewModel
{
    public class DuelMatViewModel : INotifyPropertyChanged
    {
        private string _advancePhaseText;
        private bool _isFirstTurn;
        private bool _canAttackTarget;
        private MonsterCardModel _attackingMonsterCard;

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
            Attack = new DelegateCommand<MonsterCardModel>(AttackOpponent);
            AttackTarget = new DelegateCommand<MonsterCardModel>(AttackOpponentCard);
        }

        #region Properties

        public DuelistModel Player { get; }
        public DuelistModel Opponent { get; }
        public ICommand AdvancePhase { get; }
        public ICommand PlayCard { get; }
        public ICommand Attack { get; }
        public ICommand AttackTarget { get; }

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

        public bool CanAttackTarget
        {
            get => _canAttackTarget;
            set
            {
                _canAttackTarget = value;
                OnPropertyChanged();
            }
        }

        public MonsterCardModel? AttackingMonsterCard
        {
            get => _attackingMonsterCard;
            set
            {
                _attackingMonsterCard = value;
                OnPropertyChanged();
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
                Player.PlayACard(card, this, Opponent);
            }
        }

        //This is here to handle AI logic
        private void OpponentPlayACard(object parameter)
        {
            if (parameter is ICard == false) return;
            ICard card = (ICard)parameter;
            Opponent.PlayACard(card, this, Player);
        }

        //TODO: these to methods can be condensed down in to one, similar to the draw cards method
        //TODO: should the duielist model handle this logic, or should it be handled in the duel mat view model/duel mat model, or split between the two?
        private void AttackOpponent(MonsterCardModel card)
        {
            if(PlayerBattlePhase)
            {
                if (card.CanAttack)
                {
                    if (Opponent.PlayedMonsterCards.Count == 0)
                    {
                        OpponentLifePoints = OpponentLifePoints - card.Attack;
                        card.CanAttack = false;
                    }
                    else
                    {
                        AttackingMonsterCard = card;
                        CanAttackTarget = true;
                    }
                }
            }
        }

        private void AttackOpponentCard(MonsterCardModel cardToAttack)
        {
            if (CanAttackTarget)
            {
                if (AttackingMonsterCard == null) return;
                if (AttackingMonsterCard.Attack > cardToAttack.Attack)
                {
                    Opponent.PlayedMonsterCards.Remove(cardToAttack);
                    OpponentLifePoints -= (AttackingMonsterCard.Attack - cardToAttack.Attack);
                }
                if (AttackingMonsterCard.Attack < cardToAttack.Attack)
                {
                    Player.PlayedMonsterCards.Remove(AttackingMonsterCard);
                    PlayerLifePoints -= (cardToAttack.Attack - AttackingMonsterCard.Attack);
                }
                if (AttackingMonsterCard.Attack == cardToAttack.Attack)
                {
                    Player.PlayedMonsterCards.Remove(AttackingMonsterCard);
                    Opponent.PlayedMonsterCards.Remove(cardToAttack);
                }

                AttackingMonsterCard.CanAttack = false;
                AttackingMonsterCard = null;
                CanAttackTarget = false;
            }
        }

        private void AttackPlayer(object parameter)
        {
            if (parameter is MonsterCardModel == false) return;
            MonsterCardModel card = (MonsterCardModel)parameter; //this cast shouldn't be necessary, should use the property to check if it is a monster
            if (card.CanAttack)
            {
                if (Player.PlayedMonsterCards.Count == 0)
                {
                    PlayerLifePoints = PlayerLifePoints - card.Attack;
                    card.CanAttack = false;
                }
                else
                {
                    AttackingMonsterCard = card;
                    AttackPlayerCard(Player.PlayedMonsterCards[0]);
                }
            }
        }

        private void AttackPlayerCard(MonsterCardModel cardToAttack)
        {
            if (AttackingMonsterCard == null) return;
            if (AttackingMonsterCard.Attack > cardToAttack.Attack)
            {
                Player.PlayedMonsterCards.Remove(cardToAttack);
                PlayerLifePoints -= (AttackingMonsterCard.Attack - cardToAttack.Attack);
            }
            if (AttackingMonsterCard.Attack < cardToAttack.Attack)
            {
                Opponent.PlayedMonsterCards.Remove(AttackingMonsterCard);
                OpponentLifePoints -= (cardToAttack.Attack - AttackingMonsterCard.Attack);
            }
            if (AttackingMonsterCard.Attack == cardToAttack.Attack)
            {
                Opponent.PlayedMonsterCards.Remove(AttackingMonsterCard);
                Player.PlayedMonsterCards.Remove(cardToAttack);
            }

            AttackingMonsterCard.CanAttack = false;
            AttackingMonsterCard = null;
            CanAttackTarget = false;
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
            OnPropertyChanged(nameof(PlayerMonsterCards));
        }
        private void Opponent_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(OpponentDrawPhase));
            OnPropertyChanged(nameof(OpponentStandbyPhase));
            OnPropertyChanged(nameof(OpponentMainPhase1));
            OnPropertyChanged(nameof(OpponentBattlePhase));
            OnPropertyChanged(nameof(OpponentMainPhase2));
            OnPropertyChanged(nameof(OpponentEndPhase));
            OnPropertyChanged(nameof(OpponentLifePoints));
            OnPropertyChanged(nameof(OpponentMonsterCards));
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
