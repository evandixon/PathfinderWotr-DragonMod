using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using static DragonMod.Main;

namespace DragonMod.Content.Dragon.Features
{
    public static class DragonWisdomFeature
    {
        public static BlueprintFeature Add()
        {
            var dragonStrength = Helpers.CreateBlueprint<BlueprintFeature>(DragonModContext, "DragonWisdomFeature", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(DragonModContext, $"DragonWisdom.Name", "Dragon Wisdom");
                bp.m_Description = Helpers.CreateString(DragonModContext, $"DragonWisdom.Description", "Dragon Wisdom");
                bp.m_DescriptionShort = Helpers.CreateString(DragonModContext, $"DragonWisdom.DescriptionShort", "Dragon Wisdom");
                bp.Ranks = 6;
                bp.AddComponent<AddStatBonus>(c =>
                {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Wisdom;
                    c.Value = 2;
                });
            });
            return dragonStrength;
        }

        public static T GetReference<T>() where T : BlueprintReferenceBase
        {
            return BlueprintTools.GetModBlueprintReference<T>(Main.DragonModContext, "DragonWisdomFeature");
        }
    }
}
