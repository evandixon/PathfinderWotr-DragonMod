using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using static DragonMod.Main;

namespace DragonMod.Content.Dragon.Features
{
    public static class DragonNaturalArmorFeature
    {
        public static BlueprintFeature Add()
        {
            var dragonStrength = Helpers.CreateBlueprint<BlueprintFeature>(DragonModContext, "DragonNaturalArmorFeature", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(DragonModContext, $"DragonNaturalArmorFeature.Name", "Dragon Natural Armor");
                bp.m_Description = Helpers.CreateString(DragonModContext, $"DragonNaturalArmorFeature.Description", "Dragon Natural Armor");
                bp.m_DescriptionShort = Helpers.CreateString(DragonModContext, $"DragonNaturalArmorFeature.DescriptionShort", "Dragon Natural Armor");
                bp.Ranks = 11;
                bp.AddComponent<AddStatBonus>(c =>
                {
                    c.Descriptor = ModifierDescriptor.NaturalArmor;
                    c.Stat = StatType.AC;
                    c.Value = 3;
                });
            });
            return dragonStrength;
        }

        public static T GetReference<T>() where T : BlueprintReferenceBase
        {
            return BlueprintTools.GetModBlueprintReference<T>(Main.DragonModContext, "DragonNaturalArmorFeature");
        }
    }
}
