using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using DragonMod.Content.Dragon;
using DragonMod.Content.Dragon.Bloodlines;
using DragonMod.Content.Dragon.Buffs;
using DragonMod.Content.Dragon.Features;

namespace DragonMod.Content
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
                DragonStrengthFeature.Add();
                DragonDexterityFeature.Add();
                DragonConstitutionFeature.Add();
                DragonIntelligenceFeature.Add();
                DragonWisdomFeature.Add();
                DragonCharismaFeature.Add();
                DragonNaturalArmorFeature.Add();

                DragonBloodlineGold.Instance.Add();
                DragonBloodlineSilver.Instance.Add();
                DragonBloodlineSelection.Add();

                HalfDragonFeature.Add();

                DragonClass.Add();
                DragonNaturalWeapons.Add();
                DragonProgression.Add();
                //KitsuneHalfDragonHeritage.Add();
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
