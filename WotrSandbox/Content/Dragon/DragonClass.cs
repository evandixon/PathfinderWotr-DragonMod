using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Localization;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using WotrSandbox.Infrastructure;
using static TabletopTweaks.Core.Utilities.SpellTools;
using static WotrSandbox.Main;

namespace WotrSandbox.Content.Dragon
{
    internal static class DragonClass
    {
        private static readonly LocalizedString Name = Helpers.CreateString(IsekaiContext, $"DragonClass.Name", "Dragon");
        private static readonly LocalizedString Description = Helpers.CreateString(IsekaiContext, $"DragonClass.Description",
            "Dragons are considered to some to be the perfect being, boasting immense strength, durability, and magical powers.");
        private static readonly LocalizedString DescriptionShort = Helpers.CreateString(IsekaiContext, $"DragonClass.DescriptionShort",
            "Dragons are considered to some to be the perfect being, boasting immense strength, durability, and magical powers.");
        private static BlueprintCharacterClass dragonClass;

        // Stat Progression
        private static readonly BlueprintStatProgression BABFull = BlueprintTools.GetBlueprint<BlueprintStatProgression>("b3057560ffff3514299e8b93e7648a9d");
        private static readonly BlueprintStatProgression SavesHigh = BlueprintTools.GetBlueprint<BlueprintStatProgression>("ff4662bde9e75f145853417313842751");

        public static void Add()
        {
            var defaultClothesIndex = StaticReferences.BaseClasses.IndexOf(ClassTools.Classes.DragonDiscipleClass);
            var clothesIndex = IsekaiContext.AddedContent.IsekaiDefaultClothes;
            var maxClothesIndex = StaticReferences.BaseClasses.Length;
            var clothesClass = StaticReferences.BaseClasses[clothesIndex > -1 && clothesIndex < maxClothesIndex ? clothesIndex : defaultClothesIndex];

            var visualSettings = Helpers.CreateBlueprint<BlueprintClassAdditionalVisualSettings>(IsekaiContext, "DragonClassVisual1", bp =>
            {
                // ColorRamps
                bp.ColorRamps = new[]
                {
                    new BlueprintClassAdditionalVisualSettings.ColorRamp
                    {
                        m_Type = 34359738368,
                        m_Indices = BlueprintClassAdditionalVisualSettings.ColorRamp.IndexType.SpPrim,
                        m_Primary = 2,
                        m_Secondary = 0,
                        m_SpecialPrimary = 2,
                        m_SpecialSecondary = 0
                    },
                    new BlueprintClassAdditionalVisualSettings.ColorRamp
                    {
                        m_Type = 4,
                        m_Indices = BlueprintClassAdditionalVisualSettings.ColorRamp.IndexType.SpPrim,
                        m_Primary = 2,
                        m_Secondary = 0,
                        m_SpecialPrimary = 2,
                        m_SpecialSecondary = 0
                    },
                    new BlueprintClassAdditionalVisualSettings.ColorRamp
                    {
                        m_Type = 32768,
                        m_Indices = BlueprintClassAdditionalVisualSettings.ColorRamp.IndexType.SpPrim,
                        m_Primary = 2,
                        m_Secondary = 0,
                        m_SpecialPrimary = 2,
                        m_SpecialSecondary = 0
                    },
                    new BlueprintClassAdditionalVisualSettings.ColorRamp
                    {
                        m_Type = 256,
                        m_Indices = BlueprintClassAdditionalVisualSettings.ColorRamp.IndexType.SpPrim,
                        m_Primary = 2,
                        m_Secondary = 0,
                        m_SpecialPrimary = 2,
                        m_SpecialSecondary = 0
                    },
                    new BlueprintClassAdditionalVisualSettings.ColorRamp
                    {
                        m_Type = 512,
                        m_Indices = BlueprintClassAdditionalVisualSettings.ColorRamp.IndexType.SpPrim,
                        m_Primary = 2,
                        m_Secondary = 0,
                        m_SpecialPrimary = 2,
                        m_SpecialSecondary = 0
                    },
                    new BlueprintClassAdditionalVisualSettings.ColorRamp
                    {
                        m_Type = 1024,
                        m_Indices = BlueprintClassAdditionalVisualSettings.ColorRamp.IndexType.SpPrim,
                        m_Primary = 2,
                        m_Secondary = 0,
                        m_SpecialPrimary = 2,
                        m_SpecialSecondary = 0
                    },
                    new BlueprintClassAdditionalVisualSettings.ColorRamp
                    {
                        m_Type = 2048,
                        m_Indices = BlueprintClassAdditionalVisualSettings.ColorRamp.IndexType.SpPrim,
                        m_Primary = 2,
                        m_Secondary = 0,
                        m_SpecialPrimary = 2,
                        m_SpecialSecondary = 0
                    },
                    new BlueprintClassAdditionalVisualSettings.ColorRamp
                    {
                        m_Type = 65536,
                        m_Indices = BlueprintClassAdditionalVisualSettings.ColorRamp.IndexType.SpPrim,
                        m_Primary = 2,
                        m_Secondary = 0,
                        m_SpecialPrimary = 2,
                        m_SpecialSecondary = 0
                    },
                    new BlueprintClassAdditionalVisualSettings.ColorRamp
                    {
                        m_Type = 8388608,
                        m_Indices = BlueprintClassAdditionalVisualSettings.ColorRamp.IndexType.SpPrim,
                        m_Primary = 2,
                        m_Secondary = 0,
                        m_SpecialPrimary = 2,
                        m_SpecialSecondary = 0
                    },
                    new BlueprintClassAdditionalVisualSettings.ColorRamp
                    {
                        m_Type = 33554432,
                        m_Indices = BlueprintClassAdditionalVisualSettings.ColorRamp.IndexType.SpPrim,
                        m_Primary = 2,
                        m_Secondary = 0,
                        m_SpecialPrimary = 2,
                        m_SpecialSecondary = 0
                    },
                    new BlueprintClassAdditionalVisualSettings.ColorRamp
                    {
                        m_Type = 137438953472,
                        m_Indices = BlueprintClassAdditionalVisualSettings.ColorRamp.IndexType.SpPrim,
                        m_Primary = 2,
                        m_Secondary = 0,
                        m_SpecialPrimary = 2,
                        m_SpecialSecondary = 0
                    },
                    new BlueprintClassAdditionalVisualSettings.ColorRamp
                    {
                        m_Type = 67108864,
                        m_Indices = BlueprintClassAdditionalVisualSettings.ColorRamp.IndexType.SpPrim,
                        m_Primary = 2,
                        m_Secondary = 0,
                        m_SpecialPrimary = 2,
                        m_SpecialSecondary = 0
                    },
                    new BlueprintClassAdditionalVisualSettings.ColorRamp
                    {
                        m_Type = 134217728,
                        m_Indices = BlueprintClassAdditionalVisualSettings.ColorRamp.IndexType.SpPrim,
                        m_Primary = 2,
                        m_Secondary = 0,
                        m_SpecialPrimary = 2,
                        m_SpecialSecondary = 0
                    },
                    new BlueprintClassAdditionalVisualSettings.ColorRamp
                    {
                        m_Type = 268435456,
                        m_Indices = BlueprintClassAdditionalVisualSettings.ColorRamp.IndexType.SpPrim,
                        m_Primary = 2,
                        m_Secondary = 0,
                        m_SpecialPrimary = 2,
                        m_SpecialSecondary = 0
                    },
                    new BlueprintClassAdditionalVisualSettings.ColorRamp
                    {
                        m_Type = 536870912,
                        m_Indices = BlueprintClassAdditionalVisualSettings.ColorRamp.IndexType.SpPrim,
                        m_Primary = 2,
                        m_Secondary = 0,
                        m_SpecialPrimary = 2,
                        m_SpecialSecondary = 0
                    },
                    new BlueprintClassAdditionalVisualSettings.ColorRamp
                    {
                        m_Type = 1073741824,
                        m_Indices = BlueprintClassAdditionalVisualSettings.ColorRamp.IndexType.SpPrim,
                        m_Primary = 2,
                        m_Secondary = 0,
                        m_SpecialPrimary = 2,
                        m_SpecialSecondary = 0
                    },
                    new BlueprintClassAdditionalVisualSettings.ColorRamp
                    {
                        m_Type = 2147483648,
                        m_Indices = BlueprintClassAdditionalVisualSettings.ColorRamp.IndexType.SpPrim,
                        m_Primary = 2,
                        m_Secondary = 0,
                        m_SpecialPrimary = 2,
                        m_SpecialSecondary = 0
                    },
                    new BlueprintClassAdditionalVisualSettings.ColorRamp
                    {
                        m_Type = 17179869184,
                        m_Indices = BlueprintClassAdditionalVisualSettings.ColorRamp.IndexType.SpPrim,
                        m_Primary = 2,
                        m_Secondary = 0,
                        m_SpecialPrimary = 2,
                        m_SpecialSecondary = 0
                    },
                    new BlueprintClassAdditionalVisualSettings.ColorRamp
                    {
                        m_Type = 274877906944,
                        m_Indices = BlueprintClassAdditionalVisualSettings.ColorRamp.IndexType.SpPrim,
                        m_Primary = 2,
                        m_Secondary = 0,
                        m_SpecialPrimary = 2,
                        m_SpecialSecondary = 0
                    }
                };

                bp.OverrideFootprintType = false;
                bp.FootprintType = FootprintType.Humanoid;

                // CommonSettings
                bp.CommonSettings = new BlueprintClassAdditionalVisualSettings.SettingsData
                {
                    m_EquipmentEntities = new[]
                    {
                        new KingmakerEquipmentEntityReference
                        {
                            deserializedGuid = BlueprintGuid.Parse("018f7b68f0dc48499e6a1d8d5772fbcb")
                        },
                        new KingmakerEquipmentEntityReference
                        {
                            deserializedGuid = BlueprintGuid.Parse("640221f8f88a47f18bcf6a9a6a71e19d")
                        }
                    },
                    FXs = Array.Empty<PrefabLink>()
                };

                // InGameSettings
                bp.InGameSettings = new BlueprintClassAdditionalVisualSettings.SettingsData
                {
                    m_EquipmentEntities = Array.Empty<KingmakerEquipmentEntityReference>(),
                    FXs = Array.Empty<PrefabLink>()
                };

            });

            var visualSettingsProgression = Helpers.CreateBlueprint<BlueprintClassAdditionalVisualSettingsProgression>(IsekaiContext, "DragonClassVisualProgression", bp =>
            {
                bp.Entries = new BlueprintClassAdditionalVisualSettingsProgression.Entry[]
                {
                    new BlueprintClassAdditionalVisualSettingsProgression.Entry
                    {
                        Level = 1,
                        m_Settings = visualSettings.ToReference<BlueprintClassAdditionalVisualSettings.Reference>()
                    }
                };
            });

            var spellsKnown = Helpers.CreateBlueprint<BlueprintSpellsTable>(IsekaiContext, "DragonClassSpellsKnown", bp =>
            {
                bp.Levels = new SpellsLevelEntry[]
                {
                    new SpellsLevelEntry { Count = new int[] {} }, // Level 0
                    new SpellsLevelEntry { Count = new int[] {0,1} }, // Level 1
                    new SpellsLevelEntry { Count = new int[] {0,1} }, // Level 2
                    new SpellsLevelEntry { Count = new int[] {0,1} }, // Level 3
                    new SpellsLevelEntry { Count = new int[] {0,1} }, // Level 4
                    new SpellsLevelEntry { Count = new int[] {0,1} }, // Level 5
                    new SpellsLevelEntry { Count = new int[] {0,1} }, // Level 6
                    new SpellsLevelEntry { Count = new int[] {0,1} }, // Level 7
                    new SpellsLevelEntry { Count = new int[] {0,1} }, // Level 8
                    new SpellsLevelEntry { Count = new int[] {0,1} }, // Level 9
                    new SpellsLevelEntry { Count = new int[] {0,1} }, // Level 10
                    new SpellsLevelEntry { Count = new int[] {0,1} }, // Level 11
                    new SpellsLevelEntry { Count = new int[] {0,1} }, // Level 12
                    new SpellsLevelEntry { Count = new int[] {0,1} }, // Level 13
                    new SpellsLevelEntry { Count = new int[] {0,1} }, // Level 14
                    new SpellsLevelEntry { Count = new int[] {0,1} }, // Level 15
                    new SpellsLevelEntry { Count = new int[] {0,1} }, // Level 16
                    new SpellsLevelEntry { Count = new int[] {0,1} }, // Level 17
                    new SpellsLevelEntry { Count = new int[] {0,1} }, // Level 18
                    new SpellsLevelEntry { Count = new int[] {0,1} }, // Level 19
                    new SpellsLevelEntry { Count = new int[] {0,1} }, // Level 20
                    new SpellsLevelEntry { Count = new int[] {0,1} }, // Level 21
                    new SpellsLevelEntry { Count = new int[] {0,1} }, // Level 22
                    new SpellsLevelEntry { Count = new int[] {0,1} }, // Level 23
                    new SpellsLevelEntry { Count = new int[] {0,1} }, // Level 24
                    new SpellsLevelEntry { Count = new int[] {0,1} }, // Level 25
                    new SpellsLevelEntry { Count = new int[] {0,1} }, // Level 26
                    new SpellsLevelEntry { Count = new int[] {0,1} }, // Level 27
                    new SpellsLevelEntry { Count = new int[] {0,0,0,0,0,0,0,0,1,0} }, // Level 28 - Wyrm
                    new SpellsLevelEntry { Count = new int[] {0,0,0,0,0,0,0,0,2,1} }, // Level 29
                    new SpellsLevelEntry { Count = new int[] {0,0,0,0,0,0,0,0,2,2} }, // Level 30 - Great Wyrm
                    new SpellsLevelEntry { Count = new int[] {0,0,0,0,0,0,0,0,2,2} }, // Level 31
                    new SpellsLevelEntry { Count = new int[] {0,0,0,0,0,0,0,0,2,2} }, // Level 32
                    new SpellsLevelEntry { Count = new int[] {0,0,0,0,0,0,0,0,2,2} }, // Level 33
                    new SpellsLevelEntry { Count = new int[] {0,0,0,0,0,0,0,0,2,2} }, // Level 34
                    new SpellsLevelEntry { Count = new int[] {0,0,0,0,0,0,0,0,2,2} }, // Level 35
                    new SpellsLevelEntry { Count = new int[] {0,0,0,0,0,0,0,0,2,2} }, // Level 36
                    new SpellsLevelEntry { Count = new int[] {0,0,0,0,0,0,0,0,2,2} }, // Level 37
                    new SpellsLevelEntry { Count = new int[] {0,0,0,0,0,0,0,0,2,2} }, // Level 38
                    new SpellsLevelEntry { Count = new int[] {0,0,0,0,0,0,0,0,2,2} }, // Level 39
                    new SpellsLevelEntry { Count = new int[] {0,0,0,0,0,0,0,0,2,2} }, // Level 40
                };
            });
            var spellsPerDay = Helpers.CreateBlueprint<BlueprintSpellsTable>(IsekaiContext, "DragonClassSpellsPerDay", bp =>
            {
                bp.Levels = new SpellsLevelEntry[]
                {
                    new SpellsLevelEntry { Count = new int[] {} }, // Level 0
                    new SpellsLevelEntry { Count = new int[] {0,3} }, // Level 1
                    new SpellsLevelEntry { Count = new int[] {0,3} }, // Level 2
                    new SpellsLevelEntry { Count = new int[] {0,3} }, // Level 3
                    new SpellsLevelEntry { Count = new int[] {0,3} }, // Level 4
                    new SpellsLevelEntry { Count = new int[] {0,3} }, // Level 5
                    new SpellsLevelEntry { Count = new int[] {0,3} }, // Level 6
                    new SpellsLevelEntry { Count = new int[] {0,3} }, // Level 7
                    new SpellsLevelEntry { Count = new int[] {0,3} }, // Level 8
                    new SpellsLevelEntry { Count = new int[] {0,3} }, // Level 9
                    new SpellsLevelEntry { Count = new int[] {0,3} }, // Level 10
                    new SpellsLevelEntry { Count = new int[] {0,3} }, // Level 11
                    new SpellsLevelEntry { Count = new int[] {0,3} }, // Level 12 (level 1 adjusted to 4 with stats)
                    new SpellsLevelEntry { Count = new int[] {0,3} }, // Level 13
                    new SpellsLevelEntry { Count = new int[] {0,5} }, // Level 14
                    new SpellsLevelEntry { Count = new int[] {0,5} }, // Level 15
                    new SpellsLevelEntry { Count = new int[] {0,6,4} }, // Level 16
                    new SpellsLevelEntry { Count = new int[] {0,6,4} }, // Level 17
                    new SpellsLevelEntry { Count = new int[] {0,6,6,4} }, // Level 18
                    new SpellsLevelEntry { Count = new int[] {0,6,6,4} }, // Level 19
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,4} }, // Level 20
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,4} }, // Level 21
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,4} }, // Level 22
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,4} }, // Level 23
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,6,4} }, // Level 24
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,6,4} }, // Level 25
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,6,6,4} }, // Level 26
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,6,6,4} }, // Level 27
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,6,6,6,4} }, // Level 28
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,6,6,6,4} }, // Level 29
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,6,6,6,6,4} }, // Level 30
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,6,6,6,6,4} }, // Level 31
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,6,6,6,6,4} }, // Level 32
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,6,6,6,6,4} }, // Level 33
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,6,6,6,6,4} }, // Level 34
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,6,6,6,6,4} }, // Level 35
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,6,6,6,6,4} }, // Level 36
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,6,6,6,6,4} }, // Level 37
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,6,6,6,6,4} }, // Level 38
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,6,6,6,6,4} }, // Level 39
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,6,6,6,6,4} }, // Level 40
                };
            });
            var spellsList = Helpers.CreateBlueprint<BlueprintSpellList>(IsekaiContext, "DragonClassSpellsList", bp =>
            {
                bp.SpellsByLevel = new SpellLevelList[10]
                {
                    new SpellLevelList(0) { m_Spells = new List<BlueprintAbilityReference>() },
                    new SpellLevelList(1) { m_Spells = new List<BlueprintAbilityReference>() },
                    new SpellLevelList(2) { m_Spells = new List<BlueprintAbilityReference>() },
                    new SpellLevelList(3) { m_Spells = new List<BlueprintAbilityReference>() },
                    new SpellLevelList(4) { m_Spells = new List<BlueprintAbilityReference>() },
                    new SpellLevelList(5) { m_Spells = new List<BlueprintAbilityReference>() },
                    new SpellLevelList(6) { m_Spells = new List<BlueprintAbilityReference>() },
                    new SpellLevelList(7) { m_Spells = new List<BlueprintAbilityReference>() },
                    new SpellLevelList(8) { m_Spells = new List<BlueprintAbilityReference>() },
                    new SpellLevelList(9) { m_Spells = new List<BlueprintAbilityReference>() },
                };
            });

            var spellbook = Helpers.CreateBlueprint<BlueprintSpellbook>(IsekaiContext, "DragonClassSpellbook", bp =>
            {
                bp.Name = Helpers.CreateString(IsekaiContext, "DragonClassSpellbook.Name", "Dragon Class Spellbook");
                bp.m_CharacterClass = DragonClass.GetReference();
                bp.m_SpellsPerDay = BlueprintTools.GetBlueprintReference<BlueprintSpellsTableReference>("dbff16956df2eda48a1da5c9617cc836");// spellsPerDay.ToReference<BlueprintSpellsTableReference>();
                bp.m_SpellsKnown = BlueprintTools.GetBlueprintReference<BlueprintSpellsTableReference>("d232bc78d967a964bac4d4d38e7ca5f4");// spellsKnown.ToReference<BlueprintSpellsTableReference>();
                //bp.m_SpellList = spellsList.ToReference<BlueprintSpellListReference>();
                // Use sorcerer (aka wizard) spell list for now
                bp.m_SpellList = BlueprintTools.GetBlueprintReference<BlueprintSpellListReference>("ba0401fdeb4062f40a7aa95b6f07fe89");
                bp.CastingAttribute = StatType.Charisma;
                bp.CantripsType = CantripsType.Cantrips;
                bp.IsArcane = true;
                bp.Spontaneous = true;
                bp.CasterLevelModifier = 0;// -11;

                // These relate to special spell slots (like wizard's favourite school spell slots or shaman's spirit magic slots)
                bp.HasSpecialSpellList = false;
                bp.SpecialSpellListName = StaticReferences.Strings.Null;
            });
            PatchTools.RegisterSpellbook(spellbook);

            // Main Class
            dragonClass = Helpers.CreateBlueprint<BlueprintCharacterClass>(IsekaiContext, "DragonClass", bp =>
            {
                bp.LocalizedName = Name;
                bp.LocalizedDescription = Description;
                bp.LocalizedDescriptionShort = DescriptionShort;
                bp.HitDie = DiceType.D12;
                bp.m_BaseAttackBonus = BABFull.ToReference<BlueprintStatProgressionReference>();
                bp.m_FortitudeSave = SavesHigh.ToReference<BlueprintStatProgressionReference>();
                bp.m_ReflexSave = SavesHigh.ToReference<BlueprintStatProgressionReference>();
                bp.m_WillSave = SavesHigh.ToReference<BlueprintStatProgressionReference>();
                bp.m_Difficulty = 1;
                bp.m_Spellbook = spellbook.ToReference<BlueprintSpellbookReference>();
                bp.RecommendedAttributes = new StatType[] { StatType.Strength, StatType.Charisma };
                bp.NotRecommendedAttributes = new StatType[] { };
                bp.m_EquipmentEntities = new KingmakerEquipmentEntityReference[0];                
                bp.m_StartingItems = new BlueprintItemReference[] {
                };
                bp.SkillPoints = 4;
                bp.ClassSkills = new StatType[] {
                    StatType.SkillAthletics,
                    StatType.SkillMobility,
                    StatType.SkillPerception,
                    StatType.SkillPersuasion,
                    StatType.SkillLoreReligion,
                    StatType.SkillLoreNature,
                    StatType.SkillKnowledgeArcana,
                    StatType.SkillKnowledgeWorld,
                    StatType.SkillStealth,
                    StatType.SkillUseMagicDevice
                };
                bp.IsDivineCaster = false;
                bp.IsArcaneCaster = true;
                bp.StartingGold = 0;
                bp.PrimaryColor = 0;
                bp.SecondaryColor = 0;
                bp.MaleEquipmentEntities = clothesClass.MaleEquipmentEntities;
                bp.FemaleEquipmentEntities = clothesClass.FemaleEquipmentEntities;
                bp.m_SignatureAbilities = new BlueprintFeatureReference[0] {
                };

                // Register Archetypes later using RegisterArchetype
                bp.m_Archetypes = new BlueprintArchetypeReference[0];

                // Set Progression later using SetProgression (Some features in the progression reference IsekaiProtagonistClass which doeesn't exist yet)
                bp.m_Progression = DragonProgression.GetReference();

                // Set Default Build later using SetDefaultBuild
                bp.m_DefaultBuild = null;

                bp.m_AdditionalVisualSettings = visualSettingsProgression.ToReference<BlueprintClassAdditionalVisualSettingsProgression.Reference>();// dragonAdditionalVisualSettings.ToReference<BlueprintClassAdditionalVisualSettingsProgression.Reference>();

                //bp.AddComponent<PrerequisiteCondition>(c =>
                //{
                //    c.Condition = new OrAndLogic
                //    {
                //        ConditionsChecker = new ConditionsChecker
                //        {
                //            Operation = Operation.Or,
                //            Conditions = new Condition[]
                //            {
                //                new HasFact
                //                {
                //                    m_Fact = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(IsekaiContext, "KitsuneHalfDragonHeritage")
                //                }
                //            }
                //        }
                //    };
                //});
            });

            // Register Class
            TTCoreExtensions.RegisterClass(dragonClass);
        }

        public static void SetProgression(BlueprintProgression progression)
        {
            BlueprintCharacterClass IsekaiProtagonistClass = Get();
            IsekaiProtagonistClass.m_Progression = progression.ToReference<BlueprintProgressionReference>();
        }

        public static BlueprintCharacterClass Get()
        {
            if (dragonClass != null)
            {
                return dragonClass;
            }
            return BlueprintTools.GetModBlueprint<BlueprintCharacterClass>(IsekaiContext, "DragonClass");
        }

        public static BlueprintCharacterClassReference GetReference()
        {
            return BlueprintTools.GetModBlueprintReference<BlueprintCharacterClassReference>(IsekaiContext, "DragonClass");
        }
    }
}
