using DragonMod.Content.Dragon.Features;
using DragonMod.Infrastructure;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.Utilities;
using static DragonMod.Main;

namespace DragonMod.Content.Dragon
{
    public static class DragonProgression
    {
        public static void Add()
        {
            var generalFeatSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("247a4068296e8be42890143f451b4b45");

            var simpleWeaponProficiency = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("e70ecf1ed95ca2f40b754f1adb22bbdd");
            var halfDragonFeature = HalfDragonFeature.GetReference<BlueprintFeatureReference>();

            var dragonProgression = Helpers.CreateBlueprint<BlueprintProgression>(DragonModContext, "DragonProgression", bp => {
                bp.SetName(StaticReferences.Strings.Null);
                bp.SetDescription(DragonModContext, "Dragon");
                bp.m_AllowNonContextActions = false;
                bp.IsClassFeature = true;
                bp.m_FeaturesRankIncrease = null;
                bp.m_Classes = new BlueprintProgression.ClassWithLevel[] {
                    new BlueprintProgression.ClassWithLevel {
                        m_Class = DragonClass.GetReference(),
                        AdditionalLevel = 0
                    }
                };
                bp.LevelEntries = new LevelEntry[1] {
                    Helpers.CreateLevelEntry(1,
                        simpleWeaponProficiency,
                        halfDragonFeature),
                    //Helpers.CreateLevelEntry(2, new BlueprintFeatureBase[0]),
                    //Helpers.CreateLevelEntry(3, new BlueprintFeatureBase[0]),
                    //Helpers.CreateLevelEntry(4, new BlueprintFeatureBase[0]),
                    //Helpers.CreateLevelEntry(5, new BlueprintFeatureBase[0]),
                    //Helpers.CreateLevelEntry(6, new BlueprintFeatureBase[0]),
                    //Helpers.CreateLevelEntry(7, new BlueprintFeatureBase[0]),
                    //Helpers.CreateLevelEntry(8, new BlueprintFeatureBase[0]),
                    //Helpers.CreateLevelEntry(9, new BlueprintFeatureBase[0]),
                    //Helpers.CreateLevelEntry(10, new BlueprintFeatureBase[0]),
                    //Helpers.CreateLevelEntry(11, new BlueprintFeatureBase[0]),
                    //Helpers.CreateLevelEntry(12, new BlueprintFeatureBase[0]),
                    //Helpers.CreateLevelEntry(13, new BlueprintFeatureBase[0]),
                    //Helpers.CreateLevelEntry(14, new BlueprintFeatureBase[0]),
                    //Helpers.CreateLevelEntry(15, new BlueprintFeatureBase[0]),
                    //Helpers.CreateLevelEntry(16, new BlueprintFeatureBase[0]),
                    //Helpers.CreateLevelEntry(17, new BlueprintFeatureBase[0]),
                    //Helpers.CreateLevelEntry(18, new BlueprintFeatureBase[0]),
                    //Helpers.CreateLevelEntry(19, new BlueprintFeatureBase[0]),
                    //Helpers.CreateLevelEntry(20, new BlueprintFeatureBase[0]),
                    //Helpers.CreateLevelEntry(21, new BlueprintFeatureBase[0]),
                    //Helpers.CreateLevelEntry(22, new BlueprintFeatureBase[0]),
                    //Helpers.CreateLevelEntry(23, new BlueprintFeatureBase[0]),
                    //Helpers.CreateLevelEntry(24, new BlueprintFeatureBase[0]),
                    //Helpers.CreateLevelEntry(25, new BlueprintFeatureBase[0]),
                    //Helpers.CreateLevelEntry(26, new BlueprintFeatureBase[0]),
                    //Helpers.CreateLevelEntry(27, new BlueprintFeatureBase[0]),
                    //Helpers.CreateLevelEntry(28, new BlueprintFeatureBase[0]),
                    //Helpers.CreateLevelEntry(29, new BlueprintFeatureBase[0]),
                    //Helpers.CreateLevelEntry(30, new BlueprintFeatureBase[0]),
                };

                bp.UIGroups = new UIGroup[]
                {
                    Helpers.CreateUIGroup(halfDragonFeature),
                };
                bp.m_UIDeterminatorsGroup = new BlueprintFeatureBaseReference[] {
                };
            });
        }

        public static BlueprintProgressionReference GetReference()
        {
            return BlueprintTools.GetModBlueprintReference<BlueprintProgressionReference>(DragonModContext, "DragonProgression");
        }
    }
}
