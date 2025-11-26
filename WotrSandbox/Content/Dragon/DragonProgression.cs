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
                bp.Ranks = 1;
                bp.AddComponent<AddStatBonus>(c =>
                {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Intelligence;
                    c.Value = 2;
                });
            });
            var dragonCharisma = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, "DragonCharismaFeature", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(IsekaiContext, $"DragonCharisma.Name", "Dragon Charisma");
                bp.m_Description = Helpers.CreateString(IsekaiContext, $"DragonCharisma.Description", "Dragon Charisma");
                bp.m_DescriptionShort = Helpers.CreateString(IsekaiContext, $"DragonCharisma.DescriptionShort", "Dragon Charisma");
                bp.Ranks = 1;
                bp.AddComponent<AddStatBonus>(c =>
                {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Charisma;
                    c.Value = 2;
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

            var bloodlineSelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>(IsekaiContext, "DragonBloodlineSelection", bp =>
            {
                bp.m_AllFeatures = new BlueprintFeatureReference[]
                {
                    BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(IsekaiContext, "DragonBloodlineGold")
                };
            });

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
            dragonProgression.LevelEntries = new LevelEntry[20] {
                /* Level 1 — “Your blood wakes.” (Light transformation, first powers)
                        Give the smallest portion of stats and the “starter” features.
                        Level 1 package:
                        +2 STR
                        +2 CON
                        +2 CHA (this is small enough to give early)
                        Natural Armor +1 (optional, fits the vibe)
                        Breath Weapon (Minor)
                        1/day or 1/1d4 rounds
                        1d6 per 2 class levels
                        Scent (dragons often have it)

                        This level says:
                        “You are becoming draconic, but not fully transformed.”

                        It parallels lvl 1 of Dragon Disciple without being too explosive. */
                Helpers.CreateLevelEntry(1, dragonStrength, dragonConstitution, dragonCharisma, dragonNaturalArmor, bloodlineSelection),

                /*
⭐ Level 2 — “Your body changes.” (Physical metamorphosis)
This is where you pour in the big stat increases.
Level 2 package:
+4 STR (bringing total to +6 STR so far)
+2 CON (bringing total to +4 CON so far)
+2 INT
Natural Armor +2 (total +3 so far)

This level is the “bulk” of the template power. 
*/
                Helpers.CreateLevelEntry(2, dragonStrength, dragonConstitution, dragonIntelligence, dragonNaturalArmor /*,goldDragonBreath*/),
/* ⭐ Level 3 — “You are Half-Dragon.” (Capstone transformation)
This is where the final stats and the big visual marker (wings) come in.
Level 3 package:
+2 STR (total +8)
+2 CON (total +6)
Natural Armor +1 (bringing to +4 total)
Immunity to your dragon’s element (fire/cold/electric/acid)
Wings
40–60 ft fly speed
Average maneuverability
Breath Weapon (Full)
1/1d4 rounds
Scaling area (40-ft cone / 80-ft line)
Damage = 1d6 * class level
Blindsense 30 ft
This level “locks in” the complete Half-Dragon template.*/
                Helpers.CreateLevelEntry(3, dragonStrength, dragonConstitution, dragonNaturalArmor),
                Helpers.CreateLevelEntry(4, dragonStrength, dragonNaturalArmor),
                Helpers.CreateLevelEntry(5, generalFeatSelection),
                Helpers.CreateLevelEntry(6, generalFeatSelection),
                Helpers.CreateLevelEntry(7, generalFeatSelection),
                Helpers.CreateLevelEntry(8, generalFeatSelection),
                Helpers.CreateLevelEntry(9, generalFeatSelection),
                Helpers.CreateLevelEntry(10, generalFeatSelection),
                Helpers.CreateLevelEntry(11, generalFeatSelection),
                Helpers.CreateLevelEntry(12, generalFeatSelection),
                Helpers.CreateLevelEntry(13, generalFeatSelection),
                Helpers.CreateLevelEntry(14, generalFeatSelection),
                Helpers.CreateLevelEntry(15, generalFeatSelection),
                Helpers.CreateLevelEntry(16, generalFeatSelection),
                Helpers.CreateLevelEntry(17, generalFeatSelection),
                Helpers.CreateLevelEntry(18, generalFeatSelection),
                Helpers.CreateLevelEntry(19, generalFeatSelection),
                Helpers.CreateLevelEntry(20, generalFeatSelection),
            };
            dragonProgression.UIGroups = new UIGroup[] {
                
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
                //IsekaiCantrips.ToReference<BlueprintFeatureBaseReference>(),
                //StartingWeaponSelection.ToReference<BlueprintFeatureBaseReference>(),
                //IsekaiPetSelection.ToReference<BlueprintFeatureBaseReference>(),
                //IsekaiProficiencies.ToReference<BlueprintFeatureBaseReference>(),
                //EdgeLordProficiencies.ToReference<BlueprintFeatureBaseReference>(),
                //GodEmperorProficiencies.ToReference<BlueprintFeatureBaseReference>(),
                //HeroProficiencies.ToReference<BlueprintFeatureBaseReference>(),
                //MastermindProficiencies.ToReference<BlueprintFeatureBaseReference>(),
                //OverlordProficiencies.ToReference<BlueprintFeatureBaseReference>(),
                //ArcanistArcaneReservoirFeature.ToReference<BlueprintFeatureBaseReference>(),
            };
            DragonClass.SetProgression(dragonProgression);
        }
    }
}
