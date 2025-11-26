using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
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
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
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
                    c.m_BaseValueType = ContextRankBaseValueType.SummClassLevelWithArchetype;
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
                    c.m_BaseValueType = ContextRankBaseValueType.SummClassLevelWithArchetype;
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
                    c.m_BaseValueType = ContextRankBaseValueType.SummClassLevelWithArchetype;
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
                    c.m_BaseValueType = ContextRankBaseValueType.SummClassLevelWithArchetype;
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

        private static BlueprintAbility GetMediumForm()
        {
            throw new NotImplementedException();
            var mediumAbility = Helpers.CreateBlueprint<BlueprintAbility>(IsekaiContext, "DragonBloodlineGoldFormMediumAbility", ability =>
            {
                ability.m_DisplayName = Helpers.CreateString(IsekaiContext, "DragonBloodlineGoldFormMediumAbility.Name", "Dragon Form (Medium)");

                ability.m_DefaultAiAction = null;
                ability.m_AutoUseIsForbidden = true;
                ability.Type = AbilityType.Supernatural;
                ability.Range = AbilityRange.Personal;
                ability.IgnoreMinimalRangeLimit = false;
                ability.CustomRange = new Feet(0f);
                ability.ShowNameForVariant = false;
                ability.OnlyForAllyCaster = false;
                ability.CanTargetPoint = false;
                ability.CanTargetEnemies = false;
                ability.CanTargetFriends = false;
                ability.CanTargetSelf = true;
                ability.ShouldTurnToTarget = true;
                ability.SpellResistance = false;
                ability.IgnoreSpellResistanceForAlly = false;
                ability.ActionBarAutoFillIgnored = false;
                ability.Hidden = false;
                ability.NeedEquipWeapons = false;
                ability.NotOffensive = false;
                ability.EffectOnAlly = AbilityEffectOnUnit.None;
                ability.EffectOnEnemy = AbilityEffectOnUnit.None;
                ability.m_Parent = null;
                ability.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.SelfTouch;
                ability.HasFastAnimation = false;
                ability.m_TargetMapObjects = false;
                ability.ActionType = UnitCommand.CommandType.Standard;
                ability.AvailableMetamagic = Metamagic.Quicken | Metamagic.Extend | Metamagic.Heighten;
                ability.m_IsFullRoundAction = false;

                ability.LocalizedDuration = new LocalizedString();
                ability.LocalizedSavingThrow = new LocalizedString();

                ability.DisableLog = false;
                ability.ResourceAssetIds = Array.Empty<string>();

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
                    m_Buff = new BlueprintBuffReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("f5ac253cbee44744a7399f17765160d5")
                    },
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

                //    ability.AddComponent(new ContextActionRemoveBuff
                //    {
                //        m_Buff = new BlueprintBuffReference
                //        {
                //            deserializedGuid = BlueprintGuid.Parse("c3365d5a75294b9b879c587668620bd4")
                //        },
                //        RemoveRank = false,
                //        ToCaster = true,
                //        OnlyFromCaster = false
                //    });
                //});
                //return ability;
            });
        }
    }
}
