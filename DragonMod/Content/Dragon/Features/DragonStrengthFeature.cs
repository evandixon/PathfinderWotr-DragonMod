using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using static DragonMod.Main;

namespace DragonMod.Content.Dragon.Features
{
    public static class DragonStrengthFeature
    {
        public static BlueprintFeature Add()
        {
            var dragonStrength = Helpers.CreateBlueprint<BlueprintFeature>(DragonModContext, "DragonStrengthFeature", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(DragonModContext, $"DragonStrength.Name", "Dragon Strength");
                bp.m_Description = Helpers.CreateString(DragonModContext, $"DragonStrength.Description", "Dragon Strength");
                bp.m_DescriptionShort = Helpers.CreateString(DragonModContext, $"DragonStrength.DescriptionShort", "Dragon Strength");
                bp.Ranks = 6;
                bp.AddComponent<AddStatBonus>(c =>
                {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Strength;
                    c.Value = 2;
                });
            });
            return dragonStrength;
        }

        public static T GetReference<T>() where T : BlueprintReferenceBase
        {
            return BlueprintTools.GetModBlueprintReference<T>(Main.DragonModContext, "DragonStrengthFeature");
        }
    }
}
