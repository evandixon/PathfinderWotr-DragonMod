using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.Localization;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using System;
using System.Collections.Generic;
using TabletopTweaks.Core.Utilities;
using static DragonMod.Main;

namespace DragonMod.Content.Dragon.Bloodlines
{
    public class DragonBloodlineSilver : BaseDragonBloodline
    {
        public static DragonBloodlineSilver Instance { get; } = new DragonBloodlineSilver();

        protected override string BloodlineName => "Silver";
        protected override DamageEnergyType Element => DamageEnergyType.Cold;
        protected override string EnergyImmunityBlueprintId => "9ae23798a9284e044ad2716a772a410e"; // Cold
        protected override string EnergyVulnerabilityBlueprintId => "8e934134fec60ab4c8972c85a7b62f89"; // Fire
        protected override string DragonWingsFeatureId => "0080e2cf464f67143809ed0f96ddd1f7"; // Sorcerer silver dragon wings
        protected override string PrimaryBreathProjectileId => "72b45860bdfb81f4284aa005c04594dd"; // ColdCone30Feet00Breath
        protected override string ShifterFormMediumId => "3c4bf82676d345dca2718cac680f5906";
        protected override string ShifterFormLargeId => "2de04456ce2d4e79804f899498ab31cc";
        protected override string ShifterFormHugeId => "3be4d85d65a94960b4242522d0965633";
        protected override int BaseNaturalArmorBonus => 6;

        public DragonBloodlineSilver()
        { 
            AgeCategories = new List<DragonAge>
            {
                new DragonAge
                {
                    Name = DragonAgeName.Wyrmling,
                    Size = Size.Small,
                    HitDice = 7,
                    BonusSpells = new List<Spell>()
                },
                new DragonAge
                {
                    Name = DragonAgeName.VeryYoung,
                    Size = Size.Medium,
                    HitDice = 9,
                    BonusSpells = new List<Spell>()
                },
                new DragonAge
                {
                    Name = DragonAgeName.Young,
                    Size = Size.Large,
                    HitDice = 11,
                    BonusSpells = new List<Spell>()
                    {
                        new Spell(0, "95f206566c5261c42aa5b3e7e0d1e36c"), // Light (MageLight)
                        new Spell(1, "2c38da66e5a599347ac95b3294acbe00") // True Strike
                    }
                },
                new DragonAge
                {
                    Name = DragonAgeName.Juvenile,
                    Size = Size.Large,
                    HitDice = 13,
                    BonusSpells = new List<Spell>
                    {
                        new Spell(1, "9d5d2d3ffdd73c648af3eb3e585b1113"), // Divine Favor
                    },
                    BonusFeatures = new List<Func<BlueprintFeature>>()
                    {
                    }
                },
                new DragonAge
                {
                    Name = DragonAgeName.YoungAdult,
                    Size = Size.Huge,
                    HitDice = 15,
                    DamageResistance = 5,
                    SpellResistance = 25,
                    BonusSpells = new List<Spell>()
                    {
                        new Spell(1, "ef768022b0785eb43a18969903c537c4"), // Shield
                        new Spell(2, "1c1ebf5370939a9418da93176cc44cd9"), // Cure Moderate Wounds
                    }
                },
                new DragonAge
                {
                    Name = DragonAgeName.Adult,
                    Size = Size.Huge,
                    HitDice = 17,
                    DamageResistance = 5,
                    SpellResistance = 25,
                    BonusSpells = new List<Spell>
                    {
                        new Spell(3, "92681f181b507b34ea87018e8f7a528a"), // Dispel Magic
                        new Spell(2, "134cb6d492269aa4f8662700ef57449f"), // Web
                    },
                    BonusFeatures = new List<Func<BlueprintFeature>>
                    {
                        GetFrightfulPresenceFeature
                    }
                },
                new DragonAge
                {
                    Name = DragonAgeName.MatureAdult,
                    Size = Size.Huge,
                    HitDice = 19,
                    DamageResistance = 10,
                    SpellResistance = 26,
                    BonusSpells = new List<Spell>()
                    {
                        new Spell(4, "5bdc37e4acfa209408334326076a43bc"), // Dimension Door
                        new Spell(4, "f2115ac1148256b4ba20788f7e966830") // Restoration
                    }
                },
                new DragonAge
                {
                    Name = DragonAgeName.Old,
                    Size = Size.Gargantuan,
                    HitDice = 21,
                    DamageResistance = 10,
                    SpellResistance = 28,
                    BonusSpells = new List<Spell>()
                    {
                        new Spell(5, "f9910c76efc34af41b6e43d5d8752f0f"), // Flame Strike
                        new Spell(3, "c7104f7526c4c524f91474614054547e"), // Hold Person
                        //new Spell(2, ""), // Calm Emotions
                    }
                },
                new DragonAge
                {
                    Name = DragonAgeName.VeryOld,
                    Size = Size.Gargantuan,
                    HitDice = 23,
                    DamageResistance = 15,
                    SpellResistance = 29,
                    BonusSpells = new List<Spell>()
                    {
                        // Break Enchantment
                        new Spell(6, "f0f761b808dc4b149b08eaf44b99f633"), // Greater Dispel Magic
                        new Spell(6, "ff8f1534f66559c478448723e16b6624") // Heal
                    }
                },
                new DragonAge
                {
                    Name = DragonAgeName.Ancient,
                    Size = Size.Gargantuan,
                    HitDice = 25,
                    DamageResistance = 15,
                    SpellResistance = 30,
                    BonusSpells = new List<Spell>()
                    {                    
                        // Holy Word
                        // Repulsion
                    },
                    BonusFeatures = new List<Func<BlueprintFeature>>()
                    {
                    }
                },
                new DragonAge
                {
                    Name = DragonAgeName.Wyrm,
                    Size = Size.Gargantuan,
                    HitDice = 27,
                    DamageResistance = 20,
                    SpellResistance = 31,
                    BonusSpells = new List<Spell>()
                    {
                        new Spell(8, "808ab74c12df8784ab4eeaf6a107dbea") // Holy Aura
                    }
                },
                new DragonAge
                {
                    Name = DragonAgeName.GreatWyrm,
                    Size = Size.Colossal,
                    HitDice = 29,
                    DamageResistance = 20,
                    SpellResistance = 33,
                    BonusSpells = new List<Spell>()
                    {
                        new Spell(8, "1f173a16120359e41a20fc75bb53d449") // Mass cure critical wounds
                        // True resurrection
                    }
                },
            };
        }

        protected override List<DragonAge> AgeCategories { get; }

        protected override BlueprintFeature GetSecondaryBreath(BlueprintBuff breathCooldownBuff)
        {
            // to-do

            var breathAbility = Helpers.CreateBlueprint<BlueprintAbility>(IsekaiContext, $"DragonBloodline{BloodlineName}SeconaryBreathAbility", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(IsekaiContext, $"DragonBloodline{BloodlineName}SeconaryBreathAbility.Name", "Weakening Breath");
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
            var breathAbilityReference = BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(IsekaiContext, $"DragonBloodline{BloodlineName}SeconaryBreathAbility");
            var breathAbilityReferenceUnit = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(IsekaiContext, $"DragonBloodline{BloodlineName}SeconaryBreathAbility");

            return Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, $"DragonBloodline{BloodlineName}SecondaryBreathFeature", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(IsekaiContext, $"DragonBloodline{BloodlineName}SeconaryBreathAbility.Name", "Weakening Breath");

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

        private BlueprintFeature GetFrightfulPresenceFeature()
        {
            return BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("a2e0cbebe3bb4a90a22b75d3c22d952c");
        }
    }
}
