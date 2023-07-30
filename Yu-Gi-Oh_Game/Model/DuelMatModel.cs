using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using Yu_Gi_Oh_Game.Model.MagicCards;

namespace Yu_Gi_Oh_Game.Model
{
    public class DuelMatModel
    {
        public List<ICard> Cards { get; }
        public DuelMatModel()
        {
            Cards = new List<ICard>();
            CreateCards();
        }

        private void CreateCards()
        {
            MonsterCardModel LusterDragon = new MonsterCardModel("Luster Dragon", 1900, 1600);

            MonsterCardModel GeminiElf = new MonsterCardModel("Gemini Elf", 1900, 900);

            MonsterCardModel VorseRaider = new MonsterCardModel("Vorse Raider", 1900, 1200);

            MonsterCardModel DarkMagician = new MonsterCardModel("Dark Magician", 2500, 2100);

            MonsterCardModel RedEyesBlackDragon = new MonsterCardModel("Red Eyes Black Dragon", 2400, 2000);

            MonsterCardModel BlueEyesWhiteDragon = new MonsterCardModel("Blue Eyes White Dragon", 3000, 2500);

            MonsterCardModel SliferTheSkyDragon = new MonsterCardModel("Slifer The Sky Dragon", 3500, 3500);

            //MonsterCardModel ObeliskTheTormentor = new MonsterCardModel("Obelisk The Tormentor", 4000, 4000);

            //MonsterCardModel TheWingedDragonOfRa = new MonsterCardModel("The Winged Dragon Of Ra", 5000, 5000);

            //MonsterCardModel DarkMagicianGirl = new MonsterCardModel("Dark Magician Girl", 2000, 1700);

            MagicCardModel PotOfGreed = new PotOfGreed();

            Cards.Add(LusterDragon);
            Cards.Add(GeminiElf);
            Cards.Add(VorseRaider);
            Cards.Add(DarkMagician);
            Cards.Add(RedEyesBlackDragon);
            Cards.Add(BlueEyesWhiteDragon);
            Cards.Add(SliferTheSkyDragon);
            //Cards.Add(ObeliskTheTormentor);
            //Cards.Add(TheWingedDragonOfRa);
            //Cards.Add(DarkMagicianGirl);
            Cards.Add(PotOfGreed);
        }
    }
}
