using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yu_Gi_Oh_Game.Model.MagicCards;
using Yu_Gi_Oh_Game.Model.MonsterCards;

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
