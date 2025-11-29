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

namespace DragonMod.Content.Dragon.Bloodlines.Gold
{
    public class DragonBloodlineGold : BaseDragonBloodline
    {
        public static DragonBloodlineGold Instance { get; } = new DragonBloodlineGold();

        protected override string BloodlineName => "Gold";
        protected override DamageEnergyType Element => DamageEnergyType.Fire;
        protected override string EnergyImmunityBlueprintId => "11ac3433adfa74642a93111624376070"; // Fire
        protected override string EnergyVulnerabilityBlueprintId => "b8bbe8f713da9ad44a899aa551ca6b5b"; // Cold
        protected override string DragonWingsFeatureId => "6929bac6c67ae194c8c8446e3d593953"; // Sorcerer gold dragon wings
        protected override string PrimaryBreathProjectileId => "52c3a84f628ddde4dbfb38e4a581b01a"; // FireCone30Feet00Breath
        protected override string ShifterFormMediumId => "f5ac253cbee44744a7399f17765160d5";
        protected override string ShifterFormLargeId => "5a679cd137d64c629995c626616dbb17";
        protected override string ShifterFormHugeId => "833873205d9b46e99217d02cd04a20d4";
        protected override int BaseNaturalArmorBonus => 7;
        protected override int StartingSpellcastingLevel => 12;
        protected override int MinimumXP => 9000; // Level 8 with custom table
        protected override DiceType PrimaryBreathWeaponDamageDie => DiceType.D10;

        public DragonBloodlineGold()
        { 
            AgeCategories = new List<DragonAge>
            {
                new DragonAge
                {
                    Name = DragonAgeName.Wyrmling,
                    Size = Size.Small,
                    HitDice = 8,
                    BonusSpells = new List<Spell>()
                },
                new DragonAge
                {
                    Name = DragonAgeName.VeryYoung,
                    Size = Size.Medium,
                    HitDice = 10,
                    CanChangeShape = true,
                    BonusSpells = new List<Spell>()
                },
                new DragonAge
                {
                    Name = DragonAgeName.Young,
                    Size = Size.Large,
                    HitDice = 12,
                    CanChangeShape = true,
                    BonusSpells = new List<Spell>()
                    {
                        new Spell(0, "95f206566c5261c42aa5b3e7e0d1e36c"), // Light (MageLight)
                        new Spell(0, "0557ccee0a86dc44cb3d3f6a3b235329"), // Stablize
                        new Spell(1, "9e1ad5d6f87d19e4d8883d63a6e35568"), // Mage Armor
                        new Spell(1, "ef768022b0785eb43a18969903c537c4") // Shield
                    }
                },
                new DragonAge
                {
                    Name = DragonAgeName.Juvenile,
                    Size = Size.Large,
                    HitDice = 14,
                    CanChangeShape = true,
                    BonusSpells = new List<Spell>
                    {
                        new Spell(1, "9d5d2d3ffdd73c648af3eb3e585b1113") // Divine Favor
                    },
                    BonusFeatures = new List<Func<BlueprintFeature>>()
                    {
                        GetBlessAtWillFeature
                    }
                },
                new DragonAge
                {
                    Name = DragonAgeName.YoungAdult,
                    Size = Size.Huge,
                    HitDice = 16,
                    DamageResistance = 5,
                    SpellResistance = 25,
                    CanChangeShape = true,
                    BonusSpells = new List<Spell>()
                    {
                        new Spell(1, "183d5bb91dea3a1489a6db6c9cb64445"), // Shield of Faith
                        new Spell(2, "1c1ebf5370939a9418da93176cc44cd9"), // Cure Moderate Wounds
                        new Spell(2, "21ffef7791ce73f468b6fca4d9371e8b") // Resist Energy
                    }
                },
                new DragonAge
                {
                    Name = DragonAgeName.Adult,
                    Size = Size.Huge,
                    HitDice = 18,
                    DamageResistance = 5,
                    SpellResistance = 25,
                    CanChangeShape = true,
                    BonusSpells = new List<Spell>
                    {
                        new Spell(2, "03a9630394d10164a9410882d31572f0"), // Aid
                        new Spell(3, "92681f181b507b34ea87018e8f7a528a"), // Dispel Magic
                        new Spell(3, "faabd2cc67efa4646ac58c7bb3e40fcc") // Prayer
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
                    HitDice = 20,
                    DamageResistance = 5,
                    SpellResistance = 25,
                    CanChangeShape = true,
                    BonusSpells = new List<Spell>()
                    {
                        new Spell(2, "e84fc922ccf952943b5240293669b171"), // Lesser Restoration
                        new Spell(2, "30e5dc243f937fc4b95d2f8f4e1b7ff3"), // See Invisibility
                        //new Spell(4, ""), // Divination
                        new Spell(4, "f2115ac1148256b4ba20788f7e966830") // Restoration
                    }
                },
                new DragonAge
                {
                    Name = DragonAgeName.Old,
                    Size = Size.Gargantuan,
                    HitDice = 22,
                    DamageResistance = 10,
                    SpellResistance = 29,
                    CanChangeShape = true,
                    BonusSpells = new List<Spell>()
                    {
                        new Spell(3, "486eaff58293f6441a5c2759c4872f98"), // Haste
                        new Spell(4, "c66e86905f7606c4eaa5c774f0357b2b") // Stoneskin
                                                                         //new Spell(5, "e84fc922ccf952943b5240293669b171"), // Dispel Evil
                                                                         //new Spell(5, "e84fc922ccf952943b5240293669b171")  // True Seeing
                    }
                },
                new DragonAge
                {
                    Name = DragonAgeName.VeryOld,
                    Size = Size.Gargantuan,
                    HitDice = 24,
                    DamageResistance = 14,
                    SpellResistance = 30,
                    CanChangeShape = true,
                    BonusSpells = new List<Spell>()
                    {
                        new Spell(4, "5bdc37e4acfa209408334326076a43bc"), // Dimension Door (substitute for Teleport)
                        //new Spell(4, "c66e86905f7606c4eaa5c774f0357b2b"), // Spell Immunity
                        //new Spell(5, "486eaff58293f6441a5c2759c4872f98"), // Teleport
                        new Spell(5, "0a5ddfbcfb3989543ac7c936fc256889"), // Spell Resistance (substitute for Spell Immunity)
                        new Spell(6, "ff8f1534f66559c478448723e16b6624") // Heal
                    }
                },
                new DragonAge
                {
                    Name = DragonAgeName.Ancient,
                    Size = Size.Gargantuan,
                    HitDice = 26,
                    DamageResistance = 15,
                    SpellResistance = 31,
                    CanChangeShape = true,
                    BonusSpells = new List<Spell>()
                    {                    
                        //new Spell(5, "5bdc37e4acfa209408334326076a43bc"), // True Seeing
                        new Spell(6, "f0f761b808dc4b149b08eaf44b99f633"), // Greater Dispel Magic
                        new Spell(7, "368d7cf2fb69d8a46be5a650f5a5a173"), // Walk through Space (substitute for Greater Teleport)
                        new Spell(7, "80a1a388ee938aa4e90d427ce9a7a3e9") // Resurrection
                    },
                    BonusFeatures = new List<Func<BlueprintFeature>>()
                    {
                        GetSunburstAtWillFeature
                    }
                },
                new DragonAge
                {
                    Name = DragonAgeName.Wyrm,
                    Size = Size.Gargantuan,
                    HitDice = 28,
                    DamageResistance = 20,
                    SpellResistance = 32,
                    CanChangeShape = true,
                    BonusSpells = new List<Spell>()
                    {
                        new Spell(8, "42aa71adc7343714fa92e471baa98d42") // Protection from Spells
                    }
                },
                new DragonAge
                {
                    Name = DragonAgeName.GreatWyrm,
                    Size = Size.Colossal,
                    HitDice = 30,
                    DamageResistance = 20,
                    SpellResistance = 34,
                    CanChangeShape = true,
                    BonusSpells = new List<Spell>()
                    {
                        new Spell(8, "cbf3bafa8375340498b86a3313a11e2f") // Euphoric Tranquility
                    }
                },
            };
        }

        protected override List<DragonAge> AgeCategories { get; }

        protected override BlueprintFeature GetSecondaryBreath(BlueprintBuff breathCooldownBuff)
        {
            var breathAbility = Helpers.CreateBlueprint<BlueprintAbility>(DragonModContext, $"DragonBloodline{BloodlineName}SeconaryBreathAbility", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(DragonModContext, $"DragonBloodline{BloodlineName}SeconaryBreathAbility.Name", "Weakening Breath");
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
                    // We handle the saves manually inside, so this is cosmetic / UI only
                    c.SavingThrowType = SavingThrowType.Unknown;

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
            var breathAbilityReference = BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(DragonModContext, $"DragonBloodline{BloodlineName}SeconaryBreathAbility");
            var breathAbilityReferenceUnit = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(DragonModContext, $"DragonBloodline{BloodlineName}SeconaryBreathAbility");

            return Helpers.CreateBlueprint<BlueprintFeature>(DragonModContext, $"DragonBloodline{BloodlineName}SecondaryBreathFeature", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(DragonModContext, $"DragonBloodline{BloodlineName}SeconaryBreathAbility.Name", "Weakening Breath");

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

        private BlueprintFeature GetBlessAtWillFeature()
        {
            var feature = Helpers.CreateBlueprint<BlueprintFeature>(DragonModContext, $"DragonBloodline{BloodlineName}BlessAtWillFeature", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(DragonModContext, $"DragonBloodline{BloodlineName}BlessAtWillFeature.Name", $"{BloodlineName} Dragon Blessing (At-Will)");
                bp.m_Description = Helpers.CreateString(DragonModContext, $"DragonBloodline{BloodlineName}BlessAtWillFeature.Description", "You can cast the bless spell at will as a spell-like ability.");
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
            var feature = Helpers.CreateBlueprint<BlueprintFeature>(DragonModContext, $"DragonBloodline{BloodlineName}SunburstAtWillFeature", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(DragonModContext, $"DragonBloodline{BloodlineName}SunburstAtWillFeature.Name", $"{BloodlineName} Dragon Sunburst (At-Will)");
                bp.m_Description = Helpers.CreateString(DragonModContext, $"DragonBloodline{BloodlineName}SunburstAtWillFeature.Description", "You can cast the sunburst spell at will as a spell-like ability.");
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
    }
}
