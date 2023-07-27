using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;

namespace Yu_Gi_Oh_Game.Model
{
    public class DuelMatModel
    {
        public List<CardModel> Cards { get; }
        public DuelMatModel()
        {
            Cards = new List<CardModel>();
            CreateCards();
        }

        private void CreateCards()
        {
            CardModel LusterDragon = new CardModel("Luster Dragon", 1900, 1600);

            CardModel GeminiElf = new CardModel("Gemini Elf", 1900, 900);

            CardModel VorseRaider = new CardModel("Vorse Raider", 1900, 1200);

            CardModel DarkMagician = new CardModel("Dark Magician", 2500, 2100);

            CardModel RedEyesBlackDragon = new CardModel("Red Eyes Black Dragon", 2400, 2000);

            CardModel BlueEyesWhiteDragon = new CardModel("Blue Eyes White Dragon", 3000, 2500);

            CardModel SliferTheSkyDragon = new CardModel("Slifer The Sky Dragon", 3500, 3500);

            CardModel ObeliskTheTormentor = new CardModel("Obelisk The Tormentor", 4000, 4000);

            CardModel TheWingedDragonOfRa = new CardModel("The Winged Dragon Of Ra", 5000, 5000);

            CardModel DarkMagicianGirl = new CardModel("Dark Magician Girl", 2000, 1700);

            Cards.Add(LusterDragon);
            Cards.Add(GeminiElf);
            Cards.Add(VorseRaider);
            Cards.Add(DarkMagician);
            Cards.Add(RedEyesBlackDragon);
            Cards.Add(BlueEyesWhiteDragon);
            Cards.Add(SliferTheSkyDragon);
            Cards.Add(ObeliskTheTormentor);
            Cards.Add(TheWingedDragonOfRa);
            Cards.Add(DarkMagicianGirl);

        }
    }
}
