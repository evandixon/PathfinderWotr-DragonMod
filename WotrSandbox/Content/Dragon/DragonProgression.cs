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
                bp.Ranks = 10;
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
                bp.Ranks = 10;
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
                bp.m_Description = Helpers.CreateString(IsekaiContext, $"DragonLegendaryHeroFeature.Description", "It can take many centuries for a normal dragon to reach full power. Luckily you're not ordinary. You gain class levels at twice the rate of most mortals.");
                bp.m_DescriptionShort = Helpers.CreateString(IsekaiContext, $"DragonLegendaryHeroFeature.DescriptionShort", "It can take many centuries for a normal dragon to reach full power. Luckily you're not ordinary. You gain class levels at twice the rate of most mortals.");
                bp.AddComponent<AddMechanicsFeature>(c =>
                {
                    c.m_Feature = AddMechanicsFeature.MechanicsFeatureType.LegendaryHero;
                });
            });

            var sorcererCantripsFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("c58b36ec3f759c84089c67611d1bcc21"); // Sorcerer Cantrips Feature

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
            });

            dragonProgression.LevelEntries = new LevelEntry[30] {
                Helpers.CreateLevelEntry(1,sorcererCantripsFeature, legendaryHeroFeature, dragonStrength, dragonConstitution, dragonCharisma, dragonNaturalArmor, DragonBloodlineSelection.GetReference()),
                Helpers.CreateLevelEntry(2, dragonStrength, dragonConstitution, dragonIntelligence, dragonNaturalArmor),
                Helpers.CreateLevelEntry(3, dragonStrength, dragonConstitution, dragonCharisma, dragonNaturalArmor),
                Helpers.CreateLevelEntry(4, dragonStrength, dragonNaturalArmor, dragonIntelligence),
                Helpers.CreateLevelEntry(5, dragonCharisma),
                Helpers.CreateLevelEntry(6, dragonIntelligence),
                Helpers.CreateLevelEntry(7, dragonCharisma),
                Helpers.CreateLevelEntry(8, dragonIntelligence),
                Helpers.CreateLevelEntry(9, dragonCharisma),
                Helpers.CreateLevelEntry(10, dragonIntelligence),
                Helpers.CreateLevelEntry(11, dragonCharisma),
                Helpers.CreateLevelEntry(12, dragonIntelligence),
                Helpers.CreateLevelEntry(13, dragonCharisma),
                Helpers.CreateLevelEntry(14, dragonIntelligence),
                Helpers.CreateLevelEntry(15, dragonCharisma),
                Helpers.CreateLevelEntry(16, dragonIntelligence),
                Helpers.CreateLevelEntry(17, dragonCharisma),
                Helpers.CreateLevelEntry(18, dragonIntelligence),
                Helpers.CreateLevelEntry(19, dragonCharisma),
                Helpers.CreateLevelEntry(20, dragonIntelligence),
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
            dragonProgression.UIGroups = new UIGroup[] 
            {
                Helpers.CreateUIGroup(legendaryHeroFeature, dragonStrength, dragonConstitution, dragonIntelligence, dragonCharisma, dragonNaturalArmor),

                //// Isekai UI group
                //Helpers.CreateUIGroup(PlotArmor, IsekaiFighterTraining, SignatureAbility, SignatureMoveSelection,
                //    SummonHaremFeature, IsekaiAuraSelection, GodEmperorAuraSelection, DarkAuraFeature, HeroAuraSelection, Afterimage,
                //    IsekaiQuickFooted, GodEmperorQuickFooted, MastermindQuickFooted, BeachEpisodeSelection, OtherworldlyStamina, HaxSelection,
                //    ChuunibyouActualisationFeature, DeusExMachinaFeature, MasterplanFeature, SecondPhaseFeature),
                //Helpers.CreateUIGroup(ReleaseEnergy, Gifted, SignatureMoveBonusSelection, SecretPowerSelection, BeachEpisodeBonusSelection,
                //    SecondReincarnation),
                
                //// Edge Lord UI group
                //Helpers.CreateUIGroup(SupersonicCombat, ExtraStrike, ExtraSpecialPowerSelection),
                
                //// God Emperor UI group
                //Helpers.CreateUIGroup(NascentApotheosis, LightEnergyCondensation, GodEmperorEnergySelection, BodyMindAlterSelection,
                //    EnergyCondensationSelection, BarrierSelection, PathSelection, RealmSelection, GodlyVessel, Godhood),
                
                //// Hero UI group
                //Helpers.CreateUIGroup(GracefulCombat, IsekaiChannelPositiveEnergyFeature, HandsOfSalvation, GoldBarrierFeature,
                //    GoldBarrierHeroism, GoldBarrierFastHealing, GoldBarrierResistance),
                
                //// Mastermind UI group
                //Helpers.CreateUIGroup(AutoMetamagicSelectionMastermind, ArcanistExploitSelection),
                //Helpers.CreateUIGroup(MastermindConsumeSpells, EldritchFontEldritchSurge, EldritchFontImprovedSurge, EldritchFontGreaterSurge),

                //// Overlord UI group
                //Helpers.CreateUIGroup(OverpoweredAbilitySelectionOverlord, IsekaiChannelNegativeEnergyFeature, CorruptAuraFeature, SiphoningAuraFeature),
                
                //// OP ability and Special Power UI group
                //Helpers.CreateUIGroup(OverpoweredAbilitySelection, SpecialPowerSelection, ArmorSaint),

                //// Legacy UI groups
                //Helpers.CreateUIGroup(LegacySelection.GetClassFeature()),
                //Helpers.CreateUIGroup(HeroLegacySelection.getClassFeature()),
                //Helpers.CreateUIGroup(MastermindLegacySelection.getClassFeature()),
                //Helpers.CreateUIGroup(OverlordLegacySelection.getClassFeature()),
                //Helpers.CreateUIGroup(EdgeLordLegacySelection.getClassFeature()),
            };
            dragonProgression.m_UIDeterminatorsGroup = new BlueprintFeatureBaseReference[] {
                DragonBloodlineSelection.GetReference<BlueprintFeatureBaseReference>()
            };
        }
        public static BlueprintProgressionReference GetReference()
        {
            return BlueprintTools.GetModBlueprintReference<BlueprintProgressionReference>(IsekaiContext, "DragonProgression");
        }
    }
}
