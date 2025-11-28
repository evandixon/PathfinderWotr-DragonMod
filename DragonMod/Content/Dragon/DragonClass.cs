using DragonMod.Content.Dragon.Bloodlines;
using DragonMod.Content.Dragon.Bloodlines.Gold;
using DragonMod.Infrastructure;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Localization;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using Kingmaker.Utility;
using System;
using TabletopTweaks.Core.Utilities;
using static DragonMod.Main;

namespace DragonMod.Content.Dragon
{
    internal static class DragonClass
    {
        private static readonly LocalizedString Name = Helpers.CreateString(DragonModContext, $"DragonClass.Name", "Dragon");
        private static readonly LocalizedString Description = Helpers.CreateString(DragonModContext, $"DragonClass.Description",
            "As a result of strange magical experiments, you have become a half dragon. " +
            "Your draconic soul is incubating and you will some day become a true dragon.");
        private static readonly LocalizedString DescriptionShort = Helpers.CreateString(DragonModContext, $"DragonClass.DescriptionShort",
            "As a result of strange magical experiments, you have become a half dragon, with a path toward true dragonhood.");
        private static BlueprintCharacterClass dragonClass;

        // Stat Progression
        private static readonly BlueprintStatProgression BABFull = BlueprintTools.GetBlueprint<BlueprintStatProgression>("b3057560ffff3514299e8b93e7648a9d");
        private static readonly BlueprintStatProgression SavesHigh = BlueprintTools.GetBlueprint<BlueprintStatProgression>("ff4662bde9e75f145853417313842751");

        public static void Add()
        {
            var defaultClothesIndex = StaticReferences.BaseClasses.IndexOf(ClassTools.Classes.SorcererClass);
            var clothesIndex = DragonModContext.AddedContent.IsekaiDefaultClothes;
            var maxClothesIndex = StaticReferences.BaseClasses.Length;
            var clothesClass = StaticReferences.BaseClasses[clothesIndex > -1 && clothesIndex < maxClothesIndex ? clothesIndex : defaultClothesIndex];

            var visualSettings = Helpers.CreateBlueprint<BlueprintClassAdditionalVisualSettings>(DragonModContext, "DragonClassVisual1", bp =>
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

            var visualSettingsProgression = Helpers.CreateBlueprint<BlueprintClassAdditionalVisualSettingsProgression>(DragonModContext, "DragonClassVisualProgression", bp =>
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

            var defaultBuild = GetDefaultBuild();

            // Main Class
            dragonClass = Helpers.CreateBlueprint<BlueprintCharacterClass>(DragonModContext, "DragonClass", bp =>
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
                bp.m_Spellbook = null;
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
                bp.m_DefaultBuild = defaultBuild.ToReference<BlueprintUnitFactReference>();

                bp.m_AdditionalVisualSettings = null;// visualSettingsProgression.ToReference<BlueprintClassAdditionalVisualSettingsProgression.Reference>();// dragonAdditionalVisualSettings.ToReference<BlueprintClassAdditionalVisualSettingsProgression.Reference>();

                bp.m_Archetypes = new BlueprintArchetypeReference[]
                {
                    DragonBloodlineGold.Instance.GetReference<BlueprintArchetypeReference>(),
                    DragonBloodlineSilver.Instance.GetReference<BlueprintArchetypeReference>(),
                };

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
            return BlueprintTools.GetModBlueprint<BlueprintCharacterClass>(DragonModContext, "DragonClass");
        }

        public static BlueprintCharacterClassReference GetReference()
        {
            return BlueprintTools.GetModBlueprintReference<BlueprintCharacterClassReference>(DragonModContext, "DragonClass");
        }

        private static BlueprintFeature GetDefaultBuild()
        {
            var basicFeatSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("247a4068296e8be42890143f451b4b45");

            var PowerAttack = BlueprintTools.GetBlueprint<BlueprintFeature>("9972f33f977fc724c838e59641b2fca5");
            var CombatReflexes = BlueprintTools.GetBlueprint<BlueprintFeature>("0f8939ae6f220984e8fb568abbdfba95");
            var ImprovedInitiative = BlueprintTools.GetBlueprint<BlueprintFeature>("797f25d709f559546b29e7bcb181cc74");
            var IronWill = BlueprintTools.GetBlueprint<BlueprintFeature>("175d1577bb6c9a04baf88eec99c66334");
            var Outflank = BlueprintTools.GetBlueprint<BlueprintFeature>("422dab7309e1ad343935f33a4d6e9f11");
            var IronWillImproved = BlueprintTools.GetBlueprint<BlueprintFeature>("3ea2215150a1c8a4a9bfed9d9023903e");
            var Dodge = BlueprintTools.GetBlueprint<BlueprintFeature>("97e216dbb46ae3c4faef90cf6bbe6fd5");
            var Toughness = BlueprintTools.GetBlueprint<BlueprintFeature>("d09b20029e9abfe4480b356c92095623");
            var SpellPenetration = BlueprintTools.GetBlueprint<BlueprintFeature>("ee7dc126939e4d9438357fbd5980d459");
            var GreaterSpellPenetration = BlueprintTools.GetBlueprint<BlueprintFeature>("1978c3f91cfbbc24b9c9b0d017f4beec");

            var defaultBuild = Helpers.CreateBlueprint<BlueprintFeature>(DragonModContext, "DragonClassDefaultBuild", bp =>
            {
                bp.Ranks = 1;
                bp.HideInUI = true;

                bp.AddComponent<AddClassLevels>(c =>
                {
                    c.DoNotApplyAutomatically = false;
                    c.m_CharacterClass = GetReference();
                    c.Levels = 30;
                    c.RaceStat = StatType.Strength;
                    c.LevelsStat = StatType.Charisma;

                    c.Skills = new StatType[]
                    {
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

                    c.Selections = new SelectionEntry[]
                    {
                        new SelectionEntry
                        {
                            m_Selection = basicFeatSelection.ToReference<BlueprintFeatureSelectionReference>(),
                            m_Features = new BlueprintFeatureReference[]
                            {
                                PowerAttack.ToReference<BlueprintFeatureReference>(),
                                CombatReflexes.ToReference<BlueprintFeatureReference>(),
                                ImprovedInitiative.ToReference<BlueprintFeatureReference>(),
                                Outflank.ToReference<BlueprintFeatureReference>(),
                                IronWill.ToReference<BlueprintFeatureReference>(),
                                IronWillImproved.ToReference<BlueprintFeatureReference>(),
                                Dodge.ToReference<BlueprintFeatureReference>(),
                                Toughness.ToReference<BlueprintFeatureReference>(),
                                SpellPenetration.ToReference<BlueprintFeatureReference>(),
                                GreaterSpellPenetration.ToReference<BlueprintFeatureReference>(),
                            }
                        },
                        new SelectionEntry
                        {
                            m_Selection = DragonBloodlineSelection.GetReference<BlueprintFeatureSelectionReference>(),
                            m_Features = new BlueprintFeatureReference[]
                            {
                                DragonBloodlineGold.Instance.GetReference<BlueprintFeatureReference>()
                            }
                        }
                    };
                    c.m_SelectSpells = new BlueprintAbilityReference[]
                    {
                        BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("90e59f4a4ada87243b7b3535a06d0638")
                    };
                });


                // Todo: update
                bp.AddComponent<StatsDistributionPreset>(c => {
                    c.TargetPoints = 20;
                    c.Strength = 14;
                    c.Dexterity = 12;
                    c.Constitution = 8;
                    c.Intelligence = 10;
                    c.Wisdom = 12;
                    c.Charisma = 17;
                });
                // Todo: update
                bp.AddComponent<StatsDistributionPreset>(c => {
                    c.TargetPoints = 25;
                    c.Strength = 14;
                    c.Dexterity = 13;
                    c.Constitution = 8;
                    c.Intelligence = 10;
                    c.Wisdom = 12;
                    c.Charisma = 18;
                });
                // Todo: update
                bp.AddComponent<BuildBalanceRadarChart>(c => {
                    c.Control = 2;
                    c.Defense = 5;
                    c.Magic = 4;
                    c.Melee = 5;
                    c.Ranged = 2;
                    c.Support = 3;
                });

            });

            return defaultBuild;
        }
    }
}
