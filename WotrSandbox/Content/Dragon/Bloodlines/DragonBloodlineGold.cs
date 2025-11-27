using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Root.Fx;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.Localization;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using System;
using System.Linq;
using TabletopTweaks.Core.Utilities;
using static WotrSandbox.Main;

namespace WotrSandbox.Content.Dragon.Bloodlines
{
    public class DragonBloodlineGold
    {
        private const string shifterFormMediumId = "f5ac253cbee44744a7399f17765160d5";
        private const string shifterFormLargeId = "5a679cd137d64c629995c626616dbb17";
        private const string shifterFormHugeId = "833873205d9b46e99217d02cd04a20d4";
        private const string sorcererDragonWingsFeatureId = "6929bac6c67ae194c8c8446e3d593953"; // Gold Dragon Wings
        private const string energyImmunityId = "11ac3433adfa74642a93111624376070"; // Cold
        private const string energyVulnerabilityId = "b8bbe8f713da9ad44a899aa551ca6b5b"; // Fire
        private const string bloodlineName = "Gold";
        private const DamageEnergyType element = DamageEnergyType.Fire;
        private const string primaryBreathProjectileId = "52c3a84f628ddde4dbfb38e4a581b01a"; // FireCone30Feet00Breath

        public void Add()
        {
            var dragonWings = BlueprintTools.GetBlueprint<BlueprintFeature>(sorcererDragonWingsFeatureId);
            var energyVulnerability = !string.IsNullOrEmpty(energyVulnerabilityId) ? BlueprintTools.GetBlueprint<BlueprintFeature>(energyVulnerabilityId) : null;
            var energyImmunity = BlueprintTools.GetBlueprint<BlueprintFeature>(energyImmunityId);

            var breathCooldownBuff = Helpers.CreateBlueprint<BlueprintBuff>(IsekaiContext, $"DragonBloodline{bloodlineName}BreathCooldown", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(IsekaiContext, $"DragonBloodline{bloodlineName}BreathCooldown.Name", "Fire Breath Cooldown");
                bp.m_Flags = BlueprintBuff.Flags.IsFromSpell;
                bp.Frequency = DurationRate.Rounds;
                bp.Stacking = StackingType.Replace;
            });

            var breathFeature = GetPrimaryBreath(breathCooldownBuff);
            var secondaryBreathFeature = GetSecondaryBreath(breathCooldownBuff);

            var formProtoWyrmlingFeature = GetProtoWyrmlingForm(null);
            var formWyrmlingFeature = GetWyrmlingFormFeature(formProtoWyrmlingFeature.ToReference<BlueprintUnitFactReference>());
            var formVeryYoungFeature = GetVeryYoungFormFeature(formWyrmlingFeature.ToReference<BlueprintUnitFactReference>());
            var formYoungFeature = GetYoungFormFeature(formVeryYoungFeature.ToReference<BlueprintUnitFactReference>());
            var formYoungAdultFeature = GetYoungAdultFormFeature(formYoungFeature.ToReference<BlueprintUnitFactReference>());
            var formJuvenileFeature = GetJuvenileFormFeature(formYoungFeature.ToReference<BlueprintUnitFactReference>());
            var formAdultFeature = GetAdultFormFeature(formYoungAdultFeature.ToReference<BlueprintUnitFactReference>());
            var formMatureAdultFeature = GetMatureAdultFormFeature(formAdultFeature.ToReference<BlueprintUnitFactReference>());
            var formOldFeature = GetOldFormFeature(formMatureAdultFeature.ToReference<BlueprintUnitFactReference>());
            var formVeryOldFeature = GetVeryOldFormFeature(formOldFeature.ToReference<BlueprintUnitFactReference>());
            var formAncientFeature = GetAncientFormFeature(formVeryOldFeature.ToReference<BlueprintUnitFactReference>());
            var formWyrmFeature = GetWyrmFormFeature(formAncientFeature.ToReference<BlueprintUnitFactReference>());
            var formGreatWyrmFeature = GetGreatWyrmFormFeature(formWyrmFeature.ToReference<BlueprintUnitFactReference>());

            var bonusSpellsYoung = GetYoungBonusSpells();
            var bonusSpellsJuveneile = GetJuvenileBonusSpells();
            var bonusSpellsYoungAdult = GetYoungAdultBonusSpells();
            var bonusSpellsAdult = GetAdultBonusSpells();
            var bonusSpellsMatureAdult = GetMatureAdultBonusSpells();
            var bonusSpellsOld = GetOldBonusSpells();
            var bonusSpellsVeryOld = GetVeryOldBonusSpells();
            var bonusSpellsAncient = GetAncientBonusSpells();
            var bonusSpellsWyrm = GetWyrmBonusSpells();
            var bonusSpellsGreatWyrm = GetGreatWyrmBonusSpells();

            // Not quite rules as written for a gold dragon but close enough
            var azataDragonFrightfulPresence = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("a2e0cbebe3bb4a90a22b75d3c22d952c");

            var blessAbility = GetBlessAtWillFeature();
            var sunBurstAbility = GetSunburstAtWillFeature();

            var bloodline = Helpers.CreateBlueprint<BlueprintProgression>(IsekaiContext, $"DragonBloodline{bloodlineName}", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(IsekaiContext, $"DragonBloodline{bloodlineName}.Name", $"{bloodlineName}");
                bp.m_Description = Helpers.CreateString(IsekaiContext, $"DragonBloodline{bloodlineName}.Description", $"{bloodlineName} Dragon");
                bp.m_DescriptionShort = Helpers.CreateString(IsekaiContext, $"DragonBloodline{bloodlineName}.DescriptionShort", "");
                bp.LevelEntries = new LevelEntry[30]
                {
                    energyVulnerability != null ?
                        Helpers.CreateLevelEntry(1, dragonWings, energyImmunity, energyVulnerability)
                        : Helpers.CreateLevelEntry(1, dragonWings, energyImmunity),
                    Helpers.CreateLevelEntry(2, secondaryBreathFeature),
                    Helpers.CreateLevelEntry(3, breathFeature),
                    Helpers.CreateLevelEntry(4, formProtoWyrmlingFeature),
                    Helpers.CreateLevelEntry(5, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(6, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(7, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(8, formWyrmlingFeature),
                    Helpers.CreateLevelEntry(9, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(10, formVeryYoungFeature),
                    Helpers.CreateLevelEntry(11, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(12, formYoungFeature, bonusSpellsYoung),
                    Helpers.CreateLevelEntry(13, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(14, formJuvenileFeature, bonusSpellsJuveneile, blessAbility),
                    Helpers.CreateLevelEntry(15, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(16, formYoungAdultFeature, bonusSpellsYoungAdult),
                    Helpers.CreateLevelEntry(17, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(18, formAdultFeature, bonusSpellsAdult, azataDragonFrightfulPresence),
                    Helpers.CreateLevelEntry(19, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(20, formMatureAdultFeature, bonusSpellsMatureAdult),
                    Helpers.CreateLevelEntry(21, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(22, formOldFeature, bonusSpellsOld),
                    Helpers.CreateLevelEntry(23, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(24, formVeryYoungFeature, bonusSpellsVeryOld),
                    Helpers.CreateLevelEntry(25, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(26, formAncientFeature, bonusSpellsAncient, sunBurstAbility),
                    Helpers.CreateLevelEntry(27, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(28, formWyrmFeature, bonusSpellsWyrm),
                    Helpers.CreateLevelEntry(29, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(30, formGreatWyrmFeature, bonusSpellsGreatWyrm)
                };
                bp.UIGroups = new UIGroup[]
                {
                    Helpers.CreateUIGroup(dragonWings, energyImmunity, energyVulnerability),
                    Helpers.CreateUIGroup(breathFeature, secondaryBreathFeature),
                    Helpers.CreateUIGroup(formProtoWyrmlingFeature, formWyrmlingFeature, formVeryYoungFeature, formYoungFeature, formJuvenileFeature, formYoungAdultFeature, formAdultFeature, formMatureAdultFeature, formOldFeature, formVeryOldFeature, formAncientFeature, formWyrmFeature, formGreatWyrmFeature),
                    Helpers.CreateUIGroup(bonusSpellsYoung, bonusSpellsYoungAdult, bonusSpellsAdult, bonusSpellsMatureAdult, bonusSpellsOld, bonusSpellsVeryOld, bonusSpellsAncient, bonusSpellsWyrm, bonusSpellsGreatWyrm),
                };
                bp.m_UIDeterminatorsGroup = new BlueprintFeatureBaseReference[]
                {
                };
            });
        }

        private BlueprintFeature GetPrimaryBreath(BlueprintBuff breathCooldownBuff)
        {
            var breathAbility = Helpers.CreateBlueprint<BlueprintAbility>(IsekaiContext, $"DragonBloodline{bloodlineName}BreathAbility", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(IsekaiContext, $"DragonBloodline{bloodlineName}BreathAbility.Name", $"{element:F} Breath");
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
                            },
                        }
                    };
                });

                bp.AddComponent<AbilityEffectRunAction>(c =>
                {
                    c.SavingThrowType = SavingThrowType.Reflex;
                    c.Actions = new ActionList
                    {
                        Actions = new GameAction[]
                        {
                            new ContextActionDealDamage
                            {
                                m_Type = ContextActionDealDamage.Type.Damage,
                                DamageType = new DamageTypeDescription
                                {
                                    Type = DamageType.Energy,
                                    Energy = element
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
                    c.m_Projectiles = new[] { BlueprintTools.GetBlueprintReference<BlueprintProjectileReference>(primaryBreathProjectileId) };
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
            var breathAbilityReference = BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(IsekaiContext, $"DragonBloodline{bloodlineName}BreathAbility");
            var breathAbilityReferenceUnit = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(IsekaiContext, $"DragonBloodline{bloodlineName}BreathAbility");

            return Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, $"DragonBloodline{bloodlineName}BreathFeature", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(IsekaiContext, $"DragonBloodline{bloodlineName}BreathFeature.Name", $"{element:F} Breath");

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

        private BlueprintFeature GetSecondaryBreath(BlueprintBuff breathCooldownBuff)
        {
            var breathAbility = Helpers.CreateBlueprint<BlueprintAbility>(IsekaiContext, $"DragonBloodline{bloodlineName}SeconaryBreathAbility", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(IsekaiContext, $"DragonBloodline{bloodlineName}SeconaryBreathAbility.Name", "Weakening Breath");
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
                    // We handle the saves manually inside, so this is cosmetic / UI only
                    c.SavingThrowType = SavingThrowType.Unknown;

                    c.Actions = new ActionList
                    {
                        Actions = new GameAction[]
                        {
                            // First: Fortitude save, negates Strength damage
                            new ContextActionSavingThrow
                            {
                                Type = SavingThrowType.Fortitude,
                                Actions = new ActionList
                                {
                                    Actions = new GameAction[]
                                    {
                                        // Branch on Fort success/fail
                                        new ContextActionConditionalSaved
                                        {
                                            // Fortitude SUCCESS: do nothing
                                            Succeed = new ActionList
                                            {
                                                Actions = Array.Empty<GameAction>()
                                            },

                                            // Fortitude FAIL: then roll a Will save to halve the Strength damage
                                            Failed = new ActionList
                                            {
                                                Actions = new GameAction[]
                                                {
                                                    new ContextActionSavingThrow
                                                    {
                                                        Type = SavingThrowType.Will,
                                                        Actions = new ActionList
                                                        {
                                                            Actions = new GameAction[]
                                                            {
                                                                new ContextActionDealDamage
                                                                {
                                                                    // Ability damage to Strength
                                                                    m_Type = ContextActionDealDamage.Type.AbilityDamage,
                                                                    Drain = false,
                                                                    AbilityType = StatType.Strength,

                                                                    // 1 point per "age category" → you decided = 1/2 class level
                                                                    // So we just use rank with Div2 progression.
                                                                    Value = new ContextDiceValue
                                                                    {
                                                                        DiceType = DiceType.Zero,
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
                                                                    HalfIfSaved = true   // Will save halves this damage
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
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
            var breathAbilityReference = BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(IsekaiContext, $"DragonBloodline{bloodlineName}SeconaryBreathAbility");
            var breathAbilityReferenceUnit = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(IsekaiContext, $"DragonBloodline{bloodlineName}SeconaryBreathAbility");

            return Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, $"DragonBloodline{bloodlineName}SecondaryBreathFeature", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(IsekaiContext, $"DragonBloodline{bloodlineName}SeconaryBreathAbility.Name", "Weakening Breath");

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

        private BlueprintFeature GetProtoWyrmlingForm(BlueprintUnitFactReference previousStageFeature)
        {
            return GetTinyFormFeature("ProtoWyrmling", previousStageFeature, c =>
            {
                c.StrengthBonus = 0;
                c.DexterityBonus = 4;
                c.ConstitutionBonus = 0;
                c.NaturalArmor = 2;
            }, bp =>
            {

            });
        }

        private BlueprintFeature GetWyrmlingFormFeature(BlueprintUnitFactReference previousStageFeature)
        {
            return GetSmallFormFeature("Wyrmling", previousStageFeature, c =>
            {
                c.StrengthBonus = 2;
                c.DexterityBonus = 2;
                c.ConstitutionBonus = 2;
                c.NaturalArmor = 3;
            }, bp =>
            {

            });
        }

        private BlueprintFeature GetVeryYoungFormFeature(BlueprintUnitFactReference previousStageFeature)
        {
            return GetMediumFormFeature("VeryYoung", previousStageFeature, c =>
            {
                c.StrengthBonus = 4;
                c.DexterityBonus = 0;
                c.ConstitutionBonus = 2;
                c.NaturalArmor = 6;
            }, bp =>
            {

            });
        }

        private BlueprintFeature GetYoungFormFeature(BlueprintUnitFactReference previousStageFeature)
        {
            return GetLargeFormFeature("Young", previousStageFeature, c =>
            {
                c.StrengthBonus = 6;
                c.DexterityBonus = -2;
                c.ConstitutionBonus = 4;
                c.NaturalArmor = 9;
            }, bp =>
            {

            });
        }

        private BlueprintFeature GetJuvenileFormFeature(BlueprintUnitFactReference previousStageFeature)
        {
            return GetLargeFormFeature("Juvenile", previousStageFeature, c =>
            {
                c.StrengthBonus = 6;
                c.DexterityBonus = -2;
                c.ConstitutionBonus = 4;
                c.NaturalArmor = 9;
            }, bp =>
            {

            });
        }

        private BlueprintFeature GetYoungAdultFormFeature(BlueprintUnitFactReference previousStageFeature)
        {
            return GetHugeFormFeature("YoungAdult", previousStageFeature, c =>
            {
                c.StrengthBonus = 8;
                c.DexterityBonus = -4;
                c.ConstitutionBonus = 4;
                c.NaturalArmor = 15;
            }, bp =>
            {

                bp.AddComponent<AddDamageResistancePhysical>(c =>
                {
                    c.Value = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 5
                    };
                    c.BypassedByMagic = true;
                });
                bp.AddComponent<AddSpellResistance>(c =>
                {
                    c.Value = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 25
                    };
                });
            });
        }

        private BlueprintFeature GetAdultFormFeature(BlueprintUnitFactReference previousStageFeature)
        {
            return GetHugeFormFeature("Adult", previousStageFeature, c =>
            {
                c.StrengthBonus = 8;
                c.DexterityBonus = -4;
                c.ConstitutionBonus = 4;
                c.NaturalArmor = 15;
            }, bp =>
            {

                bp.AddComponent<AddDamageResistancePhysical>(c =>
                {
                    c.Value = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 5
                    };
                    c.BypassedByMagic = true;
                });
                bp.AddComponent<AddSpellResistance>(c =>
                {
                    c.Value = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 25
                    };
                });
            });
        }

        private BlueprintFeature GetMatureAdultFormFeature(BlueprintUnitFactReference previousStageFeature)
        {
            return GetHugeFormFeature("MatureAdult", previousStageFeature, c =>
            {
                c.StrengthBonus = 8;
                c.DexterityBonus = -4;
                c.ConstitutionBonus = 4;
                c.NaturalArmor = 15;
            }, bp =>
            {

                bp.AddComponent<AddDamageResistancePhysical>(c =>
                {
                    c.Value = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 5
                    };
                    c.BypassedByMagic = true;
                });
                bp.AddComponent<AddSpellResistance>(c =>
                {
                    c.Value = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 25
                    };
                });
            });
        }

        private BlueprintFeature GetOldFormFeature(BlueprintUnitFactReference previousStageFeature)
        {
            return GetGargantuanFormFeature("Old", previousStageFeature, c =>
            {
                c.StrengthBonus = 10;
                c.DexterityBonus = -6;
                c.ConstitutionBonus = 6;
                c.NaturalArmor = 21;
            }, bp =>
            {

                bp.AddComponent<AddDamageResistancePhysical>(c =>
                {
                    c.Value = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 10
                    };
                    c.BypassedByMagic = true;
                });
                bp.AddComponent<AddSpellResistance>(c =>
                {
                    c.Value = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 29
                    };
                });
            });
        }

        private BlueprintFeature GetVeryOldFormFeature(BlueprintUnitFactReference previousStageFeature)
        {
            return GetGargantuanFormFeature("VeryOld", previousStageFeature, c =>
            {
                c.StrengthBonus = 10;
                c.DexterityBonus = -6;
                c.ConstitutionBonus = 6;
                c.NaturalArmor = 24;
            }, bp =>
            {

                bp.AddComponent<AddDamageResistancePhysical>(c =>
                {
                    c.Value = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 15
                    };
                    c.BypassedByMagic = true;
                });
                bp.AddComponent<AddSpellResistance>(c =>
                {
                    c.Value = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 30
                    };
                });
            });
        }

        private BlueprintFeature GetAncientFormFeature(BlueprintUnitFactReference previousStageFeature)
        {
            return GetGargantuanFormFeature("Ancient", previousStageFeature, c =>
            {
                c.StrengthBonus = 10;
                c.DexterityBonus = -6;
                c.ConstitutionBonus = 6;
                c.NaturalArmor = 30;
            }, bp =>
            {

                bp.AddComponent<AddDamageResistancePhysical>(c =>
                {
                    c.Value = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 15
                    };
                    c.BypassedByMagic = true;
                });
                bp.AddComponent<AddSpellResistance>(c =>
                {
                    c.Value = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 31
                    };
                });
            });
        }

        private BlueprintFeature GetWyrmFormFeature(BlueprintUnitFactReference previousStageFeature)
        {
            return GetGargantuanFormFeature("Wyrm", previousStageFeature, c =>
            {
                c.StrengthBonus = 10;
                c.DexterityBonus = -6;
                c.ConstitutionBonus = 6;
                c.NaturalArmor = 33;
            }, bp =>
            {
                bp.AddComponent<AddDamageResistancePhysical>(c =>
                {
                    c.Value = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 20
                    };
                    c.BypassedByMagic = true;
                });
                bp.AddComponent<AddSpellResistance>(c =>
                {
                    c.Value = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 32
                    };
                });
            });
        }

        private BlueprintFeature GetGreatWyrmFormFeature(BlueprintUnitFactReference previousStageFeature)
        {
            return GetCollossalFormFeature("GreatWyrm", previousStageFeature, c =>
            {
                c.StrengthBonus = 12;
                c.DexterityBonus = -8;
                c.ConstitutionBonus = 8;
                c.NaturalArmor = 36;
            }, bp =>
            {
                bp.AddComponent<AddDamageResistancePhysical>(c =>
                {
                    c.Value = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 20
                    };
                    c.BypassedByMagic = true;
                });
                bp.AddComponent<AddSpellResistance>(c =>
                {
                    c.Value = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 34
                    };
                });
            });
        }

        private BlueprintFeature GetTinyFormFeature(string age, BlueprintUnitFactReference previousStageFeature, Action<Polymorph> polymorphSettings, Action<BlueprintBuff> buffSettings = null)
        {
            var shifterFormBuff = BlueprintTools.GetBlueprint<BlueprintBuff>(shifterFormMediumId);
            var shifterFormBuffPolymorph = (Polymorph)(shifterFormBuff.Components.Single(c => c is Polymorph));

            return GetFormFeature(Size.Tiny, age, c =>
            {
                c.m_Prefab = shifterFormBuffPolymorph.m_Prefab;
                c.m_PrefabFemale = shifterFormBuffPolymorph.m_PrefabFemale;
                c.m_Portrait = shifterFormBuffPolymorph.m_Portrait;
                c.m_EnterTransition = shifterFormBuffPolymorph.m_EnterTransition;
                c.m_ExitTransition = shifterFormBuffPolymorph.m_ExitTransition;
                c.m_TransitionExternal = shifterFormBuffPolymorph.m_TransitionExternal;

                c.m_AdditionalLimbs = new[]
                {
                    // Bite 1d4
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("35dfad6517f401145af54111be04d6cf")
                    },
                    // Claw 1d3
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("800092a2b9a743b48ae8aeeb5d243dcc")
                    },
                        
                    // Claw 1d3
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("800092a2b9a743b48ae8aeeb5d243dcc")
                    }
                };
                polymorphSettings.Invoke(c);
            }, previousStageFeature, buffSettings);
        }

        private BlueprintFeature GetSmallFormFeature(string age, BlueprintUnitFactReference previousStageFeature, Action<Polymorph> polymorphSettings, Action<BlueprintBuff> buffSettings = null)
        {
            var shifterFormBuff = BlueprintTools.GetBlueprint<BlueprintBuff>(shifterFormMediumId);
            var shifterFormBuffPolymorph = (Polymorph)(shifterFormBuff.Components.Single(c => c is Polymorph));

            return GetFormFeature(Size.Small, age, c =>
            {
                c.m_Prefab = shifterFormBuffPolymorph.m_Prefab;
                c.m_PrefabFemale = shifterFormBuffPolymorph.m_PrefabFemale;
                c.m_Portrait = shifterFormBuffPolymorph.m_Portrait;
                c.m_EnterTransition = shifterFormBuffPolymorph.m_EnterTransition;
                c.m_ExitTransition = shifterFormBuffPolymorph.m_ExitTransition;
                c.m_TransitionExternal = shifterFormBuffPolymorph.m_TransitionExternal;

                c.m_AdditionalLimbs = new[]
                {
                    // Bite 1d6
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("a000716f88c969c499a535dadcf09286")
                    },
                    // Claw 1d4
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("118fdd03e569a66459ab01a20af6811a")
                    },
                        
                    // Claw 1d4
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("118fdd03e569a66459ab01a20af6811a")
                    }
                };
                polymorphSettings.Invoke(c);
            }, previousStageFeature, buffSettings);
        }

        private BlueprintFeature GetMediumFormFeature(string age, BlueprintUnitFactReference previousStageFeature, Action<Polymorph> polymorphSettings, Action<BlueprintBuff> buffSettings = null)
        {
            var shifterFormBuff = BlueprintTools.GetBlueprint<BlueprintBuff>(shifterFormMediumId);
            var shifterFormBuffPolymorph = (Polymorph)(shifterFormBuff.Components.Single(c => c is Polymorph));

            return GetFormFeature(Size.Medium, age, c =>
            {
                c.m_Prefab = shifterFormBuffPolymorph.m_Prefab;
                c.m_PrefabFemale = shifterFormBuffPolymorph.m_PrefabFemale;
                c.m_Portrait = shifterFormBuffPolymorph.m_Portrait;
                c.m_EnterTransition = shifterFormBuffPolymorph.m_EnterTransition;
                c.m_ExitTransition = shifterFormBuffPolymorph.m_ExitTransition;
                c.m_TransitionExternal = shifterFormBuffPolymorph.m_TransitionExternal;

                c.m_AdditionalLimbs = new[]
                {
                    // Bite 1d8
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("61bc14eca5f8c1040900215000cfc218")
                    },
                    // Claw 1d6
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("65eb73689b94d894080d33a768cdf645")
                    },
                        
                    // Claw 1d6
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("65eb73689b94d894080d33a768cdf645")
                    }
                };
                c.m_SecondaryAdditionalLimbs = new[]
                {
                    // Wing 1d4
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("864e29d3e07ad4a4f96d576b366b4a86")
                    },
                    // Wing 1d4
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("864e29d3e07ad4a4f96d576b366b4a86")
                    }
                };
                polymorphSettings.Invoke(c);
            }, previousStageFeature, buffSettings);
        }

        private BlueprintFeature GetLargeFormFeature(string age, BlueprintUnitFactReference previousStageFeature, Action<Polymorph> polymorphSettings, Action<BlueprintBuff> buffSettings = null)
        {
            var shifterFormBuff = BlueprintTools.GetBlueprint<BlueprintBuff>(shifterFormLargeId);
            var shifterFormBuffPolymorph = (Polymorph)(shifterFormBuff.Components.Single(c => c is Polymorph));

            return GetFormFeature(Size.Large, age, c =>
            {
                c.m_Prefab = shifterFormBuffPolymorph.m_Prefab;
                c.m_PrefabFemale = shifterFormBuffPolymorph.m_PrefabFemale;
                c.m_Portrait = shifterFormBuffPolymorph.m_Portrait;
                c.m_EnterTransition = shifterFormBuffPolymorph.m_EnterTransition;
                c.m_ExitTransition = shifterFormBuffPolymorph.m_ExitTransition;
                c.m_TransitionExternal = shifterFormBuffPolymorph.m_TransitionExternal;

                c.m_AdditionalLimbs = new[]
                {
                    // Bite 2d6
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("2abc1dc6172759c42971bd04b8c115cb")
                    },
                    // Claw 1d8
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("13a4ac62fe603fc4c99f9ed5e5d0b9d6")
                    },
                        
                    // Claw 1d8
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("13a4ac62fe603fc4c99f9ed5e5d0b9d6")
                    }
                };
                c.m_SecondaryAdditionalLimbs = new[]
                {
                    // Wing 1d6
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("cdbf5fdd86eb4d238cef15f7835e42c3")
                    },
                    // Wing 1d6
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("cdbf5fdd86eb4d238cef15f7835e42c3")
                    },
                    // Tail Slap 1d8
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("29e50b018da8468c8dcb411148ba6413")
                    }
                };
                polymorphSettings.Invoke(c);
            }, previousStageFeature, buffSettings);
        }

        private BlueprintFeature GetHugeFormFeature(string age, BlueprintUnitFactReference previousStageFeature, Action<Polymorph> polymorphSettings, Action<BlueprintBuff> buffSettings = null)
        {
            var shifterFormBuff = BlueprintTools.GetBlueprint<BlueprintBuff>(shifterFormHugeId);
            var shifterFormBuffPolymorph = (Polymorph)(shifterFormBuff.Components.Single(c => c is Polymorph));

            return GetFormFeature(Size.Huge, age, c =>
            {
                c.m_Prefab = shifterFormBuffPolymorph.m_Prefab;
                c.m_PrefabFemale = shifterFormBuffPolymorph.m_PrefabFemale;
                c.m_Portrait = shifterFormBuffPolymorph.m_Portrait;
                c.m_EnterTransition = shifterFormBuffPolymorph.m_EnterTransition;
                c.m_ExitTransition = shifterFormBuffPolymorph.m_ExitTransition;
                c.m_TransitionExternal = shifterFormBuffPolymorph.m_TransitionExternal;

                c.m_AdditionalLimbs = new[]
                {
                    // Bite 2d6 // Todo: upgrade to 2d8
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("2abc1dc6172759c42971bd04b8c115cb")
                    },
                    // Claw 2d6
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("d498b2af675bd3447a0ab65ccc34d952")
                    },
                        
                    // Claw 2d6
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("d498b2af675bd3447a0ab65ccc34d952")
                    }
                };
                c.m_SecondaryAdditionalLimbs = new[]
                {
                    // Wing 1d6 //todo: upgrade to 1d8
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("cdbf5fdd86eb4d238cef15f7835e42c3")
                    },
                    // Wing 1d6 //todo: upgrade to 1d8
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("cdbf5fdd86eb4d238cef15f7835e42c3")
                    },
                    // Tail Slap 1d8 //todo: upgrade to 2d6
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("29e50b018da8468c8dcb411148ba6413")
                    }
                };
                polymorphSettings.Invoke(c);
            }, previousStageFeature, bp =>
            {

                if (buffSettings != null)
                {
                    buffSettings.Invoke(bp);
                }
            });
        }

        private BlueprintFeature GetGargantuanFormFeature(string age, BlueprintUnitFactReference previousStageFeature, Action<Polymorph> polymorphSettings, Action<BlueprintBuff> buffSettings = null)
        {
            var shifterFormBuff = BlueprintTools.GetBlueprint<BlueprintBuff>(shifterFormHugeId);
            var shifterFormBuffPolymorph = (Polymorph)(shifterFormBuff.Components.Single(c => c is Polymorph));

            return GetFormFeature(Size.Gargantuan, age, c =>
            {
                c.m_Prefab = shifterFormBuffPolymorph.m_Prefab;
                c.m_PrefabFemale = shifterFormBuffPolymorph.m_PrefabFemale;
                c.m_Portrait = shifterFormBuffPolymorph.m_Portrait;
                c.m_EnterTransition = shifterFormBuffPolymorph.m_EnterTransition;
                c.m_ExitTransition = shifterFormBuffPolymorph.m_ExitTransition;
                c.m_TransitionExternal = shifterFormBuffPolymorph.m_TransitionExternal;

                c.m_AdditionalLimbs = new[]
                {
                    // Bite 2d6 // Todo: upgrade to 4d6
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("2abc1dc6172759c42971bd04b8c115cb")
                    },
                    // Claw 2d8
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("bd440fff6bfc3954aac8b6e59a9d7489")
                    },
                        
                    // Claw 2d8
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("bd440fff6bfc3954aac8b6e59a9d7489")
                    }
                };
                c.m_SecondaryAdditionalLimbs = new[]
                {
                    // Wing 1d6 //todo: upgrade to 2d6
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("cdbf5fdd86eb4d238cef15f7835e42c3")
                    },
                    // Wing 1d6 //todo: upgrade to 2d6
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("cdbf5fdd86eb4d238cef15f7835e42c3")
                    },
                    // Tail Slap 1d8 //todo: upgrade to 2d8
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("29e50b018da8468c8dcb411148ba6413")
                    }
                };
                polymorphSettings.Invoke(c);
            }, previousStageFeature, bp =>
            {

                if (buffSettings != null)
                {
                    buffSettings.Invoke(bp);
                }
            });
        }

        private BlueprintFeature GetCollossalFormFeature(string age, BlueprintUnitFactReference previousStageFeature, Action<Polymorph> polymorphSettings, Action<BlueprintBuff> buffSettings = null)
        {
            var shifterFormBuff = BlueprintTools.GetBlueprint<BlueprintBuff>(shifterFormHugeId);
            var shifterFormBuffPolymorph = (Polymorph)(shifterFormBuff.Components.Single(c => c is Polymorph));

            return GetFormFeature(Size.Colossal, age, c =>
            {
                c.m_Prefab = shifterFormBuffPolymorph.m_Prefab;
                c.m_PrefabFemale = shifterFormBuffPolymorph.m_PrefabFemale;
                c.m_Portrait = shifterFormBuffPolymorph.m_Portrait;
                c.m_EnterTransition = shifterFormBuffPolymorph.m_EnterTransition;
                c.m_ExitTransition = shifterFormBuffPolymorph.m_ExitTransition;
                c.m_TransitionExternal = shifterFormBuffPolymorph.m_TransitionExternal;

                c.m_AdditionalLimbs = new[]
                {
                    // Bite 2d6 // Todo: upgrade to 4d8
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("2abc1dc6172759c42971bd04b8c115cb")
                    },
                    // Claw 2d8 // Todo: upgrade to 4d6
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("bd440fff6bfc3954aac8b6e59a9d7489")
                    },
                        
                    // Claw 2d8 // Todo: upgrade to 4d6
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("bd440fff6bfc3954aac8b6e59a9d7489")
                    }
                };
                c.m_SecondaryAdditionalLimbs = new[]
                {
                    // Wing 1d6 //todo: upgrade to 2d8
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("cdbf5fdd86eb4d238cef15f7835e42c3")
                    },
                    // Wing 1d6 //todo: upgrade to 2d8
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("cdbf5fdd86eb4d238cef15f7835e42c3")
                    },
                    // Tail Slap 1d8 //todo: upgrade to 4d6
                    new BlueprintItemWeaponReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("29e50b018da8468c8dcb411148ba6413")
                    }
                };

                polymorphSettings.Invoke(c);
            }, previousStageFeature, bp =>
            {
                if (buffSettings != null)
                {
                    buffSettings.Invoke(bp);
                }
            });
        }

        private BlueprintFeature GetFormFeature(Size size, string age, Action<Polymorph> polymorphSettings, BlueprintUnitFactReference previousStageFeature, Action<BlueprintBuff> buffSettings = null)
        {
            var sizeName = size.ToString("F");
            var mythicDragonFormAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("a0273cfaafe84f0b89a70b3580568ebc");

            var buff = Helpers.CreateBlueprint<BlueprintBuff>(IsekaiContext, $"DragonBloodline{bloodlineName}Form{age}Buff", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(IsekaiContext, $"DragonBloodline{bloodlineName}Form{age}Buff.Name", $"{bloodlineName} Dragon Form ({age})");
                if (age == "ProtoWyrmling")
                {
                    bp.m_Description = Helpers.CreateString(IsekaiContext, $"DragonBloodline{bloodlineName}Form{age}Buff.Description", $"Although you're not a true dragon yet, you can assume a dragon form one size smaller than a wyrmling.");
                }
                else
                {
                    bp.m_Description = Helpers.CreateString(IsekaiContext, $"DragonBloodline{bloodlineName}Form{age}Buff.Description", $"As a {age} dragon, you can assume your true form of a {sizeName} dragon, gaining its physical attributes and natural weapons.");
                }
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

                bp.m_Icon = mythicDragonFormAbility.m_Icon;

                bp.AddComponent<Polymorph>(c =>
                {
                    c.m_Race = null;
                    c.m_SpecialDollType = SpecialDollType.None;
                    c.m_ReplaceUnitForInspection = null;
                    //m_PortraitTypeEntry = UnitEntityData.PortraitType.SmallPortrait,
                    c.m_KeepSlots = false;
                    c.UseSizeAsBaseForDamage = true;

                    c.m_MainHand = null;
                    c.m_OffHand = null;
                    c.AllowDamageTransfer = true;
                    c.m_Facts = new BlueprintUnitFactReference[]
                    {
                        // Turn back ability standard
                        new BlueprintUnitFactReference
                        {
                            deserializedGuid = BlueprintGuid.Parse("bd09b025ee2a82f46afab922c4decca9")
                        },
                    };
                    c.m_SilentCaster = true;

                    c.Size = size;
                    polymorphSettings.Invoke(c);
                });
                bp.AddComponent(new SpellDescriptorComponent
                {
                    Descriptor = new SpellDescriptorWrapper
                    {
                        m_IntValue = 8589934592L
                    }
                });
                bp.AddComponent(new ReplaceAsksList
                {
                    // BlackDragon_Barks
                    m_Asks = new BlueprintUnitAsksListReference
                    {
                        deserializedGuid = BlueprintGuid.Parse("3c0924a80e504f04c94de6ec2a28f9aa")
                    }
                });
                bp.AddComponent(new BuffMovementSpeed
                {
                    Descriptor = ModifierDescriptor.None,
                    Value = 30,
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
                });
                bp.AddComponent(new ReplaceSourceBone
                {
                    SourceBone = "Locator_HeadCenterFX_00"
                });
                bp.AddComponent(new ReplaceCastSource
                {
                    CastSource = CastSource.Head
                });
                bp.AddComponent(new Blindsense
                {
                    Range = 60.Feet(),
                    Blindsight = false,
                    Exceptions = null
                });
                bp.AddComponent(new AddMechanicsFeature
                {
                    m_Feature = AddMechanicsFeature.MechanicsFeatureType.NaturalSpell
                });
                bp.AddComponent<NotDispelable>();

                if (buffSettings != null)
                {
                    buffSettings.Invoke(bp);
                }
            });
            var ability = Helpers.CreateBlueprint<BlueprintAbility>(IsekaiContext, $"DragonBloodline{bloodlineName}Form{age}Ability", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(IsekaiContext, $"DragonBloodline{bloodlineName}Form{age}Ability.Name", $"Dragon Form ({age})");

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


                bp.AddComponent<AbilityEffectRunAction>(c =>
                {
                    c.Actions = new ActionList
                    {
                        Actions = new GameAction[]
                        {
                            new ContextActionApplyBuff
                            {
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
                                IsFromSpell = true,
                                IsNotDispelable = false,
                                ToCaster = true,
                                AsChild = false,
                                SameDuration = false,
                                NotLinkToAreaEffect = false
                            }
                        }
                    };
                });

                bp.AddComponent<SpellDescriptorComponent>(c =>
                {
                    c.Descriptor = new SpellDescriptorWrapper
                    {
                        m_IntValue = 8589934592L
                    };
                });

                bp.AddComponent<AbilityExecuteActionOnCast>(c =>
                {
                    c.Actions = new ActionList
                    {
                        Actions = new GameAction[]
                        {
                            new ContextActionRemoveBuffsByDescriptor
                            {
                                SpellDescriptor = new SpellDescriptorWrapper
                                {
                                    m_IntValue = 8589934592L
                                }
                            }
                        }
                    };
                });
            });

            var feature = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, $"DragonBloodline{bloodlineName}Form{age}Feature", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(IsekaiContext, $"DragonBloodline{bloodlineName}Form{age}Feature.Name", $"Dragon Form ({age})");

                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddComponent<AddFacts>(c =>
                {
                    c.m_Facts = new BlueprintUnitFactReference[]
                    {
                        ability.ToReference<BlueprintUnitFactReference>()
                    };
                });
                if (previousStageFeature != null)
                {
                    bp.AddComponent<RemoveFeatureOnApply>(c =>
                    {
                        c.m_Feature = previousStageFeature;
                    });
                }
            });
            
            return feature;
        }

        private BlueprintFeature GetYoungBonusSpells()
        {
            return GetBonusSpellsFeature("Young",
                new Spell(0, "95f206566c5261c42aa5b3e7e0d1e36c"), // Light (MageLight)
                new Spell(0, "0557ccee0a86dc44cb3d3f6a3b235329"), // Stablize
                new Spell(1, "9e1ad5d6f87d19e4d8883d63a6e35568"), // Mage Armor
                new Spell(1, "ef768022b0785eb43a18969903c537c4") // Shield
            );
        }

        private BlueprintFeature GetJuvenileBonusSpells()
        {
            return GetBonusSpellsFeature("Juvenile",
                new Spell(1, "9d5d2d3ffdd73c648af3eb3e585b1113") // Divine Favor
            );
        }

        private BlueprintFeature GetYoungAdultBonusSpells()
        {
            return GetBonusSpellsFeature("YoungAdult",
                new Spell(1, "183d5bb91dea3a1489a6db6c9cb64445"), // Shield of Faith
                new Spell(2, "1c1ebf5370939a9418da93176cc44cd9"), // Cure Moderate Wounds
                new Spell(2, "21ffef7791ce73f468b6fca4d9371e8b") // Resist Energy
            );
        }

        private BlueprintFeature GetAdultBonusSpells()
        {
            return GetBonusSpellsFeature("Adult",
                new Spell(2, "03a9630394d10164a9410882d31572f0"), // Aid
                new Spell(3, "92681f181b507b34ea87018e8f7a528a"), // Dispel Magic
                new Spell(3, "faabd2cc67efa4646ac58c7bb3e40fcc") // Prayer
            );
        }

        private BlueprintFeature GetMatureAdultBonusSpells()
        {
            return GetBonusSpellsFeature("MatureAdult",
                new Spell(2, "e84fc922ccf952943b5240293669b171"), // Lesser Restoration
                new Spell(2, "30e5dc243f937fc4b95d2f8f4e1b7ff3"), // See Invisibility
                //new Spell(4, "03a9630394d10164a9410882d31572f0"), // Divination
                new Spell(4, "f2115ac1148256b4ba20788f7e966830") // Restoration
            );
        }

        private BlueprintFeature GetOldBonusSpells()
        {
            return GetBonusSpellsFeature("Old",
                new Spell(3, "486eaff58293f6441a5c2759c4872f98"), // Haste
                new Spell(4, "c66e86905f7606c4eaa5c774f0357b2b") // Stoneskin
                                                                 //new Spell(5, "e84fc922ccf952943b5240293669b171"), // Dispel Evil
                                                                 //new Spell(5, "e84fc922ccf952943b5240293669b171")  // True Seeing
            );
        }

        private BlueprintFeature GetVeryOldBonusSpells()
        {
            return GetBonusSpellsFeature("VeryOld",
                new Spell(4, "5bdc37e4acfa209408334326076a43bc"), // Dimension Door (substitute for Teleport)
                //new Spell(4, "c66e86905f7606c4eaa5c774f0357b2b"), // Spell Immunity
                //new Spell(5, "486eaff58293f6441a5c2759c4872f98"), // Teleport
                new Spell(5, "0a5ddfbcfb3989543ac7c936fc256889"), // Spell Resistance (substitute for Spell Immunity)
                new Spell(6, "ff8f1534f66559c478448723e16b6624") // Heal
            );
        }

        private BlueprintFeature GetAncientBonusSpells()
        {
            return GetBonusSpellsFeature("Ancient",
                //new Spell(5, "5bdc37e4acfa209408334326076a43bc"), // True Seeing
                new Spell(6, "f0f761b808dc4b149b08eaf44b99f633"), // Greater Dispel Magic
                new Spell(7, "368d7cf2fb69d8a46be5a650f5a5a173"), // Walk through Space (substitute for Greater Teleport)
                new Spell(7, "80a1a388ee938aa4e90d427ce9a7a3e9") // Resurrection
            );
        }

        private BlueprintFeature GetWyrmBonusSpells()
        {
            return GetBonusSpellsFeature("Wyrm",
                new Spell(8, "42aa71adc7343714fa92e471baa98d42") // Protection from Spells
            );
        }

        private BlueprintFeature GetGreatWyrmBonusSpells()
        {
            return GetBonusSpellsFeature("GreatWyrm",
                new Spell(8, "cbf3bafa8375340498b86a3313a11e2f") // Euphoric Tranquility
            );
        }

        private BlueprintFeature GetBonusSpellsFeature(string ageCategory, params Spell[] spells)
        {
            var feature = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, $"DragonBloodline{bloodlineName}BonusSpells{ageCategory}Feature", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(IsekaiContext, $"DragonBloodline{bloodlineName}BonusSpells{ageCategory}Feature.Name", $"{bloodlineName} Dragon Bonus Spells ({ageCategory})");
                bp.m_Description = Helpers.CreateString(IsekaiContext, $"DragonBloodline{bloodlineName}BonusSpells{ageCategory}LevelFeature.Description", "At certain levels, you gain access to spells that represent the power of your dragon heritage. These spells are added to your spellbook and can be prepared and cast as normal.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                foreach (var spell in spells)
                {
                    bp.AddComponent<AddKnownSpell>(c =>
                    {
                        c.m_Spell = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>(spell.BlueprintId);
                        c.m_CharacterClass = DragonClass.GetReference();
                        c.SpellLevel = spell.Level;
                    });
                }
            });

            return feature;
        }

        private BlueprintFeature GetBlessAtWillFeature()
        {
            var feature = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, $"DragonBloodline{bloodlineName}BlessAtWillFeature", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(IsekaiContext, $"DragonBloodline{bloodlineName}BlessAtWillFeature.Name", $"{bloodlineName} Dragon Blessing (At-Will)");
                bp.m_Description = Helpers.CreateString(IsekaiContext, $"DragonBloodline{bloodlineName}BlessAtWillFeature.Description", "You can cast the bless spell at will as a spell-like ability.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<AddFacts>(c =>
                {
                    c.m_Facts = new BlueprintUnitFactReference[]
                    {
                        BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("90e59f4a4ada87243b7b3535a06d0638") // Bless spell
                    };
                });
                bp.AddComponent<ReplaceCasterLevelOfAbility>(c =>
                {
                    c.m_Spell = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("90e59f4a4ada87243b7b3535a06d0638"); // Bless spell;
                    c.m_Class = DragonClass.GetReference();
                });
            });
            return feature;
        }

        private BlueprintFeature GetSunburstAtWillFeature()
        {
            var feature = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, $"DragonBloodline{bloodlineName}SunburstAtWillFeature", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(IsekaiContext, $"DragonBloodline{bloodlineName}SunburstAtWillFeature.Name", $"{bloodlineName} Dragon Sunburst (At-Will)");
                bp.m_Description = Helpers.CreateString(IsekaiContext, $"DragonBloodline{bloodlineName}SunburstAtWillFeature.Description", "You can cast the sunburst spell at will as a spell-like ability.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<AddFacts>(c =>
                {
                    c.m_Facts = new BlueprintUnitFactReference[]
                    {
                        BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("e96424f70ff884947b06f41a765b7658") // Sunburst spell
                    };
                });
                bp.AddComponent<ReplaceCasterLevelOfAbility>(c =>
                {
                    c.m_Spell = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("e96424f70ff884947b06f41a765b7658"); // Sunburst spell;
                    c.m_Class = DragonClass.GetReference();
                });
            });
            return feature;
        }

        private struct Spell
        {
            public Spell(int level, string blueprintId)
            {
                Level = level;
                BlueprintId = blueprintId;
            }

            public int Level { get; }
            public string BlueprintId { get; }
        }
    }
}
