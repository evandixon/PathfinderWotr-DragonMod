using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace DragonMod.Content.Dragon.Features
{
    public static class DragonLegendaryHeroFeature
    {
        public static void Add()
        {
            var legendaryHeroFeature = Helpers.CreateBlueprint<BlueprintFeature>(Main.DragonModContext, "DragonLegendaryHeroFeature", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(Main.DragonModContext, $"DragonLegendaryHeroFeature.Name", "Legendary Dragon");
                bp.m_Description = Helpers.CreateString(Main.DragonModContext, $"DragonLegendaryHeroFeature.Description", "It can take many centuries for a normal dragon to reach full power. Luckily you're not ordinary. Your level cap is 40, and you gain levels at roughly twice the rate of mortals.");
                bp.m_DescriptionShort = Helpers.CreateString(Main.DragonModContext, $"DragonLegendaryHeroFeature.DescriptionShort", "It can take many centuries for a normal dragon to reach full power. Luckily you're not ordinary. Your level cap is 40, and you gain levels at roughly twice the rate of mortals.");
                bp.AddComponent<AddMechanicsFeature>(c =>
                {
                    c.m_Feature = AddMechanicsFeature.MechanicsFeatureType.LegendaryHero;
                });
            });
        }

        public static T GetReference<T>() where T : BlueprintReferenceBase
        {
            return BlueprintTools.GetModBlueprintReference<T>(Main.DragonModContext, "DragonLegendaryHeroFeature");
        }
}
}
