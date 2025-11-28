using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using static DragonMod.Main;

namespace DragonMod.Content.Dragon.Features
{
    public static class DragonConstitutionFeature
    {
        public static BlueprintFeature Add()
        {
            var dragonStrength = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, "DragonConstitutionFeature", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(IsekaiContext, $"DragonConstitution.Name", "Dragon Constitution");
                bp.m_Description = Helpers.CreateString(IsekaiContext, $"DragonConstitution.Description", "Dragon Constitution");
                bp.m_DescriptionShort = Helpers.CreateString(IsekaiContext, $"DragonConstitution.DescriptionShort", "Dragon Constitution");
                bp.Ranks = 3;
                bp.AddComponent<AddStatBonus>(c =>
                {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Constitution;
                    c.Value = 2;
                });
            });
            return dragonStrength;
        }

        public static T GetReference<T>() where T : BlueprintReferenceBase
        {
            return BlueprintTools.GetModBlueprintReference<T>(Main.IsekaiContext, "DragonConstitutionFeature");
        }
    }
}
