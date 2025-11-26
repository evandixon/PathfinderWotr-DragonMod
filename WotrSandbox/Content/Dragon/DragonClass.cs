using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
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
using static WotrSandbox.Main;

namespace WotrSandbox.Content.Dragon
{
    internal static class DragonClass
    {
        private static readonly LocalizedString Name = Helpers.CreateString(IsekaiContext, $"DragonClass.Name", "Half Dragon");
        private static readonly LocalizedString Description = Helpers.CreateString(IsekaiContext, $"DragonClass.Description",
            "Half Dragon");
        private static readonly LocalizedString DescriptionShort = Helpers.CreateString(IsekaiContext, $"DragonClass.DescriptionShort",
            "Half Dragon");
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
                bp.m_Spellbook = null;// IsekaiProtagonistSpellbook.GetReference();
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
                    StatType.SkillKnowledgeArcana,
                    StatType.SkillStealth,
                    StatType.SkillLoreNature
                };
                bp.IsDivineCaster = false;
                bp.IsArcaneCaster = false;
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
                bp.m_Progression = null;

                // Set Default Build later using SetDefaultBuild
                bp.m_DefaultBuild = null;

                bp.m_AdditionalVisualSettings = null;// dragonAdditionalVisualSettings.ToReference<BlueprintClassAdditionalVisualSettingsProgression.Reference>();

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

            //IsekaiProtagonistSpellbook.SetCharacterClass(isekaiProtagonistClass);

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
