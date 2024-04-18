using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yu_Gi_Oh_Game.Model.MagicCards;
using Yu_Gi_Oh_Game.Model.MonsterCards;
using Yu_Gi_Oh_Game.Model.MonsterCards.EffectMonsters;

namespace Yu_Gi_Oh_Game.Model.Deck
{
    //should there be a property that keeps track of the index of each card in the deck kind of like an id? or is that overkill?
    public class DeckModel : IDeckModel
    {
        private readonly List<ICard> _deck;

        public DeckModel()
        {
            _deck = new List<ICard>();
            CreateCards();
        }

        public event EventHandler<DeckEventArgs> DeckUpdated;

        public IEnumerable<ICard> Deck { get => _deck; } //It is possible I may be able to encapsulate this too
        public int CardsLeft { get => Deck.Count(); }

        public void Shuffle()
        {
            for (int n = CardsLeft - 1; n > 0; --n)
            {
                int r = Random.Shared.Next(n + 1);
                (_deck[r], _deck[n]) = (_deck[n], _deck[r]);
            }
            OnDeckUpdated(DeckAction.Shuffle);
        }

        //TODO: this only adds them at the end (which is essentially the top of the deck), it needs an optional parameter to tell what location in the IEnumerable the cards need to go
        public void AddCards(IEnumerable<ICard> cards)
        {
            _deck.AddRange(cards);
            OnDeckUpdated(DeckAction.Add);
        }

        public void RemoveCard(int index) 
        {
            _deck.RemoveAt(index);
            OnDeckUpdated(DeckAction.Remove);
        }

        public ICard GetCard(int index)
        {
            return _deck[index];
        }

        //public IEnumerable<ICard> DrawCard(int numberOfCards)
        //{
        //    List<ICard> cards = new List<ICard>();
        //    for (int i = 0; i < numberOfCards; i++)
        //    {
        //        if (CardsLeft <= 0) break; //at some point make this to where the player loses the game
        //        var newCard = _deck[CardsLeft - 1];
        //        cards.Add(newCard);
        //        _deck.RemoveAt(CardsLeft - 1);
        //        OnDeckUpdated(DeckAction.Remove);
        //    }
        //    return cards;
        //}

        private void CreateCards()
        {
            NormalMonsterCardModel LusterDragon = new NormalMonsterCardModel("Luster Dragon", 1900, 1600);

            //Dark_Elf DarkElf = new Dark_Elf();

            NormalMonsterCardModel GeminiElf = new NormalMonsterCardModel("Gemini Elf", 1900, 900);

            NormalMonsterCardModel VorseRaider = new NormalMonsterCardModel("Vorse Raider", 1900, 1200);

            NormalMonsterCardModel DarkMagician = new NormalMonsterCardModel("Dark Magician", 2500, 2100);

            NormalMonsterCardModel RedEyesBlackDragon = new NormalMonsterCardModel("Red Eyes Black Dragon", 2400, 2000);

            NormalMonsterCardModel BlueEyesWhiteDragon = new NormalMonsterCardModel("Blue Eyes White Dragon", 3000, 2500);

            NormalMonsterCardModel SliferTheSkyDragon = new NormalMonsterCardModel("Slifer The Sky Dragon", 3500, 3500);

            NormalMonsterCardModel ObeliskTheTormentor = new NormalMonsterCardModel("Obelisk The Tormentor", 4000, 4000);

            NormalMonsterCardModel TheWingedDragonOfRa = new NormalMonsterCardModel("The Winged Dragon Of Ra", 5000, 5000);

            NormalMonsterCardModel DarkMagicianGirl = new NormalMonsterCardModel("Dark Magician Girl", 2000, 1700);

            IMagicTrapCard PotOfGreed = new PotOfGreed();

            IMagicTrapCard Ookazi = new Ookazi();

            _deck.Add(LusterDragon);
            _deck.Add(GeminiElf);
            _deck.Add(VorseRaider);
            _deck.Add(DarkMagician);
            _deck.Add(RedEyesBlackDragon);
            _deck.Add(BlueEyesWhiteDragon);
            _deck.Add(SliferTheSkyDragon);
            _deck.Add(ObeliskTheTormentor);
            _deck.Add(TheWingedDragonOfRa);
            _deck.Add(DarkMagicianGirl);
            _deck.Add(PotOfGreed);
            _deck.Add(Ookazi);
            //_deck.Add(DarkElf);
        }

        private void OnDeckUpdated(DeckAction action)
        {
            DeckUpdated?.Invoke(this, new DeckEventArgs(action));
        }
    }
}
