using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yu_Gi_Oh_Game.Model.MagicCards;

namespace Yu_Gi_Oh_Game.Model
{
    public class DeckModel : IDeckModel
    {
        public DeckModel()
        {
            Deck = new List<ICard>();
            CreateCards();
        }

        public List<ICard> Deck { get; }

        private void CreateCards()
        {
            MonsterCardModel LusterDragon = new MonsterCardModel("Luster Dragon", 1900, 1600);

            MonsterCardModel GeminiElf = new MonsterCardModel("Gemini Elf", 1900, 900);

            MonsterCardModel VorseRaider = new MonsterCardModel("Vorse Raider", 1900, 1200);

            MonsterCardModel DarkMagician = new MonsterCardModel("Dark Magician", 2500, 2100);

            MonsterCardModel RedEyesBlackDragon = new MonsterCardModel("Red Eyes Black Dragon", 2400, 2000);

            MonsterCardModel BlueEyesWhiteDragon = new MonsterCardModel("Blue Eyes White Dragon", 3000, 2500);

            MonsterCardModel SliferTheSkyDragon = new MonsterCardModel("Slifer The Sky Dragon", 3500, 3500);

            MonsterCardModel ObeliskTheTormentor = new MonsterCardModel("Obelisk The Tormentor", 4000, 4000);

            MonsterCardModel TheWingedDragonOfRa = new MonsterCardModel("The Winged Dragon Of Ra", 5000, 5000);

            MonsterCardModel DarkMagicianGirl = new MonsterCardModel("Dark Magician Girl", 2000, 1700);

            MagicCardModel PotOfGreed = new PotOfGreed();

            MagicCardModel Ookazi = new Ookazi();

            Deck.Add(LusterDragon);
            Deck.Add(GeminiElf);
            Deck.Add(VorseRaider);
            Deck.Add(DarkMagician);
            Deck.Add(RedEyesBlackDragon);
            Deck.Add(BlueEyesWhiteDragon);
            Deck.Add(SliferTheSkyDragon);
            Deck.Add(ObeliskTheTormentor);
            Deck.Add(TheWingedDragonOfRa);
            Deck.Add(DarkMagicianGirl);
            Deck.Add(PotOfGreed);
            Deck.Add(Ookazi);
        }
    }
}
