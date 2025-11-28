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
            var dragonStrength = Helpers.CreateBlueprint<BlueprintFeature>(DragonModContext, "DragonDexterity", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(DragonModContext, $"DragonDexterity.Name", "Dragon Dexterity");
                bp.m_Description = Helpers.CreateString(DragonModContext, $"DragonDexterity.Description", "Dragon Dexterity");
                bp.m_DescriptionShort = Helpers.CreateString(DragonModContext, $"DragonDexterity.DescriptionShort", "Dragon Dexterity");
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
            return BlueprintTools.GetModBlueprintReference<T>(Main.DragonModContext, "DragonDexterity");
        }
    }
}
