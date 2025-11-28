using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using static DragonMod.Main;

namespace DragonMod.Content.Dragon.Features
{
    public static class DragonDexterityFeature
    {
        public static BlueprintFeature Add()
        {
            var dragonStrength = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, "DragonDexterity", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(IsekaiContext, $"DragonDexterity.Name", "Dragon Dexterity");
                bp.m_Description = Helpers.CreateString(IsekaiContext, $"DragonDexterity.Description", "Dragon Dexterity");
                bp.m_DescriptionShort = Helpers.CreateString(IsekaiContext, $"DragonDexterity.DescriptionShort", "Dragon Dexterity");
                bp.Ranks = 2;
                bp.AddComponent<AddStatBonus>(c =>
                {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Dexterity;
                    c.Value = -2;
                });
            });
            return dragonStrength;
        }

        public static T GetReference<T>() where T : BlueprintReferenceBase
        {
            return BlueprintTools.GetModBlueprintReference<T>(Main.IsekaiContext, "DragonDexterity");
        }
    }
}
