using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Root.Fx;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.Localization;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using Kingmaker.Visual;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using UnityEngine;
using static WotrSandbox.Main;

namespace WotrSandbox.Content.Dragon.Bloodlines
{
    public static class DragonBloodlineGold
    {
        public static void Add()
        {
            var goldDragonWings = BlueprintTools.GetBlueprint<BlueprintFeature>("6929bac6c67ae194c8c8446e3d593953");
            var coldVulnerability = BlueprintTools.GetBlueprint<BlueprintFeature>("b8bbe8f713da9ad44a899aa551ca6b5b");
            var fireImmunity = BlueprintTools.GetBlueprint<BlueprintFeature>("11ac3433adfa74642a93111624376070");

            var breathCooldownBuff = Helpers.CreateBlueprint<BlueprintBuff>(IsekaiContext, "DragonBloodlineGoldBreathCooldown", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(IsekaiContext, "DragonBloodlineGoldBreathCooldown.Name", "Fire Breath Cooldown");
                bp.m_Flags = BlueprintBuff.Flags.IsFromSpell;
                bp.Frequency = DurationRate.Rounds;
                bp.Stacking = StackingType.Replace;
            });

            var breathFeature = GetPrimaryBreath(breathCooldownBuff);
            var secondaryBreathFeature = GetSecondaryBreath(breathCooldownBuff);

            //var mediumFormFeature = GetMediumFormFeature();

            var bloodlineGold = Helpers.CreateBlueprint<BlueprintProgression>(IsekaiContext, "DragonBloodlineGold", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(IsekaiContext, $"DragonBloodlineGold.Name", "Gold");
                bp.m_Description = Helpers.CreateString(IsekaiContext, $"DragonBloodlineGold.Description", "Gold Dragon");
                bp.m_DescriptionShort = Helpers.CreateString(IsekaiContext, $"DragonBloodlineGold.DescriptionShort", "");
                bp.LevelEntries = new LevelEntry[20]
                {
                    Helpers.CreateLevelEntry(1, goldDragonWings, fireImmunity, coldVulnerability),
                    Helpers.CreateLevelEntry(2, secondaryBreathFeature),
                    Helpers.CreateLevelEntry(3, breathFeature),
                    Helpers.CreateLevelEntry(4, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(5, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(6, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(7, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(8, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(9, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(10, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(11, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(12, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(13, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(14, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(15, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(16, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(17, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(18, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(19, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(20, new BlueprintFeature[0])
                };
            });
        }

        private static BlueprintFeature GetPrimaryBreath(BlueprintBuff breathCooldownBuff)
        {
            var breathAbility = Helpers.CreateBlueprint<BlueprintAbility>(IsekaiContext, "DragonBloodlineGoldBreathAbility", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(IsekaiContext, "DragonBloodlineGoldBreathAbility.Name", "Fire Breath");
                bp.CanTargetPoint = true;
                bp.CanTargetEnemies = true;
                bp.CanTargetFriends = true;
                bp.CanTargetSelf = true;
                bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
                bp.EffectOnAlly = AbilityEffectOnUnit.None;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.BreathWeapon;
                bp.ActionType = UnitCommand.CommandType.Standard;
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.Projectile;
                bp.LocalizedDuration = new LocalizedString(); // todo
                bp.LocalizedSavingThrow = new LocalizedString(); //todo

                bp.AddComponent<AbilityCasterHasNoFacts>(c =>
                {
                    c.m_Facts = new[]
                    {
                        breathCooldownBuff.ToReference<BlueprintUnitFactReference>()
                    };
                });

                bp.AddComponent<AbilityEffectRunAction>(c =>
                {
                    c.SavingThrowType = SavingThrowType.Reflex;
                    c.Actions = new ActionList
                    {
                        Actions = new GameAction[]
                        {
                            new ContextActionOnContextCaster
                            {
                                Actions = new ActionList
                                {
                                    Actions = new GameAction[]
                                    {
                                        new ContextActionApplyBuff
                                        {
                                            m_Buff = breathCooldownBuff.ToReference<BlueprintBuffReference>(),
                                            DurationValue = new ContextDurationValue
                                            {
                                                Rate = DurationRate.Rounds,
                                                DiceType = DiceType.D4,
                                                DiceCountValue = 1,
                                                BonusValue = 1
                                            }
                                        }
                                    }
                                }
                            },
                            new ContextActionDealDamage
                            {
                                m_Type = ContextActionDealDamage.Type.Damage,
                                DamageType = new DamageTypeDescription
                                {
                                    Type = DamageType.Energy,
                                    Energy = DamageEnergyType.Fire
                                },
                                Value = new ContextDiceValue
                                {
                                    DiceType = DiceType.D6,
                                    DiceCountValue = new ContextValue
                                    {
                                        ValueType = ContextValueType.Rank,
                                        ValueRank = AbilityRankType.DamageDice
                                    }
                                },
                                IsAoE = true,
                                HalfIfSaved = true
                            }
                        }
                    };
                });
                bp.AddComponent<AbilityDeliverProjectile>(c =>
                {
                    //FireCone30Feet00Breath
                    c.m_Projectiles = new[] { BlueprintTools.GetBlueprintReference<BlueprintProjectileReference>("52c3a84f628ddde4dbfb38e4a581b01a") };
                    c.Type = AbilityProjectileType.Cone;
                    c.m_Length = new Feet
                    {
                        m_Value = 30
                    };
                    c.m_LineWidth = new Feet
                    {
                        m_Value = 5
                    };
                    c.NeedAttackRoll = false;
                });
                bp.AddComponent<ContextRankConfig>(c =>
                {
                    c.m_Type = Kingmaker.Enums.AbilityRankType.DamageDice;
                    c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    c.m_Progression = ContextRankProgression.AsIs;
                    c.m_Max = 20;
                    c.m_Class = new BlueprintCharacterClassReference[]
                    {
                        DragonClass.GetReference()
                    };
                });
                bp.AddComponent<ContextRankConfig>(c =>
                {
                    c.m_Type = Kingmaker.Enums.AbilityRankType.StatBonus;
                    c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    c.m_Progression = ContextRankProgression.Div2;
                    c.m_Max = 20;
                    c.m_Class = new BlueprintCharacterClassReference[]
                    {
                        DragonClass.GetReference()
                    };
                });
                bp.AddComponent<ContextCalculateAbilityParamsBasedOnClass>(c =>
                {
                    c.m_CharacterClass = DragonClass.GetReference();
                    c.StatType = StatType.Constitution;
                });
                bp.AddComponent<SpellDescriptorComponent>(c =>
                {
                    c.Descriptor = new SpellDescriptorWrapper
                    {
                        m_IntValue = 1099511627777
                    };
                });
            });
            var breathAbilityReference = BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(IsekaiContext, "DragonBloodlineGoldBreathAbility");
            var breathAbilityReferenceUnit = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(IsekaiContext, "DragonBloodlineGoldBreathAbility");

            return Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, "DragonBloodlineGoldBreathFeature", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(IsekaiContext, "DragonBloodlineGoldBreathFeature.Name", "Fire Breath");

                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddComponent<AddFacts>(c =>
                {
                    c.m_Facts = new BlueprintUnitFactReference[]
                    {
                        breathAbilityReferenceUnit
                    };
                });
                bp.AddComponent<ReplaceAbilitiesStat>(c =>
                {
                    c.Stat = StatType.Constitution;
                    c.m_Ability = new BlueprintAbilityReference[]
                    {
                        breathAbilityReference
                    };
                });
                bp.AddComponent<ReplaceCasterLevelOfAbility>(c =>
                {
                    c.m_Spell = breathAbilityReference;
                    c.m_Class = DragonClass.GetReference();
                });
                bp.AddComponent<BindAbilitiesToClass>(c =>
                {
                    c.LevelStep = 1;
                    c.Odd = true;
                    c.FullCasterChecks = true;
                    c.Stat = StatType.Charisma;
                    c.m_CharacterClass = DragonClass.GetReference();
                    c.m_Abilites = new[] { breathAbilityReference };
                });
            });
        }

        private static BlueprintFeature GetSecondaryBreath(BlueprintBuff breathCooldownBuff)
        {
            var breathAbility = Helpers.CreateBlueprint<BlueprintAbility>(IsekaiContext, "DragonBloodlineGoldSeconaryBreathAbility", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(IsekaiContext, "DragonBloodlineGoldSeconaryBreathAbility.Name", "Weakening Breath");
                bp.CanTargetPoint = true;
                bp.CanTargetEnemies = true;
                bp.CanTargetFriends = true;
                bp.CanTargetSelf = true;
                bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
                bp.EffectOnAlly = AbilityEffectOnUnit.None;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.BreathWeapon;
                bp.ActionType = UnitCommand.CommandType.Standard;
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.Projectile;
                bp.LocalizedDuration = new LocalizedString(); // todo
                bp.LocalizedSavingThrow = new LocalizedString(); //todo

                bp.AddComponent<AbilityCasterHasNoFacts>(c =>
                {
                    c.m_Facts = new[]
                    {
                        breathCooldownBuff.ToReference<BlueprintUnitFactReference>()
                    };
                });

                bp.AddComponent<AbilityEffectRunAction>(c =>
                {
                    c.Actions = new ActionList
                    {
                        Actions = new GameAction[]
                        {
                            new ContextActionOnContextCaster
                            {
                                Actions = new ActionList
                                {
                                    Actions = new GameAction[]
                                    {
                                        new ContextActionApplyBuff
                                        {
                                            m_Buff = breathCooldownBuff.ToReference<BlueprintBuffReference>(),
                                            DurationValue = new ContextDurationValue
                                            {
                                                Rate = DurationRate.Rounds,
                                                DiceType = DiceType.D4,
                                                DiceCountValue = 1,
                                                BonusValue = 1
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    };
                });

                bp.AddComponent<AbilityEffectRunAction>(c =>
                {
                    c.SavingThrowType = SavingThrowType.Will;
                    c.Actions = new ActionList
                    {
                        Actions = new GameAction[]
                        {
                            new ContextActionDealDamage
                            {
                                m_Type = ContextActionDealDamage.Type.AbilityDamage,
                                Drain = false,
                                AbilityType = StatType.Strength,
                                Value = new ContextDiceValue
                                {
                                    DiceType = DiceType.D6,
                                    DiceCountValue = new ContextValue
                                    {
                                        ValueType = ContextValueType.Simple,
                                        Value = 0
                                    },
                                    BonusValue = new ContextValue
                                    {
                                        ValueType = ContextValueType.Rank,
                                        ValueRank = AbilityRankType.DamageBonus
                                    }
                                },
                                IsAoE = true,
                                HalfIfSaved = true
                            }
                        }
                    };
                });
                bp.AddComponent<AbilityDeliverProjectile>(c =>
                {
                    //NecromancyCone30Feet00Breath
                    c.m_Projectiles = new[] { BlueprintTools.GetBlueprintReference<BlueprintProjectileReference>("4d601ab51e167c04a8c6883260e872ee") };
                    c.Type = AbilityProjectileType.Cone;
                    c.m_Length = new Feet
                    {
                        m_Value = 30
                    };
                    c.m_LineWidth = new Feet
                    {
                        m_Value = 5
                    };
                    c.NeedAttackRoll = false;
                });
                bp.AddComponent<ContextRankConfig>(c =>
                {
                    c.m_Type = Kingmaker.Enums.AbilityRankType.DamageBonus;
                    c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    c.m_Progression = ContextRankProgression.Div2;
                    c.m_Max = 20;
                    c.m_Class = new BlueprintCharacterClassReference[]
                    {
                        DragonClass.GetReference()
                    };
                });
                bp.AddComponent<ContextRankConfig>(c =>
                {
                    c.m_Type = Kingmaker.Enums.AbilityRankType.StatBonus;
                    c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    c.m_Progression = ContextRankProgression.Div2;
                    c.m_Max = 20;
                    c.m_Class = new BlueprintCharacterClassReference[]
                    {
                        DragonClass.GetReference()
                    };
                });
                bp.AddComponent<ContextCalculateAbilityParamsBasedOnClass>(c =>
                {
                    c.m_CharacterClass = DragonClass.GetReference();
                    c.StatType = StatType.Constitution;
                });
                bp.AddComponent<SpellDescriptorComponent>(c =>
                {
                    c.Descriptor = new SpellDescriptorWrapper
                    {
                        m_IntValue = 1099511627777
                    };
                });
            });
            var breathAbilityReference = BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(IsekaiContext, "DragonBloodlineGoldSeconaryBreathAbility");
            var breathAbilityReferenceUnit = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(IsekaiContext, "DragonBloodlineGoldSeconaryBreathAbility");

            return Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, "DragonBloodlineGoldSecondaryBreathFeature", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(IsekaiContext, "DragonBloodlineGoldSeconaryBreathAbility.Name", "Weakening Breath");

                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddComponent<AddFacts>(c =>
                {
                    c.m_Facts = new BlueprintUnitFactReference[]
                    {
                        breathAbilityReferenceUnit
                    };
                });
                bp.AddComponent<ReplaceAbilitiesStat>(c =>
                {
                    c.Stat = StatType.Constitution;
                    c.m_Ability = new BlueprintAbilityReference[]
                    {
                        breathAbilityReference
                    };
                });
                bp.AddComponent<ReplaceCasterLevelOfAbility>(c =>
                {
                    c.m_Spell = breathAbilityReference;
                    c.m_Class = DragonClass.GetReference();
                });
                bp.AddComponent<BindAbilitiesToClass>(c =>
                {
                    c.LevelStep = 1;
                    c.Odd = true;
                    c.FullCasterChecks = true;
                    c.Stat = StatType.Charisma;
                    c.m_CharacterClass = DragonClass.GetReference();
                    c.m_Abilites = new[] { breathAbilityReference };
                });
            });
        }

        private static BlueprintFeature GetMediumFormFeature()
        {
            var buff = Helpers.CreateBlueprint<BlueprintBuff>(IsekaiContext, "DragonBloodlineGoldFormMediumAbility", bp =>
            {
                bp.Comment = "";
                bp.m_AllowNonContextActions = false;
                bp.IsClassFeature = true;
                bp.m_Flags = 0;
                bp.Stacking = StackingType.Replace;
                bp.Ranks = 0;
                bp.TickEachSecond = false;
                bp.Frequency = DurationRate.Rounds;
                bp.FxOnStart = null;
                bp.FxOnRemove = null;
                bp.ResourceAssetIds = Array.Empty<string>();

                bp.m_Icon = null; //todo

                // === Components ===

                // 1) Polymorph
                var enterOldCurve = new AnimationCurve(
                    new Keyframe(0f, 0f, 0f, 1f),
                    new Keyframe(1f, 1f, 1f, 0f)
                );
                enterOldCurve.preWrapMode = (WrapMode)8;
                enterOldCurve.postWrapMode = (WrapMode)8;

                var enterNewCurve = new AnimationCurve(
                    new Keyframe(0f, 0f, 0f, 1f),
                    new Keyframe(1f, 1f, 1f, 0f)
                );
                enterNewCurve.preWrapMode = (WrapMode)8;
                enterNewCurve.postWrapMode = (WrapMode)8;

                var exitOldCurve = new AnimationCurve(
                    new Keyframe(0f, 0f, 0f, 1f),
                    new Keyframe(1f, 1f, 1f, 0f)
                );
                exitOldCurve.preWrapMode = (WrapMode)8;
                exitOldCurve.postWrapMode = (WrapMode)8;

                var exitNewCurve = new AnimationCurve(
                    new Keyframe(0f, 0f, 0f, 1f),
                    new Keyframe(1f, 1f, 1f, 0f)
                );
                exitNewCurve.preWrapMode = (WrapMode)8;
                exitNewCurve.postWrapMode = (WrapMode)8;

                var polymorph = new Polymorph
                {
                    name = "$Polymorph$6d979887-7a35-4133-9caf-37f051bebf9e",
                    m_Race = null,
                    m_Prefab = new UnitViewLink { AssetId = "7a47bc6dbd2e2014aa5be8519e93a02e" },
                    m_PrefabFemale = null,
                    m_SpecialDollType = SpecialDollType.None,
                    m_ReplaceUnitForInspection = null,
                    m_Portrait = new BlueprintPortraitReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("a2cad01ac4d2485f9a34e08d912bbe2c")
                    },
                    //m_PortraitTypeEntry = UnitEntityData.PortraitType.SmallPortrait,
                    m_KeepSlots = false,
                    Size = Size.Medium,
                    UseSizeAsBaseForDamage = true,
                    StrengthBonus = 4,
                    DexterityBonus = 0,
                    ConstitutionBonus = 2,
                    NaturalArmor = 4,
                    m_MainHand = null,
                    m_OffHand = null,
                    AllowDamageTransfer = true,
                    m_AdditionalLimbs = new[]
                    {
                        new BlueprintItemWeaponReference
                        {
                            deserializedGuid = BlueprintGuid.Parse("61bc14eca5f8c1040900215000cfc218")
                        },
                        new BlueprintItemWeaponReference
                        {
                            deserializedGuid = BlueprintGuid.Parse("65eb73689b94d894080d33a768cdf645")
                        },
                        new BlueprintItemWeaponReference
                        {
                            deserializedGuid = BlueprintGuid.Parse("65eb73689b94d894080d33a768cdf645")
                        }
                    },
                    m_SecondaryAdditionalLimbs = new[]
                    {
                        new BlueprintItemWeaponReference
                        {
                            deserializedGuid = BlueprintGuid.Parse("864e29d3e07ad4a4f96d576b366b4a86")
                        },
                        new BlueprintItemWeaponReference
                        {
                            deserializedGuid = BlueprintGuid.Parse("864e29d3e07ad4a4f96d576b366b4a86")
                        }
                    },
                    m_Facts = new[]
                    {
                        new BlueprintUnitFactReference
                        {
                            deserializedGuid = BlueprintGuid.Parse("bd09b025ee2a82f46afab922c4decca9")
                        },
                        new BlueprintUnitFactReference
                        {
                            deserializedGuid = BlueprintGuid.Parse("d808863c4bd44fd8bd9cf5892460705d")
                        }
                    },
                    m_EnterTransition = new Polymorph.VisualTransitionSettings
                    {
                        OldPrefabVisibilityTime = 0.5f,
                        OldPrefabFX = null,
                        NewPrefabFX = null,
                        ScaleTime = 0.5f,
                        ScaleOldPrefab = true,
                        OldScaleCurve = enterOldCurve,
                        ScaleNewPrefab = true,
                        NewScaleCurve = enterNewCurve
                    },
                    m_ExitTransition = new Polymorph.VisualTransitionSettings
                    {
                        OldPrefabVisibilityTime = 0.5f,
                        OldPrefabFX = null,
                        NewPrefabFX = null,
                        ScaleTime = 0.5f,
                        ScaleOldPrefab = true,
                        OldScaleCurve = exitOldCurve,
                        ScaleNewPrefab = true,
                        NewScaleCurve = exitNewCurve
                    },
                    m_TransitionExternal = ResourcesLibrary.TryGetResource<PolymorphTransitionSettings>("d17a5a97df63906499f72a39e55c5453"),
                    m_SilentCaster = true
                };

                // 2) SpellDescriptorComponent
                var spellDescriptor = new SpellDescriptorComponent
                {
                    Descriptor = new SpellDescriptorWrapper
                    {
                        m_IntValue = 8589934592L
                    }
                };

                // 3) ReplaceAsksList
                var replaceAsks = new ReplaceAsksList
                {
                    m_Asks = new BlueprintUnitAsksListReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("3c0924a80e504f04c94de6ec2a28f9aa")
                    }
                };

                // 4) BuffMovementSpeed
                var moveSpeed = new BuffMovementSpeed
                {
                    Descriptor = ModifierDescriptor.None,
                    Value = 10,
                    ContextBonus = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0,
                        ValueRank = AbilityRankType.Default,
                        ValueShared = AbilitySharedValue.Damage
                    },
                    CappedOnMultiplier = false,
                    MultiplierCap = 0f,
                    CappedMinimum = false,
                    MinimumCap = 0
                };

                // 5) ReplaceSourceBone
                var replaceSourceBone = new ReplaceSourceBone
                {
                    SourceBone = "Locator_HeadCenterFX_00"
                };

                // 6) ReplaceCastSource
                var replaceCastSource = new ReplaceCastSource
                {
                    CastSource = CastSource.Head
                };

                // 7) Blindsense
                var blindsense = new Blindsense
                {
                    name = "$Blindsense$ad06d492-8da0-40eb-acf6-0585ca0a0fa4",
                    Range = 30.Feet(),
                    Blindsight = false,
                    Exceptions = null
                };

                // 8) AddMechanicsFeature (Natural Spell)
                var naturalSpell = new AddMechanicsFeature
                {
                    name = "$AddMechanicsFeature$c064571d-8d0c-44b0-be7e-b16d162094dd",
                    m_Feature = AddMechanicsFeature.MechanicsFeatureType.NaturalSpell
                };

                // 9) AddFactContextActions (Activated / Deactivated / NewRound)
                var activatedActions = new ActionList
                {
                    Actions = new GameAction[]
                    {
                        new ContextActionApplyBuff
                        {
                            name = "$ContextActionApplyBuff$aac8af5b-4704-4a72-81db-025defb1fc3d",
                            m_Buff = new BlueprintBuffReference
                            {
                                deserializedGuid = BlueprintGuid.Parse("1b7290523aad4b9f8de5310fd873b000")
                            },
                            Permanent = true,
                            UseDurationSeconds = false,
                            DurationValue = new ContextDurationValue
                            {
                                Rate = DurationRate.Rounds,
                                DiceType = DiceType.Zero,
                                DiceCountValue = new ContextValue
                                {
                                    ValueType = ContextValueType.Simple,
                                    Value = 0
                                },
                                BonusValue = new ContextValue
                                {
                                    ValueType = ContextValueType.Simple,
                                    Value = 0
                                },
                                m_IsExtendable = false
                            },
                            DurationSeconds = 0f,
                            IsFromSpell = false,
                            IsNotDispelable = true,
                            ToCaster = false,
                            AsChild = true,
                            SameDuration = true,
                            NotLinkToAreaEffect = false
                        },
                        new Conditional
                        {
                            name = "$Conditional$3dc345b8-d15e-4769-ae22-3338a62921ec",
                            ConditionsChecker = new ConditionsChecker
                            {
                                Operation = Operation.And,
                                Conditions = new Condition[]
                                {
                                    new ContextConditionHasFact
                                    {
                                        name = "$ContextConditionHasFact$d65fd18c-e95f-4d44-848d-4548d2b4b134",
                                        Not = false,
                                        m_Fact = new BlueprintUnitFactReference
                                        {
                                            deserializedGuid = BlueprintGuid.Parse("8e8a34c754d649aa9286fe8ee5cc3f10")
                                        }
                                    }
                                }
                            },
                            IfTrue = new ActionList
                            {
                                Actions = new GameAction[]
                                {
                                    new ContextActionApplyBuff
                                    {
                                        name = "$ContextActionApplyBuff$bc529b88-7896-4851-a6e6-805a9f9d5700",
                                        m_Buff = new BlueprintBuffReference
                                        {
                                            deserializedGuid = BlueprintGuid.Parse("1a5a2ce6793a4458957f45517662bb0e")
                                        },
                                        Permanent = true,
                                        UseDurationSeconds = false,
                                        DurationValue = new ContextDurationValue
                                        {
                                            Rate = DurationRate.Rounds,
                                            DiceType = DiceType.Zero,
                                            DiceCountValue = new ContextValue
                                            {
                                                ValueType = ContextValueType.Simple,
                                                Value = 0
                                            },
                                            BonusValue = new ContextValue
                                            {
                                                ValueType = ContextValueType.Simple,
                                                Value = 0
                                            },
                                            m_IsExtendable = true
                                        },
                                        DurationSeconds = 0f,
                                        IsFromSpell = false,
                                        IsNotDispelable = false,
                                        ToCaster = false,
                                        AsChild = true,
                                        SameDuration = false,
                                        NotLinkToAreaEffect = false
                                    }
                                }
                            },
                            IfFalse = new ActionList()
                        },
                        new ContextActionApplyBuff
                        {
                            name = "$ContextActionApplyBuff$536ca09f-b53e-4f44-b818-0814ee5bf7c0",
                            m_Buff = new BlueprintBuffReference
                            {
                                deserializedGuid = BlueprintGuid.Parse("0aa98aab4aea4665a30327f80fd28bb5")
                            },
                            Permanent = true,
                            UseDurationSeconds = false,
                            DurationValue = new ContextDurationValue
                            {
                                Rate = DurationRate.Rounds,
                                DiceType = DiceType.Zero,
                                DiceCountValue = new ContextValue
                                {
                                    ValueType = ContextValueType.Simple,
                                    Value = 0
                                },
                                BonusValue = new ContextValue
                                {
                                    ValueType = ContextValueType.Simple,
                                    Value = 0
                                },
                                m_IsExtendable = true
                            },
                            DurationSeconds = 0f,
                            IsFromSpell = false,
                            IsNotDispelable = false,
                            ToCaster = false,
                            AsChild = true,
                            SameDuration = false,
                            NotLinkToAreaEffect = false
                        }
                    }
                };

                var deactivatedActions = new ActionList
                {
                    Actions = new GameAction[]
                    {
                        new ContextActionRemoveBuff
                        {
                            name = "$ContextActionRemoveBuff$198f8588-4b18-48eb-a064-b4b1ada9f5b4",
                            m_Buff = new BlueprintBuffReference
                            {
                                deserializedGuid = BlueprintGuid.Parse("1a5a2ce6793a4458957f45517662bb0e")
                            },
                            RemoveRank = false,
                            ToCaster = false,
                            OnlyFromCaster = false
                        },
                        new ContextActionRemoveBuff
                        {
                            name = "$ContextActionRemoveBuff$66193162-6a7a-499c-8da0-a32917ab0d97",
                            m_Buff = new BlueprintBuffReference
                            {
                                deserializedGuid = BlueprintGuid.Parse("0aa98aab4aea4665a30327f80fd28bb5")
                            },
                            RemoveRank = false,
                            ToCaster = false,
                            OnlyFromCaster = false
                        }
                    }
                };

                var addFactContextActions = new AddFactContextActions
                {
                    name = "$AddFactContextActions$12e4fd95-775f-4e29-b7c3-ac9dcbbcdae3",
                    Activated = activatedActions,
                    Deactivated = deactivatedActions,
                    NewRound = new ActionList()
                };

                // 10) ReplaceAbilityParamsWithContext
                var replaceAbilityParams = new ReplaceAbilityParamsWithContext
                {
                    name = "$ReplaceAbilityParamsWithContext$6b4fe5e0-ee06-4a12-8909-5e1d53d3f4ef",
                    m_Ability = new BlueprintAbilityReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("741abfb8c40e43619d74d46ee684312c")
                    }
                };

                // 11) AddDamageResistanceEnergy
                var drEnergy = new AddDamageResistanceEnergy
                {
                    name = "$AddDamageResistanceEnergy$a2a3de6f-1c5e-42df-a190-1a350f1b4fb0",
                    Value = new ContextValue
                    {
                        ValueType = ContextValueType.Shared,
                        Value = 20,
                        ValueRank = AbilityRankType.Default,
                        ValueShared = AbilitySharedValue.Damage
                    },
                    UsePool = false,
                    Pool = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 12
                    },
                    Type = DamageEnergyType.Fire,
                    UseValueMultiplier = false,
                    ValueMultiplier = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    },
                    HealOnDamage = false,
                    HealRate = AddEnergyDamageImmunity.HealingRate.DamageAsIs
                };

                // 12) ContextCalculateSharedValue
                var sharedValue = new ContextCalculateSharedValue
                {
                    name = "$ContextCalculateSharedValue$f040b5ea-7caa-4adc-b48e-ea4b646a1337",
                    ValueType = AbilitySharedValue.Damage,
                    Value = new ContextDiceValue
                    {
                        DiceType = DiceType.One,
                        DiceCountValue = new ContextValue
                        {
                            ValueType = ContextValueType.Rank,
                            ValueRank = AbilityRankType.Default
                        },
                        BonusValue = new ContextValue
                        {
                            ValueType = ContextValueType.Simple,
                            Value = 20
                        }
                    },
                    Modifier = 1.0f
                };

                // 13) ContextRankConfig (CasterBuffRank)
                var rankConfig = new ContextRankConfig
                {
                    name = "$ContextRankConfig$14262e99-5c4d-4013-9b5d-eee47d16ad80",
                    m_Type = AbilityRankType.Default,
                    m_BaseValueType = ContextRankBaseValueType.CasterBuffRank,
                    m_Buff = new BlueprintBuffReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("5d2155c13f9842b2be8196edc82ef057")
                    },
                    m_BuffRankMultiplier = 5,
                    m_Progression = ContextRankProgression.AsIs,
                    m_Max = 20
                };

                // 14) SuppressBuffs
                var suppressBuffs = new SuppressBuffs
                {
                    name = "$SuppressBuffs$569a3d0f-0814-4c7b-8404-86f6d5cfb39d",
                    m_Buffs = new[]
                    {
                        new BlueprintBuffReference
                        {
                            deserializedGuid = BlueprintGuid.Parse("b09147e9b63b49b89c90361fbad90a68")
                        }
                    },
                    Schools = Array.Empty<SpellSchool>(),
                    Descriptor = new SpellDescriptorWrapper { m_IntValue = 0 }
                };

                // Attach all components                
                bp.AddComponent(polymorph);
                bp.AddComponent(spellDescriptor);
                bp.AddComponent(replaceAsks);
                bp.AddComponent(moveSpeed);
                bp.AddComponent(replaceSourceBone);
                bp.AddComponent(replaceCastSource);
                bp.AddComponent(blindsense);
                bp.AddComponent(naturalSpell);
                bp.AddComponent(addFactContextActions);
                bp.AddComponent(replaceAbilityParams);
                bp.AddComponent(drEnergy);
                bp.AddComponent(sharedValue);
                bp.AddComponent(rankConfig);
                bp.AddComponent(suppressBuffs);

            });
            var ability = Helpers.CreateBlueprint<BlueprintAbility>(IsekaiContext, "DragonBloodlineGoldFormMediumAbility", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(IsekaiContext, "DragonBloodlineGoldFormMediumAbility.Name", "Dragon Form (Medium)");

                bp.m_DefaultAiAction = null;
                bp.m_AutoUseIsForbidden = true;
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.Personal;
                bp.IgnoreMinimalRangeLimit = false;
                bp.CustomRange = new Feet(0f);
                bp.ShowNameForVariant = false;
                bp.OnlyForAllyCaster = false;
                bp.CanTargetPoint = false;
                bp.CanTargetEnemies = false;
                bp.CanTargetFriends = false;
                bp.CanTargetSelf = true;
                bp.ShouldTurnToTarget = true;
                bp.SpellResistance = false;
                bp.IgnoreSpellResistanceForAlly = false;
                bp.ActionBarAutoFillIgnored = false;
                bp.Hidden = false;
                bp.NeedEquipWeapons = false;
                bp.NotOffensive = false;
                bp.EffectOnAlly = AbilityEffectOnUnit.None;
                bp.EffectOnEnemy = AbilityEffectOnUnit.None;
                bp.m_Parent = null;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.SelfTouch;
                bp.HasFastAnimation = false;
                bp.m_TargetMapObjects = false;
                bp.ActionType = UnitCommand.CommandType.Standard;
                bp.AvailableMetamagic = Metamagic.Quicken | Metamagic.Extend | Metamagic.Heighten;
                bp.m_IsFullRoundAction = false;

                bp.LocalizedDuration = new LocalizedString();
                bp.LocalizedSavingThrow = new LocalizedString();

                bp.DisableLog = false;
                bp.ResourceAssetIds = Array.Empty<string>();

                // === Components ===

                var spellDescriptorComp = new SpellDescriptorComponent
                {
                    Descriptor = new SpellDescriptorWrapper
                    {
                        m_IntValue = 8589934592L
                    }
                };

                var applyBuffPermanent = new ContextActionApplyBuff
                {
                    // Dragon shifter form gold
                    m_Buff = buff.ToReference<BlueprintBuffReference>(),
                    Permanent = true,
                    UseDurationSeconds = false,
                    DurationValue = new ContextDurationValue
                    {
                        Rate = DurationRate.Minutes,
                        DiceType = DiceType.Zero,
                        DiceCountValue = new ContextValue
                        {
                            ValueType = ContextValueType.Simple,
                            Value = 0,
                            ValueRank = AbilityRankType.Default,
                            ValueShared = AbilitySharedValue.Damage
                        },
                        BonusValue = new ContextValue
                        {
                            ValueType = ContextValueType.Simple,
                            Value = 1,
                            ValueRank = AbilityRankType.Default,
                            ValueShared = AbilitySharedValue.Damage
                        },
                        m_IsExtendable = true
                    },
                    DurationSeconds = 0f,
                    IsFromSpell = false,
                    IsNotDispelable = false,
                    ToCaster = true,
                    AsChild = false,
                    SameDuration = false,
                    NotLinkToAreaEffect = false
                };

                bp.AddComponent<AbilityExecuteActionOnCast>(c =>
                {
                    c.Actions = new ActionList
                    {
                        Actions = new GameAction[]
                        {
                            new ContextActionRemoveBuff
                            {
                                m_Buff = new BlueprintBuffReference
                                {
                                    deserializedGuid = BlueprintGuid.Parse("c3365d5a75294b9b879c587668620bd4")
                                },
                                RemoveRank = false,
                                ToCaster = true,
                                OnlyFromCaster = false
                            }
                        }
                    };
                });
            });

            var feature = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, "DragonBloodlineGoldFormMediumFeature", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(IsekaiContext, "DragonBloodlineGoldFormMediumFeature.Name", "Dragon Form (Medium)");

                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddComponent<AddFacts>(c =>
                {
                    c.m_Facts = new BlueprintUnitFactReference[]
                    {
                        ability.ToReference<BlueprintUnitFactReference>()
                    };
                });
            });
            
            return feature;
        }
    }
}
