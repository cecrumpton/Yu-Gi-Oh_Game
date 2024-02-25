using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Yu_Gi_Oh_Game.Model.Duelist;
using Yu_Gi_Oh_Game.Model.MagicCards;
using Yu_Gi_Oh_Game.Model.MonsterCards;

namespace Yu_Gi_Oh_Game.Model.Deck
{
    public class OpponentDeckModel : IDeckModel
    {
        private readonly List<ICard> _deck;

        public OpponentDeckModel()
        {
            _deck = new List<ICard>();
            CreateCards();
        }

        public IEnumerable<ICard> Deck { get => _deck; }
        public int CardsLeft { get => Deck.Count(); }

        public event EventHandler<DeckEventArgs> DeckUpdated;

        private void OnDeckUpdated(IEnumerable<ICard> deck, DeckAction action)
        {
            DeckUpdated?.Invoke(this, new DeckEventArgs(deck, action));
        }

        public void Shuffle()
        {
            for (int n = Deck.Count() - 1; n > 0; --n)
            {
                int r = Random.Shared.Next(n + 1);
                (_deck[r], _deck[n]) = (_deck[n], _deck[r]);
            }
            OnDeckUpdated(Deck, DeckAction.Shuffle);
        }

        private void CreateCards()
        {
            NormalMonsterCardModel LusterDragon = new NormalMonsterCardModel("Luster Dragon", 1900, 1600);

            NormalMonsterCardModel GeminiElf = new NormalMonsterCardModel("Gemini Elf", 1900, 900);

            NormalMonsterCardModel VorseRaider = new NormalMonsterCardModel("Vorse Raider", 1900, 1200);

            NormalMonsterCardModel DarkMagician = new NormalMonsterCardModel("Dark Magician", 2500, 2100);

            NormalMonsterCardModel RedEyesBlackDragon = new NormalMonsterCardModel("Red Eyes Black Dragon", 2400, 2000);

            NormalMonsterCardModel BlueEyesWhiteDragon = new NormalMonsterCardModel("Blue Eyes White Dragon", 3000, 2500);

            NormalMonsterCardModel SliferTheSkyDragon = new NormalMonsterCardModel("Slifer The Sky Dragon", 3500, 3500);

            NormalMonsterCardModel ObeliskTheTormentor = new NormalMonsterCardModel("Obelisk The Tormentor", 4000, 4000);

            NormalMonsterCardModel TheWingedDragonOfRa = new NormalMonsterCardModel("The Winged Dragon Of Ra", 5000, 5000);

            NormalMonsterCardModel DarkMagicianGirl = new NormalMonsterCardModel("Dark Magician Girl", 2000, 1700);


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
        }
    }
}
