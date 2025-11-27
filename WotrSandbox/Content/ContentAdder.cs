using HarmonyLib;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Config;
using TabletopTweaks.Core.Utilities;
using UnityEngine.UIElements;
using WotrSandbox.Config;
using WotrSandbox.Content.Buffs;
using WotrSandbox.Content.Dragon;
using WotrSandbox.Content.Dragon.Bloodlines;
using WotrSandbox.Content.Dragon.Heritages;
using static WotrSandbox.Main;

namespace WotrSandbox.Content
{
    class ContentAdder
    {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch
        {
            private static bool Initialized;

            [HarmonyPriority(Priority.First)]
            [HarmonyPostfix]
            public static void CreateNewBlueprints()
            {
                if (Initialized) return;
                Initialized = true;

                LegendXpTable.Patch();

                AddIsekaiProtagonistClass();
            }

            public static void AddIsekaiProtagonistClass()
            {
                new DragonBloodlineGold().Add();

                DragonClass.Add();
                DragonNaturalWeapons.Add();
                DragonProgression.Add();
                KitsuneHalfDragonHeritage.Add();
            }

        }
    }


    [HarmonyPatch]
    static class FinalPatcher
    {
        private static bool Run = false;

        [HarmonyPriority(-100)]
        [HarmonyPatch(typeof(StartGameLoader), nameof(StartGameLoader.LoadAllJson))]
        [HarmonyPostfix]
        static void Postfix()
        {
            if (Run) return;
            Run = true;
        }
    }
}
