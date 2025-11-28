using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.Utilities;
using static DragonMod.Main;

namespace DragonMod.Content.Dragon.Heritages {

    internal class HumanHeritageSelection {
        private static BlueprintFeature[] ourHeritages;
        private static BlueprintFeature dummyBasicFeat;
        private static BlueprintFeature dummyNoFeat;
        private static BlueprintFeatureSelection ourHeritageSelection;

        public static void CreateDummy() {
            ourHeritages = new BlueprintFeature[] { };

            ourHeritageSelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>(DragonModContext, "IsekaiHumanHeritageSelection", bp => {
                bp.SetName(DragonModContext, "Alternate Racial Traits");
                bp.SetDescription(DragonModContext, "The following alternate traits are available.");
                bp.IsClassFeature = true;
                bp.Groups = new[] { FeatureGroup.Racial };
                bp.Group = FeatureGroup.KitsuneHeritage;
                bp.m_Features = new BlueprintFeatureReference[] { };
                bp.m_AllFeatures = new BlueprintFeatureReference[] { };
            });
            dummyBasicFeat = Helpers.CreateBlueprint<BlueprintFeature>(DragonModContext, "IsekaiHumanHeritageDummyFeat", bp => {
                bp.SetName(DragonModContext, FeatTools.Selections.BasicFeatSelection.m_DisplayName);
                bp.SetDescription(DragonModContext, FeatTools.Selections.BasicFeatSelection.m_Description);
                bp.m_Icon = FeatTools.Selections.BasicFeatSelection.m_Icon;
                bp.IsClassFeature = true;
            });
            dummyNoFeat = Helpers.CreateBlueprint<BlueprintFeature>(DragonModContext, "IsekaiHumanHeritageNoFeat", bp => {
                bp.SetName(DragonModContext, "None");
                bp.SetDescription(DragonModContext, "No alternate trait");
                bp.m_Icon = FeatTools.Selections.BasicFeatSelection.m_Icon;
                bp.IsClassFeature = true;
            });
        }

        public static void Register(BlueprintFeature feature) {
            ourHeritages = ourHeritages.AppendToArray(feature);
        }

        public static void Patch() {
            BlueprintRace human = BlueprintTools.GetBlueprint<BlueprintRace>("0a5d473ead98b0646b94495af250fdc4");
            BlueprintFeature skilled = BlueprintTools.GetBlueprint<BlueprintFeature>("3adf9274a210b164cb68f472dc1e4544");
            BlueprintFeatureSelection basicFeat = FeatTools.Selections.BasicFeatSelection;
            BlueprintFeatureSelection candidate = null;

            foreach (var toTest in human.m_Features) {
                if (toTest != null && toTest.Get() is BlueprintFeatureSelection selection && !selection.Equals(basicFeat)) {
                    candidate = selection;
                }
            }

            if (candidate != null) {
                DragonModContext.Logger.Log("found a selection added by another mod, adding onto that rather than creating our own");
                foreach (var heritage in ourHeritages) {
                    candidate.AddFeatures(heritage);
                }
            } else {
                DragonModContext.Logger.Log("patching our own alternate human heritage feats into the game because no other source was present");
                human.m_Features = new BlueprintFeatureBaseReference[] {
                    dummyBasicFeat.ToReference<BlueprintFeatureBaseReference>(),
                    skilled.ToReference<BlueprintFeatureBaseReference>(),
                    ourHeritageSelection.ToReference<BlueprintFeatureBaseReference>(),
                };
                ourHeritageSelection.AddFeatures(basicFeat);
                ourHeritageSelection.AddFeatures(dummyNoFeat);
                foreach (var heritage in ourHeritages) {
                    ourHeritageSelection.AddFeatures(heritage);
                }
            }
        }
    }
}