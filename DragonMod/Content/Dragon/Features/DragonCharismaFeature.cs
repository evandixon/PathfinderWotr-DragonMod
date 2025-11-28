using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using static DragonMod.Main;

namespace DragonMod.Content.Dragon.Features
{
    public static class DragonCharismaFeature
    {
        public static BlueprintFeature Add()
        {
            var dragonStrength = Helpers.CreateBlueprint<BlueprintFeature>(DragonModContext, "DragonCharismaFeature", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(DragonModContext, $"DragonCharisma.Name", "Dragon Charisma");
                bp.m_Description = Helpers.CreateString(DragonModContext, $"DragonCharisma.Description", "Dragon Charisma");
                bp.m_DescriptionShort = Helpers.CreateString(DragonModContext, $"DragonCharisma.DescriptionShort", "Dragon Charisma");
                bp.Ranks = 6;
                bp.AddComponent<AddStatBonus>(c =>
                {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Charisma;
                    c.Value = 2;
                });
            });
            return dragonStrength;
        }

        public static T GetReference<T>() where T : BlueprintReferenceBase
        {
            return BlueprintTools.GetModBlueprintReference<T>(Main.DragonModContext, "DragonCharismaFeature");
        }
    }
}
