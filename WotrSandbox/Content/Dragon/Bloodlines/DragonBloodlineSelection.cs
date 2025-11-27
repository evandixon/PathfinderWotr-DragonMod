using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using static WotrSandbox.Main;

namespace WotrSandbox.Content.Dragon.Bloodlines
{
    public static class DragonBloodlineSelection
    {
        public static void Add()
        {
            var bloodlineSelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>(IsekaiContext, "DragonBloodlineSelection", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(IsekaiContext, $"DragonBloodlineSelection.Name", "Dragon Type");
                bp.m_Description = Helpers.CreateString(IsekaiContext, $"DragonBloodlineSelection.Description", "There are many kinds of dragons in the world.");
                bp.m_AllFeatures = new BlueprintFeatureReference[]
                {
                    BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(IsekaiContext, "DragonBloodlineGold"),
                    BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(IsekaiContext, "DragonBloodlineSilver"),
                };
            });
        }

        public static BlueprintFeatureReference GetReference()
        {
            return BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(IsekaiContext, "DragonBloodlineSelection");
        }
        public static T GetReference<T>() where T : BlueprintReferenceBase
        {
            return BlueprintTools.GetModBlueprintReference<T>(IsekaiContext, "DragonBloodlineSelection");
        }
    }
}
