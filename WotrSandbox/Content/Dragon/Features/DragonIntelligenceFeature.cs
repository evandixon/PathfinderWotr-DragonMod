using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using static WotrSandbox.Main;

namespace WotrSandbox.Content.Dragon.Features
{
    public static class DragonIntelligenceFeature
    {
        public static BlueprintFeature Add()
        {
            var dragonStrength = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, "DragonIntelligenceFeature", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(IsekaiContext, $"DragonIntelligence.Name", "Dragon Intelligence");
                bp.m_Description = Helpers.CreateString(IsekaiContext, $"DragonIntelligence.Description", "Dragon Intelligence");
                bp.m_DescriptionShort = Helpers.CreateString(IsekaiContext, $"DragonIntelligence.DescriptionShort", "Dragon Intelligence");
                bp.Ranks = 6;
                bp.AddComponent<AddStatBonus>(c =>
                {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Intelligence;
                    c.Value = 2;
                });
            });
            return dragonStrength;
        }

        public static T GetReference<T>() where T : BlueprintReferenceBase
        {
            return BlueprintTools.GetModBlueprintReference<T>(Main.IsekaiContext, "DragonIntelligenceFeature");
        }
    }
}
