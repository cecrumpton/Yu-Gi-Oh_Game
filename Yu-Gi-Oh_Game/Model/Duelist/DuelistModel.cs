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
using Yu_Gi_Oh_Game.Model.Deck;
using Yu_Gi_Oh_Game.Model.MagicCards;
using Yu_Gi_Oh_Game.Model.MonsterCards;
using Yu_Gi_Oh_Game.ViewModel;

namespace Yu_Gi_Oh_Game.Model.Duelist
{
    public class DuelistModel : INotifyPropertyChanged
    {
        private readonly List<ICard> _deck;
        private readonly List<ICard> _hand;
        private readonly List<IMonsterCard> _playedMonsterCards;
        private readonly List<IMagicTrapCard> _playedMagicAndTrapCards;

        private int _lifePoints;
        private bool _isDrawPhase;
        private bool _isStandbyPhase;
        private bool _isMainPhase1;
        private bool _isBattlePhase;
        private bool _isMainPhase2;
        private bool _isEndPhase;
        private bool _canNormalSummonMonster;

        public DuelistModel(IDeckModel deckModel)
        {
            _deck = deckModel.Deck.ToList();
            _hand = new List<ICard>();
            _playedMonsterCards = new List<IMonsterCard>();
            _playedMagicAndTrapCards = new List<IMagicTrapCard>();
        }

        public event EventHandler<HandEventArgs> HandUpdated;

        private void OnHandUpdated(ICard card, HandAction action)
        {
            HandUpdated?.Invoke(this, new HandEventArgs(card, action));
        }

        public event EventHandler<DeckEventArgs> DeckUpdated;

        private void OnDeckUpdated(IEnumerable<ICard> deck, DeckAction action)
        {
            DeckUpdated?.Invoke(this, new DeckEventArgs(deck, action));
        }

        public event EventHandler<PlayCardEventArgs> PlayCardUpdated;

        private void OnPlayCardUpdated(ICard card)
        {
            PlayCardUpdated?.Invoke(this, new PlayCardEventArgs(card));
        }

        #region properties

        public IEnumerable<ICard> Deck { get => _deck; }
        public IEnumerable<ICard> Hand { get => _hand; }
        public IEnumerable<IMonsterCard> PlayedMonsterCards { get => _playedMonsterCards; }
        public IEnumerable<IMagicTrapCard> PlayedMagicAndTrapCards { get => _playedMagicAndTrapCards; }
        public int CardsLeft { get => Deck.Count(); }

        public int LifePoints
        {
            get => _lifePoints;
            set
            {
                _lifePoints = value;
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
                var newCard = _deck[CardsLeft - 1];
                _hand.Add(newCard);
                _deck.RemoveAt(CardsLeft - 1); //instead of modifying deck here, should call a method that modifys the deck in the deck class (aka it needs its on dra property)
                OnHandUpdated(newCard, HandAction.Add);
                OnDeckUpdated(Deck, DeckAction.Remove);
            }
        }

        public void ShuffleDeck() //TODO this should go in the deck model
        {
            for (int n = CardsLeft - 1; n > 0; --n)
            {
                int r = Random.Shared.Next(n + 1);
                (_deck[r], _deck[n]) = (_deck[n], _deck[r]);
            }
            OnDeckUpdated(Deck, DeckAction.Shuffle);
        }

        //when implementing chains I can remove the await and async out of this method.
        public void PlayACard(ICard card, DuelMatViewModel vm, DuelistModel opponent)
        {
            if (card.YuGiOhCardType == CardType.Monster && PlayedMonsterCards.Count() < 5)
            {
                if (CanNormalSummonMonster == false) return;
                _hand.Remove(card);
                _playedMonsterCards.Add((IMonsterCard)card);
                CanNormalSummonMonster = false;
            }
            if (card.YuGiOhCardType == CardType.Magic)
            {
                _hand.Remove(card);
                _playedMagicAndTrapCards.Add((IMagicTrapCard)card);
                //the duelist model shouldn't be resolving the effect of the card (or really anything else below this line)
                //await Task.Delay(2000);
                //card.ResolveEffect(this, vm, opponent);
                //if (card.IsContinuous == false)
                //    PlayedMagicAndTrapCards.Remove(magicCard);
            }
            OnHandUpdated(card, HandAction.Remove);
            OnPlayCardUpdated(card);
            //OnPropertyChanged(); // NotifyCardRemovedFromHand and NotifyCardAddedToPlayedCards (this can be separated between monsters and traps)
        }

        //This method likely doesn't belong in the duelist model, it ould be better off in a monster card model
        //TODO: these to methods can be condensed down in to one, similar to the draw cards method
        public void AttackOpponent(IMonsterCard monsterCard, int opponentLifePoints)
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
