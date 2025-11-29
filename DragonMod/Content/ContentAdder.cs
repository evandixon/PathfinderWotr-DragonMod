using DragonMod.Content.Dragon;
using DragonMod.Content.Dragon.Bloodlines;
using DragonMod.Content.Dragon.Buffs;
using DragonMod.Content.Dragon.Features;
using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;

namespace DragonMod.Content
{
    public static class ContentAdder
    {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        public static class BlueprintsCache_Init_Patch
        {
            public static bool Initialized { get; private set; }

            [HarmonyPriority(Priority.First)]
            [HarmonyPostfix]
            public static void CreateNewBlueprints()
            {
                if (Initialized) return;
                Initialized = true;

                Main.Log("Creating dragon mod blueprints");

                XpTablePatcher.Patch();

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
                DragonLegendaryHeroFeature.Add();

                HalfDragonFeature.Add();

                DragonBloodlineGold.Instance.Add();
                DragonBloodlineSilver.Instance.Add();
                DragonBloodlineSelection.Add();

                DragonClass.Add();
                DragonNaturalWeapons.Add();
                DragonProgression.Add();
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
