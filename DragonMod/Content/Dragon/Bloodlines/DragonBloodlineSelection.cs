using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using static DragonMod.Main;

namespace DragonMod.Content.Dragon.Bloodlines
{
    public static class DragonBloodlineSelection
    {
        public static void Add()
        {
            var bloodlineSelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>(DragonModContext, "DragonBloodlineSelection", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(DragonModContext, $"DragonBloodlineSelection.Name", "Dragon Type");
                bp.m_Description = Helpers.CreateString(DragonModContext, $"DragonBloodlineSelection.Description", "There are many kinds of dragons in the world.");
                bp.m_AllFeatures = new BlueprintFeatureReference[]
                {
                    BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(DragonModContext, "DragonBloodlineGold"),
                    BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(DragonModContext, "DragonBloodlineSilver"),
                };
            });
        }

        public static BlueprintFeatureReference GetReference()
        {
            return BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(DragonModContext, "DragonBloodlineSelection");
        }
        public static T GetReference<T>() where T : BlueprintReferenceBase
        {
            return BlueprintTools.GetModBlueprintReference<T>(DragonModContext, "DragonBloodlineSelection");
        }
    }
}
