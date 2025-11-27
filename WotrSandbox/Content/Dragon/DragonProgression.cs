using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using WotrSandbox.Infrastructure;
using static WotrSandbox.Main;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Enums;
using Kingmaker.EntitySystem.Stats;
using WotrSandbox.Content.Dragon.Bloodlines;

namespace WotrSandbox.Content.Dragon
{
    public static class DragonProgression
    {
        public static void Add()
        {
            var generalFeatSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("247a4068296e8be42890143f451b4b45");

            var dragonStrength = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, "DragonStrengthFeature", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(IsekaiContext, $"DragonStrength.Name", "Dragon Strength");
                bp.m_Description = Helpers.CreateString(IsekaiContext, $"DragonStrength.Description", "Dragon Strength");
                bp.m_DescriptionShort = Helpers.CreateString(IsekaiContext, $"DragonStrength.DescriptionShort", "Dragon Strength");
                bp.Ranks = 4;
                bp.AddComponent<AddStatBonus>(c =>
                {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Strength;
                    c.Value = 2;
                });
            });
            var dragonConstitution = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, "DragonConstitutionFeature", bp =>
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
            var dragonIntelligence = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, "DragonIntelligenceFeature", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(IsekaiContext, $"DragonIntelligence.Name", "Dragon Intelligence");
                bp.m_Description = Helpers.CreateString(IsekaiContext, $"DragonIntelligence.Description", "Dragon Intelligence");
                bp.m_DescriptionShort = Helpers.CreateString(IsekaiContext, $"DragonIntelligence.DescriptionShort", "Dragon Intelligence");
                bp.Ranks = 4;
                bp.AddComponent<AddStatBonus>(c =>
                {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Intelligence;
                    c.Value = 1;
                });
            });
            var dragonCharisma = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, "DragonCharismaFeature", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(IsekaiContext, $"DragonCharisma.Name", "Dragon Charisma");
                bp.m_Description = Helpers.CreateString(IsekaiContext, $"DragonCharisma.Description", "Dragon Charisma");
                bp.m_DescriptionShort = Helpers.CreateString(IsekaiContext, $"DragonCharisma.DescriptionShort", "Dragon Charisma");
                bp.Ranks = 4;
                bp.AddComponent<AddStatBonus>(c =>
                {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Charisma;
                    c.Value = 1;
                });
            });
            var dragonNaturalArmor = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, "DragonNaturalArmorFeature", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(IsekaiContext, $"DragonNaturalArmor.Name", "Dragon Natural Armor");
                bp.m_Description = Helpers.CreateString(IsekaiContext, $"DragonNaturalArmor.Description", "Dragon Natural Armor");
                bp.m_DescriptionShort = Helpers.CreateString(IsekaiContext, $"DragonNaturalArmor.DescriptionShort", "Dragon Natural Armor");
                bp.Ranks = 4;
                bp.AddComponent<AddStatBonus>(c =>
                {
                    c.Descriptor = ModifierDescriptor.NaturalArmor;
                    c.Stat = StatType.AC;
                    c.Value = 1;
                });
            });

            var legendaryHeroFeature = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, "DragonLegendaryHeroFeature", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(IsekaiContext, $"DragonLegendaryHeroFeature.Name", "Legendary Dragon");
                bp.m_Description = Helpers.CreateString(IsekaiContext, $"DragonLegendaryHeroFeature.Description", "It can take many centuries for a normal dragon to reach full power. Luckily you're not ordinary. Your level cap is 40, and you gain levels at roughly twice the rate of mortals.");
                bp.m_DescriptionShort = Helpers.CreateString(IsekaiContext, $"DragonLegendaryHeroFeature.DescriptionShort", "It can take many centuries for a normal dragon to reach full power. Luckily you're not ordinary. Your level cap is 40, and you gain levels at roughly twice the rate of mortals.");
                bp.AddComponent<AddMechanicsFeature>(c =>
                {
                    c.m_Feature = AddMechanicsFeature.MechanicsFeatureType.LegendaryHero;
                });
            });

            var simpleWeaponProficiency = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("e70ecf1ed95ca2f40b754f1adb22bbdd");
            var halfDragonFeature = HalfDragonFeature.GetReference<BlueprintFeatureReference>();

            var dragonProgression = Helpers.CreateBlueprint<BlueprintProgression>(IsekaiContext, "DragonProgression", bp => {
                bp.SetName(StaticReferences.Strings.Null);
                bp.SetDescription(IsekaiContext, "Dragon");
                bp.m_AllowNonContextActions = false;
                bp.IsClassFeature = true;
                bp.m_FeaturesRankIncrease = null;
                bp.m_Classes = new BlueprintProgression.ClassWithLevel[] {
                    new BlueprintProgression.ClassWithLevel {
                        m_Class = DragonClass.GetReference(),
                        AdditionalLevel = 0
                    }
                };
                bp.LevelEntries = new LevelEntry[30] {
                    Helpers.CreateLevelEntry(1,
                        simpleWeaponProficiency,
                        halfDragonFeature,
                        legendaryHeroFeature,
                        DragonBloodlineSelection.GetReference()),
                    Helpers.CreateLevelEntry(2, new BlueprintFeatureBase[0]),
                    Helpers.CreateLevelEntry(3, new BlueprintFeatureBase[0]),
                    Helpers.CreateLevelEntry(4, new BlueprintFeatureBase[0]),
                    Helpers.CreateLevelEntry(5, new BlueprintFeatureBase[0]),
                    Helpers.CreateLevelEntry(6, new BlueprintFeatureBase[0]),
                    Helpers.CreateLevelEntry(7, new BlueprintFeatureBase[0]),
                    Helpers.CreateLevelEntry(8, new BlueprintFeatureBase[0]),
                    Helpers.CreateLevelEntry(9, new BlueprintFeatureBase[0]),
                    Helpers.CreateLevelEntry(10, new BlueprintFeatureBase[0]),
                    Helpers.CreateLevelEntry(11, new BlueprintFeatureBase[0]),
                    Helpers.CreateLevelEntry(12, new BlueprintFeatureBase[0]),
                    Helpers.CreateLevelEntry(13, new BlueprintFeatureBase[0]),
                    Helpers.CreateLevelEntry(14, new BlueprintFeatureBase[0]),
                    Helpers.CreateLevelEntry(15, new BlueprintFeatureBase[0]),
                    Helpers.CreateLevelEntry(16, new BlueprintFeatureBase[0]),
                    Helpers.CreateLevelEntry(17, new BlueprintFeatureBase[0]),
                    Helpers.CreateLevelEntry(18, new BlueprintFeatureBase[0]),
                    Helpers.CreateLevelEntry(19, new BlueprintFeatureBase[0]),
                    Helpers.CreateLevelEntry(20, new BlueprintFeatureBase[0]),
                    Helpers.CreateLevelEntry(21, new BlueprintFeatureBase[0]),
                    Helpers.CreateLevelEntry(22, new BlueprintFeatureBase[0]),
                    Helpers.CreateLevelEntry(23, new BlueprintFeatureBase[0]),
                    Helpers.CreateLevelEntry(24, new BlueprintFeatureBase[0]),
                    Helpers.CreateLevelEntry(25, new BlueprintFeatureBase[0]),
                    Helpers.CreateLevelEntry(26, new BlueprintFeatureBase[0]),
                    Helpers.CreateLevelEntry(27, new BlueprintFeatureBase[0]),
                    Helpers.CreateLevelEntry(28, new BlueprintFeatureBase[0]),
                    Helpers.CreateLevelEntry(29, new BlueprintFeatureBase[0]),
                    Helpers.CreateLevelEntry(30, new BlueprintFeatureBase[0]),
                };

                bp.UIGroups = new UIGroup[]
                {
                    Helpers.CreateUIGroup(legendaryHeroFeature),
                };
                bp.m_UIDeterminatorsGroup = new BlueprintFeatureBaseReference[] {
                    DragonBloodlineSelection.GetReference<BlueprintFeatureBaseReference>()
                };
            });
        }

        public static BlueprintProgressionReference GetReference()
        {
            return BlueprintTools.GetModBlueprintReference<BlueprintProgressionReference>(IsekaiContext, "DragonProgression");
        }
    }
}
