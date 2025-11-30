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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TabletopTweaks.Core.Utilities;
using DragonMod.Content.Dragon.Features;
using static DragonMod.Main;
using Kingmaker.UnitLogic.Mechanics.Properties;
using DragonMod.Infrastructure;
using DragonMod.Content.Dragon.Buffs;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.UnitLogic.Mechanics.Conditions;

namespace DragonMod.Content.Dragon.Bloodlines
{
    public abstract class BaseDragonBloodline
    {
        protected abstract string BloodlineName { get; }
        protected abstract DamageEnergyType Element { get; }
        protected abstract string PrimaryBreathProjectileId { get; }
        protected abstract string EnergyImmunityBlueprintId { get; }
        protected abstract string EnergyVulnerabilityBlueprintId { get; }
        protected abstract string DragonWingsFeatureId { get; }
        protected abstract string ShifterFormMediumId { get; }
        protected abstract string ShifterFormLargeId { get; }
        protected abstract string ShifterFormHugeId { get; }
        protected abstract int BaseNaturalArmorBonus { get; }
        protected abstract int StartingSpellcastingLevel { get; }
        protected abstract DiceType PrimaryBreathWeaponDamageDie { get; }
        protected abstract int MinimumXP { get; }
        public abstract List<DragonAge> AgeCategories { get; }

        public int[] GetXpTable()
        {
            _xpTable ??= XpTablePatcher.GetCustomXPTable(AgeCategories.First().HitDice, AgeCategories.Last().HitDice);
            return _xpTable;
        }
        private int[] _xpTable;

        // A backup idea if legend patching doesn't pan out
        // Needs to be accompanied by stat adjustments like extra HP and BAB
        protected virtual Dictionary<string, int> AgeToPCLevel { get; } = new Dictionary<string, int>
        {
            [DragonAgeName.Wyrmling] = 1,
            [DragonAgeName.VeryYoung] = 2,
            [DragonAgeName.Young] = 3,
            [DragonAgeName.Juvenile] = 4,
            [DragonAgeName.YoungAdult] = 6,
            [DragonAgeName.Adult] = 8,
            [DragonAgeName.MatureAdult] = 10,
            [DragonAgeName.Old] = 12,
            [DragonAgeName.VeryOld] = 14,
            [DragonAgeName.Ancient] = 16,
            [DragonAgeName.Wyrm] = 18,
            [DragonAgeName.GreatWyrm] = 20,
        };

        public void Add()
        {
            var dragonWings = BlueprintTools.GetBlueprint<BlueprintFeature>(DragonWingsFeatureId);
            var energyVulnerability = !string.IsNullOrEmpty(EnergyVulnerabilityBlueprintId) ? BlueprintTools.GetBlueprint<BlueprintFeature>(EnergyVulnerabilityBlueprintId) : null;
            var energyImmunity = BlueprintTools.GetBlueprint<BlueprintFeature>(EnergyImmunityBlueprintId);

            var breathCooldownBuff = Helpers.CreateBlueprint<BlueprintBuff>(DragonModContext, $"DragonBloodline{BloodlineName}BreathCooldown", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(DragonModContext, $"DragonBloodline{BloodlineName}BreathCooldown.Name", $"{Element:f} Breath Cooldown");
                bp.m_Flags = BlueprintBuff.Flags.IsFromSpell;
                bp.Frequency = DurationRate.Rounds;
                bp.Stacking = StackingType.Replace;
            });

            // Note: this is the half-dragon breath
            // True dragon breath starts lower and scales much higher
            // It should be replaced somewhere along the way
            var breathFeature = GetPrimaryBreath(breathCooldownBuff);

            var secondaryBreathFeature = GetSecondaryBreath(breathCooldownBuff);

            var spellbook = BaseDragonSpellbook.Add(BloodlineName, StartingSpellcastingLevel);

            //var backwardsCompatibility = Helpers.CreateBlueprint<BlueprintProgression>(DragonModContext, $"DragonBloodline{BloodlineName}", bp =>
            //{
            //    bp.m_DisplayName = Helpers.CreateString(DragonModContext, $"DragonBloodline{BloodlineName}Archetype.Name", $"{BloodlineName} Dragon Backwards Compatibility");
            //    bp.m_Description = Helpers.CreateString(DragonModContext, $"DragonBloodline{BloodlineName}Archetype.Description", $"If you can read this, you should respec your character.");
            //});

            //var extraXPFeature = Helpers.CreateBlueprint<BlueprintFeature>(DragonModContext, $"DragonBloodline{BloodlineName}ExtraXPFeature", bp =>
            //{
            //    bp.m_DisplayName = Helpers.CreateString(DragonModContext, $"DragonBloodline{BloodlineName}ExtraXPFeature.Name", $"{BloodlineName} Dragon Extra XP");
            //    bp.m_Description = Helpers.CreateString(DragonModContext, $"DragonBloodline{BloodlineName}ExtraXPFeature.Description", $"Dragons are born strong and start at a higher level.");

            //    bp.AddComponent<MinimumXPComponent>(c =>
            //    {
            //        c.MinimumXP = this.MinimumXP;
            //    });
            //});

            var startingStatsFeature = Helpers.CreateBlueprint<BlueprintFeature>(DragonModContext, $"DragonBloodline{BloodlineName}StartingStatsFeature", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(DragonModContext, $"DragonBloodline{BloodlineName}StartingStatsFeature.Name", $"{BloodlineName} Dragon Stats");
                bp.m_Description = Helpers.CreateString(DragonModContext, $"DragonBloodline{BloodlineName}StartingStatsFeature.Description", $"Dragons are born strong and have higher starting stats.");

                bp.AddComponent<AddStatBonus>(c => 
                {
                    c.Descriptor = ModifierDescriptor.NaturalArmor;
                    c.Stat = StatType.AC;
                    c.Value = BaseNaturalArmorBonus;
                });

                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Strength;
                    c.Value = 4;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Constitution;
                    c.Value = 2;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Intelligence;
                    c.Value = 2;
                });
                //bp.AddComponent<AddStatBonus>(c => {
                //    c.Descriptor = ModifierDescriptor.Racial;
                //    c.Stat = StatType.Wisdom;
                //    c.Value = 2;
                //});
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Charisma;
                    c.Value = 2;
                });

                // Natural weapons
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[]
                    {
                        new BlueprintUnitFactReference { deserializedGuid = DragonNaturalWeapons.GetReference().Guid }
                    };
                });

                // Dragon Tye features
                bp.AddComponent<AddFacts>(c =>
                {
                    c.m_Facts = new BlueprintUnitFactReference[]
                    {
                        BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("c263f44f72df009489409af122b5eefc"),
                        BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("4b152a7bc5bab5042b437b955fea46cd"),
                        BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("70cffb448c132fa409e49156d013b175")
                    };
                });

                // Keen senses (low light vision)
                // Darkvision doesn't exist but close enough
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[]
                    {
                        BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("9c747d24f6321f744aa1bb4bd343880d")
                    };
                });
            });

            var bloodline = Helpers.CreateBlueprint<BlueprintArchetype>(DragonModContext, $"DragonBloodline{BloodlineName}Archetype", bp =>
            {
                bp.LocalizedName = Helpers.CreateString(DragonModContext, $"DragonBloodline{BloodlineName}Archetype.Name", $"{BloodlineName} Dragon");
                bp.LocalizedDescription = Helpers.CreateString(DragonModContext, $"DragonBloodline{BloodlineName}Archetype.Description", $"As a result of strange magical experiments, you have become a true dragon.");
                bp.LocalizedDescriptionShort = Helpers.CreateString(DragonModContext, $"DragonBloodline{BloodlineName}Archetype.DescriptionShort", "");
                bp.m_ReplaceSpellbook = spellbook.ToReference<BlueprintSpellbookReference>();
                //bp.BuildChanging = true;

                bp.RemoveFeatures = new LevelEntry[]
                {
                    Helpers.CreateLevelEntry(1, HalfDragonFeature.GetReference<BlueprintFeatureReference>())
                };
                bp.AddFeatures = new LevelEntry[30]
                {
                    Helpers.CreateLevelEntry(1, dragonWings, energyImmunity, startingStatsFeature),
                    Helpers.CreateLevelEntry(2, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(3, new BlueprintFeature[0]),
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
                    Helpers.CreateLevelEntry(20, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(21, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(22, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(23, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(24, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(25, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(26, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(27, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(28, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(29, new BlueprintFeature[0]),
                    Helpers.CreateLevelEntry(30, new BlueprintFeature[0])
                };
                bp.m_StartingItems = new BlueprintItemReference[0];
                bp.ClassSkills = new StatType[0];
                bp.RecommendedAttributes = new StatType[0];
                bp.NotRecommendedAttributes = new StatType[0];
                bp.m_SignatureAbilities = new BlueprintFeatureReference[0];
            });

            if (energyVulnerability != null)
            {
                bloodline.AddFeatures.Single(l => l.Level == 1).m_Features.Add(energyVulnerability.ToReference<BlueprintFeatureBaseReference>());
            }

            BlueprintFeature previousFormFeature = null;
            foreach (var age in AgeCategories)
            {
                //var level = AgeToPCLevel[age.Name];

                var breathStartingLevel = age.HitDice; // Withold breath until the dragon reaches the starting HD so the math works right
                var level = age.HitDice;
                if (age.Name == DragonAgeName.Wyrmling)
                {
                    // Start at level 1 to give the feel of a dragon at the start
                    level = 1;
                }
                var levelEntry = bloodline.AddFeatures.Single(l => l.Level == level);

                var formFeature = GetFormFeature(age, previousFormFeature?.ToReference<BlueprintUnitFactReference>());
                levelEntry.m_Features.Add(formFeature.ToReference<BlueprintFeatureBaseReference>());
                previousFormFeature = formFeature;

                levelEntry.m_Features.Add(breathFeature.ToReference<BlueprintFeatureBaseReference>());
                if (secondaryBreathFeature != null)
                {
                    levelEntry.m_Features.Add(secondaryBreathFeature.ToReference<BlueprintFeatureBaseReference>());
                }

                if (age.BonusSpells?.Any() == true)
                {
                    var spellsFeature = GetBonusSpellsFeature(age);
                    levelEntry.m_Features.Add(spellsFeature.ToReference<BlueprintFeatureBaseReference>());
                }

                if (age.BonusFeatures?.Any() == true)
                {
                    foreach (var feature in age.BonusFeatures)
                    {
                        levelEntry.m_Features.Add(feature().ToReference<BlueprintFeatureBaseReference>());
                    }
                }

                ProcessAgeBasedStatBonusesForBaseCharacter(age, levelEntry);
            }

            //var spellbookFeature = Helpers.CreateBlueprint<BlueprintFeature>(DragonModContext, $"DragonBloodline{BloodlineName}SpellbookFeature", bp =>
            //{
            //    bp.m_DisplayName = Helpers.CreateString(DragonModContext, $"DragonBloodline{BloodlineName}SpellbookFeature.Name", $"{BloodlineName} Dragon Spellbook");
            //    bp.Ranks = 1;
            //    bp.IsClassFeature = true;
            //    bp.AddComponent<AddSpellbook>(c =>
            //    {
            //        c.m_Spellbook = spellbook.ToReference<BlueprintSpellbookReference>();
            //        c.m_CasterLevel = new ContextValue
            //        {
            //            ValueType = ContextValueType.CasterProperty,
            //            Property = UnitProperty.Level,
            //            m_AbilityParameter = AbilityParameterType.Level,
            //            PropertyName = ContextPropertyName.Value1
            //        };
            //    });
            //});
            //var spellbookLevelFeature = Helpers.CreateBlueprint<BlueprintFeature>(DragonModContext, $"DragonBloodline{BloodlineName}SpellbookLevelFeature", bp =>
            //{
            //    bp.m_DisplayName = Helpers.CreateString(DragonModContext, $"DragonBloodline{BloodlineName}SpellbookLevelFeature.Name", $"{BloodlineName} Dragon Spellbook Level");
            //    bp.Ranks = 1;
            //    bp.IsClassFeature = true;
            //    bp.HideInUI = true;
            //    bp.AddComponent<AddSpellbookLevel>(c =>
            //    {
            //        c.m_Spellbook = spellbook.ToReference<BlueprintSpellbookReference>();
            //    });
            //});
            //bloodline.LevelEntries.Single(l => l.Level == StartingSpellcastingLevel).m_Features.Add(spellbookFeature.ToReference<BlueprintFeatureBaseReference>());
            //for (int i = StartingSpellcastingLevel; i <= 30; i++)
            //{
            //    bloodline.LevelEntries.Single(l => l.Level == i).m_Features.Add(spellbookLevelFeature.ToReference<BlueprintFeatureBaseReference>());
            //}
        }

        public T GetReference<T>() where T : BlueprintReferenceBase
        {
            return BlueprintTools.GetModBlueprintReference<T>(DragonModContext, $"DragonBloodline{BloodlineName}Archetype");
        }

        private BlueprintFeature GetPrimaryBreath(BlueprintBuff breathCooldownBuff)
        {
            var featureName = $"DragonBloodline{BloodlineName}BreathFeature";
            var breathAbility = Helpers.CreateBlueprint<BlueprintAbility>(DragonModContext, $"DragonBloodline{BloodlineName}BreathAbility", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(DragonModContext, $"DragonBloodline{BloodlineName}BreathAbility.Name", $"{Element:F} Breath");
                bp.CanTargetPoint = true;
                bp.CanTargetEnemies = true;
                bp.CanTargetFriends = true;
                bp.CanTargetSelf = false;
                bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
                bp.EffectOnAlly = AbilityEffectOnUnit.Harmful;
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
                    c.IgnoreCaster = true;
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
                                    Energy = Element
                                },
                                Value = new ContextDiceValue
                                {
                                    DiceType = PrimaryBreathWeaponDamageDie,
                                    DiceCountValue = new ContextValue
                                    {
                                        ValueType = ContextValueType.Rank,
                                        ValueRank = AbilityRankType.DamageDice
                                    },
                                    BonusValue  = new ContextValue
                                    {
                                        ValueType = ContextValueType.Simple,
                                        Value = 0
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
                    c.m_Projectiles = new[] { BlueprintTools.GetBlueprintReference<BlueprintProjectileReference>(PrimaryBreathProjectileId) };
                    c.Type = AbilityProjectileType.Cone;
                    c.m_Length = new Feet
                    {
                        m_Value = 30
                    };
                    c.NeedAttackRoll = false;
                });
                bp.AddComponent<ContextRankConfig>(c =>
                {
                    c.m_Type = AbilityRankType.DamageDice;
                    c.m_BaseValueType = ContextRankBaseValueType.FeatureRank;
                    c.m_Progression = ContextRankProgression.DoublePlusBonusValue;
                    c.m_Max = 20;
                    c.m_Class = new BlueprintCharacterClassReference[]
                    {
                        DragonClass.GetReference()
                    };
                    c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(DragonModContext, featureName);
                });
                bp.AddComponent<ContextRankConfig>(c =>
                {
                    c.m_Type = AbilityRankType.StatBonus;
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
            var breathAbilityReference = BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(DragonModContext, $"DragonBloodline{BloodlineName}BreathAbility");
            var breathAbilityReferenceUnit = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(DragonModContext, $"DragonBloodline{BloodlineName}BreathAbility");

            return Helpers.CreateBlueprint<BlueprintFeature>(DragonModContext, featureName, bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(DragonModContext, $"DragonBloodline{BloodlineName}BreathFeature.Name", $"{Element:F} Breath");

                bp.Ranks = 12;
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

        protected abstract BlueprintFeature GetSecondaryBreath(BlueprintBuff breathCooldownBuff);

        private BlueprintFeature GetFormFeature(DragonAge age, BlueprintUnitFactReference previousStageFeature)
        {
            var sizeName = age.Size.ToString("F").Replace(" ", "");
            var ageName = age.Name.Replace(" ", "");
            var mythicDragonFormAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("a0273cfaafe84f0b89a70b3580568ebc");

            // will hold a delegate to add size-specific weapon override to buff
            Action<BlueprintBuff> addSizeSpecificBuffComponent = null;

            var buff = Helpers.CreateBlueprint<BlueprintBuff>(DragonModContext, $"DragonBloodline{BloodlineName}Form{ageName}Buff", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(DragonModContext, $"DragonBloodline{BloodlineName}Form{ageName}Buff.Name", $"{BloodlineName} Dragon Form ({ageName})");
                bp.m_Description = Helpers.CreateString(DragonModContext, $"DragonBloodline{BloodlineName}Form{ageName}Buff.Description", $"As a {ageName} dragon, you can assume your true form of a {sizeName} dragon, gaining its physical attributes and natural weapons.");
                bp.Comment = "";
                bp.m_AllowNonContextActions = false;
                bp.IsClassFeature = true;
                bp.m_Flags = 0;
                bp.Stacking = StackingType.Replace;
                bp.Ranks = (int)BlueprintBuff.Flags.StayOnDeath;
                bp.TickEachSecond = false;
                bp.Frequency = DurationRate.Rounds;
                bp.FxOnStart = null;
                bp.FxOnRemove = null;
                bp.ResourceAssetIds = Array.Empty<string>();

                bp.m_Icon = mythicDragonFormAbility.m_Icon;

                // Dragon claws make it hard to hold weapons, wear gloves or shoes, etc
                bp.AddComponent<LockEquipmentSlot>(c =>
                {
                    c.m_SlotType = LockEquipmentSlot.SlotType.Gloves;
                });
                bp.AddComponent<LockEquipmentSlot>(c =>
                {
                    c.m_SlotType = LockEquipmentSlot.SlotType.Boots;
                });
                bp.AddComponent<LockEquipmentSlot>(c =>
                {
                    c.m_SlotType = LockEquipmentSlot.SlotType.MainHand;
                });
                bp.AddComponent<LockEquipmentSlot>(c =>
                {
                    c.m_SlotType = LockEquipmentSlot.SlotType.OffHand;
                });
                bp.AddComponent<LockEquipmentSlot>(c =>
                {
                    c.m_SlotType = LockEquipmentSlot.SlotType.Weapon1;
                });
                bp.AddComponent<LockEquipmentSlot>(c =>
                {
                    c.m_SlotType = LockEquipmentSlot.SlotType.Weapon2;
                });
                bp.AddComponent<LockEquipmentSlot>(c =>
                {
                    c.m_SlotType = LockEquipmentSlot.SlotType.Weapon3;
                });
                bp.AddComponent<LockEquipmentSlot>(c =>
                {
                    c.m_SlotType = LockEquipmentSlot.SlotType.Weapon4;
                });
                bp.AddComponent<LockEquipmentSlot>(c =>
                {
                    c.m_SlotType = LockEquipmentSlot.SlotType.Weapon5;
                });
                bp.AddComponent<LockEquipmentSlot>(c =>
                {
                    c.m_SlotType = LockEquipmentSlot.SlotType.Weapon6;
                });
                bp.AddComponent<LockEquipmentSlot>(c =>
                {
                    c.m_SlotType = LockEquipmentSlot.SlotType.Weapon7;
                });
                bp.AddComponent<LockEquipmentSlot>(c =>
                {
                    c.m_SlotType = LockEquipmentSlot.SlotType.Weapon8;
                });

                // Can dragons wear barding?
                // I'm inclined to think "no".
                bp.AddComponent<LockEquipmentSlot>(c =>
                {
                    c.m_SlotType = LockEquipmentSlot.SlotType.Armor;
                });
                //bp.AddComponent<AddFacts>(c =>
                //{
                //    c.m_Facts = new BlueprintUnitFactReference[]
                //    {
                //        // Barding proficiency
                //        new BlueprintUnitFactReference
                //        {
                //            deserializedGuid = BlueprintGuid.Parse("c62ba548b1a34b94b9802925b35737c2")
                //        }
                //    };
                //});

                bp.AddComponent<UnequipUnsupportedItemsComponent>();

                bp.AddComponent<Polymorph>(c =>
                {
                    c.m_Race = null;
                    c.m_SpecialDollType = SpecialDollType.None;
                    c.m_ReplaceUnitForInspection = null;
                    //m_PortraitTypeEntry = UnitEntityData.PortraitType.SmallPortrait,
                    c.m_KeepSlots = true;
                    c.UseSizeAsBaseForDamage = true;

                    c.m_MainHand = null;
                    c.m_OffHand = null;
                    c.AllowDamageTransfer = true;
                    c.m_SilentCaster = true;
                    if (age.CanChangeShape)
                    {
                        c.m_Facts = new BlueprintUnitFactReference[]
                        {
                            // Turn back ability standard
                            new BlueprintUnitFactReference
                            {
                                deserializedGuid = BlueprintGuid.Parse("bd09b025ee2a82f46afab922c4decca9")
                            },
                        };
                    }
                    else
                    {
                        c.m_Facts = Array.Empty<BlueprintUnitFactReference>();
                    }

                    c.Size = age.Size;

                    switch (c.Size)
                    {
                        case Size.Fine:
                        case Size.Diminutive:
                            throw new InvalidOperationException("Unsupported size: " +  c.Size);
                        case Size.Tiny:
                            addSizeSpecificBuffComponent = ApplyTinyFormPolymorphSettings(c);
                            break;
                        case Size.Small:
                            addSizeSpecificBuffComponent = ApplySmallFormPolymorphSettings(c);
                            break;
                        case Size.Medium:
                            addSizeSpecificBuffComponent = ApplyMediumFormPolymorphSettings(c);
                            break;
                        case Size.Large:
                            addSizeSpecificBuffComponent = ApplyLargeFormPolymorphSettings(c);
                            break;
                        case Size.Huge:
                            addSizeSpecificBuffComponent = ApplyHugeFormPolymorphSettings(c);
                            break;
                        case Size.Gargantuan:
                            addSizeSpecificBuffComponent = ApplyGargantuanFormPolymorphSettings(c);
                            break;
                        case Size.Colossal:
                            addSizeSpecificBuffComponent = ApplyCollossalFormPolymorphSettings(c);
                            break;
                    }

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
                bp.AddComponent<ChangeUnitSize>(c =>
                {
                    c.m_Type = ChangeUnitSize.ChangeType.Value;
                    c.Size = age.Size;
                });


                bp.AddComponent<AddFactContextActions>(c =>
                {
                    c.Activated = ActionFlow.DoSingle<ContextActionRemoveBuff>(r =>
                    {
                        r.m_Buff = DragonNaturalWeapons.GetReference();
                    });
                    c.Deactivated = ActionFlow.DoSingle<ContextActionApplyBuff>(r =>
                    {
                        r.m_Buff = DragonNaturalWeapons.GetReference();
                        r.Permanent = true;
                    });
                });

                // Remove kitsune bite
                bp.AddComponent<AddFactContextActions>(c =>
                {
                    c.Activated = ActionFlow.DoSingle<ContextActionRemoveBuff>(r =>
                    {
                        // Kitsune bite
                        r.m_Buff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("08ce3d993d3a69e43a7b77425f93e964");
                    });
                    c.Deactivated = ActionFlow.DoSingle<Conditional>(c =>
                    {
                        c.ConditionsChecker = ActionFlow.IfSingle<ContextConditionHasFact>(c =>
                        {
                            // Kitsune change shape
                            c.m_Fact = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("88063b0ec1cbc8b499e313cde36a8519");
                        });
                        c.IfTrue = ActionFlow.DoSingle<ContextActionApplyBuff>(r =>
                        {
                            // Kitsune bite
                            r.m_Buff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("08ce3d993d3a69e43a7b77425f93e964");
                            r.Permanent = true;
                        });
                        c.IfFalse = ActionFlow.DoNothing();
                    });
                });

                if (age.DamageResistance > 0)
                {
                    bp.AddComponent<AddDamageResistancePhysical>(c =>
                    {
                        c.Value = new ContextValue
                        {
                            ValueType = ContextValueType.Simple,
                            Value = age.DamageResistance
                        };
                        c.BypassedByMagic = true;
                    });
                }
                if (age.SpellResistance > 0)
                {
                    bp.AddComponent<AddSpellResistance>(c =>
                    {
                        c.Value = new ContextValue
                        {
                            ValueType = ContextValueType.Simple,
                            Value = age.SpellResistance
                        };
                    });
                }

            });

            // execute deferred size-specific buff component addition
            addSizeSpecificBuffComponent?.Invoke(buff);

            var ability = Helpers.CreateBlueprint<BlueprintAbility>(DragonModContext, $"DragonBloodline{BloodlineName}Form{ageName}Ability", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(DragonModContext, $"DragonBloodline{BloodlineName}Form{ageName}Ability.Name", $"Dragon Form ({ageName})");

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

            var feature = Helpers.CreateBlueprint<BlueprintFeature>(DragonModContext, $"DragonBloodline{BloodlineName}Form{ageName}Feature", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(DragonModContext, $"DragonBloodline{BloodlineName}Form{ageName}Feature.Name", $"Dragon Form ({ageName})");

                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddComponent<AddFacts>(c =>
                {
                    c.m_Facts = new BlueprintUnitFactReference[]
                    {
                        age.CanChangeShape 
                        ? ability.ToReference<BlueprintUnitFactReference>() 
                        : buff.ToReference<BlueprintUnitFactReference>()
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

        private Action<BlueprintBuff> ApplyTinyFormPolymorphSettings(Polymorph c)
        {
            var shifterFormBuff = BlueprintTools.GetBlueprint<BlueprintBuff>(ShifterFormMediumId);
            var shifterFormBuffPolymorph = (Polymorph)(shifterFormBuff.Components.Single(cmp => cmp is Polymorph));

            c.m_Prefab = shifterFormBuffPolymorph.m_Prefab;
            c.m_PrefabFemale = shifterFormBuffPolymorph.m_PrefabFemale;
            c.m_Portrait = shifterFormBuffPolymorph.m_Portrait;
            c.m_EnterTransition = shifterFormBuffPolymorph.m_EnterTransition;
            c.m_ExitTransition = shifterFormBuffPolymorph.m_ExitTransition;
            c.m_TransitionExternal = shifterFormBuffPolymorph.m_TransitionExternal;

            c.StrengthBonus = -4;
            c.DexterityBonus = 2;
            c.ConstitutionBonus = 0;

            c.m_AdditionalLimbs = new[]
            {
                // Bite 1d4
                new BlueprintItemWeaponReference
                {
                    deserializedGuid = BlueprintGuid.Parse("35dfad6517f401145af54111be04d6cf")
                },
            };

            return (buff) =>
            {
                // Because slots are kept but weapons are locked
                buff.AddComponent<EmptyHandWeaponOverride>(cmp =>
                {
                    // Claw 1d3
                    cmp.m_Weapon = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("800092a2b9a743b48ae8aeeb5d243dcc");
                });
            };
        }

        private Action<BlueprintBuff> ApplySmallFormPolymorphSettings(Polymorph c)
        {
            var shifterFormBuff = BlueprintTools.GetBlueprint<BlueprintBuff>(ShifterFormMediumId);
            var shifterFormBuffPolymorph = (Polymorph)(shifterFormBuff.Components.Single(cmp => cmp is Polymorph));

            c.m_Prefab = shifterFormBuffPolymorph.m_Prefab;
            c.m_PrefabFemale = shifterFormBuffPolymorph.m_PrefabFemale;
            c.m_Portrait = shifterFormBuffPolymorph.m_Portrait;
            c.m_EnterTransition = shifterFormBuffPolymorph.m_EnterTransition;
            c.m_ExitTransition = shifterFormBuffPolymorph.m_ExitTransition;
            c.m_TransitionExternal = shifterFormBuffPolymorph.m_TransitionExternal;

            c.StrengthBonus = -2;
            c.DexterityBonus = 2;
            c.ConstitutionBonus = 0;

            c.m_AdditionalLimbs = new[]
            {
                // Bite 1d6
                new BlueprintItemWeaponReference
                {
                    deserializedGuid = BlueprintGuid.Parse("a000716f88c969c499a535dadcf09286")
                },
            };

            return (buff) =>
            {
                // Because slots are kept but weapons are locked
                buff.AddComponent<EmptyHandWeaponOverride>(cmp =>
                {
                    // Claw 1d4
                    cmp.m_Weapon = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("118fdd03e569a66459ab01a20af6811a");
                });
            };
        }

        private Action<BlueprintBuff> ApplyMediumFormPolymorphSettings(Polymorph c)
        {
            var shifterFormBuff = BlueprintTools.GetBlueprint<BlueprintBuff>(ShifterFormMediumId);
            var shifterFormBuffPolymorph = (Polymorph)(shifterFormBuff.Components.Single(cmp => cmp is Polymorph));

            c.m_Prefab = shifterFormBuffPolymorph.m_Prefab;
            c.m_PrefabFemale = shifterFormBuffPolymorph.m_PrefabFemale;
            c.m_Portrait = shifterFormBuffPolymorph.m_Portrait;
            c.m_EnterTransition = shifterFormBuffPolymorph.m_EnterTransition;
            c.m_ExitTransition = shifterFormBuffPolymorph.m_ExitTransition;
            c.m_TransitionExternal = shifterFormBuffPolymorph.m_TransitionExternal;

            c.StrengthBonus = 0;
            c.DexterityBonus = 0;
            c.ConstitutionBonus = 0;

            c.m_AdditionalLimbs = new[]
            {
                // Bite 1d8
                new BlueprintItemWeaponReference
                {
                    deserializedGuid = BlueprintGuid.Parse("61bc14eca5f8c1040900215000cfc218")
                },
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

            return (buff) =>
            {
                // Because slots are kept but weapons are locked
                buff.AddComponent<EmptyHandWeaponOverride>(cmp =>
                {
                    // Claw 1d6
                    cmp.m_Weapon = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("65eb73689b94d894080d33a768cdf645");
                });
            };
        }

        private Action<BlueprintBuff> ApplyLargeFormPolymorphSettings(Polymorph c)
        {
            var shifterFormBuff = BlueprintTools.GetBlueprint<BlueprintBuff>(ShifterFormLargeId);
            var shifterFormBuffPolymorph = (Polymorph)(shifterFormBuff.Components.Single(cmp => cmp is Polymorph));

            c.m_Prefab = shifterFormBuffPolymorph.m_Prefab;
            c.m_PrefabFemale = shifterFormBuffPolymorph.m_PrefabFemale;
            c.m_Portrait = shifterFormBuffPolymorph.m_Portrait;
            c.m_EnterTransition = shifterFormBuffPolymorph.m_EnterTransition;
            c.m_ExitTransition = shifterFormBuffPolymorph.m_ExitTransition;
            c.m_TransitionExternal = shifterFormBuffPolymorph.m_TransitionExternal;

            c.StrengthBonus = 4;
            c.DexterityBonus = -2;
            c.ConstitutionBonus = 2;

            c.m_AdditionalLimbs = new[]
            {
                // Bite 2d6
                new BlueprintItemWeaponReference
                {
                    deserializedGuid = BlueprintGuid.Parse("2abc1dc6172759c42971bd04b8c115cb")
                },
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

            return (buff) =>
            {
                // Because slots are kept but weapons are locked
                buff.AddComponent<EmptyHandWeaponOverride>(cmp =>
                {
                    // Claw 1d8
                    cmp.m_Weapon = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("13a4ac62fe603fc4c99f9ed5e5d0b9d6");
                });
            };
        }

        private Action<BlueprintBuff> ApplyHugeFormPolymorphSettings(Polymorph c)
        {
            var shifterFormBuff = BlueprintTools.GetBlueprint<BlueprintBuff>(ShifterFormHugeId);
            var shifterFormBuffPolymorph = (Polymorph)(shifterFormBuff.Components.Single(cmp => cmp is Polymorph));

            c.m_Prefab = shifterFormBuffPolymorph.m_Prefab;
            c.m_PrefabFemale = shifterFormBuffPolymorph.m_PrefabFemale;
            c.m_Portrait = shifterFormBuffPolymorph.m_Portrait;
            c.m_EnterTransition = shifterFormBuffPolymorph.m_EnterTransition;
            c.m_ExitTransition = shifterFormBuffPolymorph.m_ExitTransition;
            c.m_TransitionExternal = shifterFormBuffPolymorph.m_TransitionExternal;

            c.StrengthBonus = 8;
            c.DexterityBonus = -4;
            c.ConstitutionBonus = 4;

            c.m_AdditionalLimbs = new[]
            {
                // Bite 2d6 // Todo: upgrade to 2d8
                new BlueprintItemWeaponReference
                {
                    deserializedGuid = BlueprintGuid.Parse("2abc1dc6172759c42971bd04b8c115cb")
                },
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

            return (buff) =>
            {
                // Because slots are kept but weapons are locked
                buff.AddComponent<EmptyHandWeaponOverride>(cmp =>
                {
                    // Claw 2d6
                    cmp.m_Weapon = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("d498b2af675bd3447a0ab65ccc34d952");
                });
            };
        }

        private Action<BlueprintBuff> ApplyGargantuanFormPolymorphSettings(Polymorph c)
        {
            var shifterFormBuff = BlueprintTools.GetBlueprint<BlueprintBuff>(ShifterFormHugeId);
            var shifterFormBuffPolymorph = (Polymorph)(shifterFormBuff.Components.Single(cmp => cmp is Polymorph));

            c.m_Prefab = shifterFormBuffPolymorph.m_Prefab;
            c.m_PrefabFemale = shifterFormBuffPolymorph.m_PrefabFemale;
            c.m_Portrait = shifterFormBuffPolymorph.m_Portrait;
            c.m_EnterTransition = shifterFormBuffPolymorph.m_EnterTransition;
            c.m_ExitTransition = shifterFormBuffPolymorph.m_ExitTransition;
            c.m_TransitionExternal = shifterFormBuffPolymorph.m_TransitionExternal;

            c.StrengthBonus = 12;
            c.DexterityBonus = -4;
            c.ConstitutionBonus = 6;

            c.m_AdditionalLimbs = new[]
            {
                // Bite 2d6 // Todo: upgrade to 4d6
                new BlueprintItemWeaponReference
                {
                    deserializedGuid = BlueprintGuid.Parse("2abc1dc6172759c42971bd04b8c115cb")
                },
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

            return (buff) =>
            {
                // Because slots are kept but weapons are locked
                buff.AddComponent<EmptyHandWeaponOverride>(cmp =>
                {
                    // Claw 2d8
                    cmp.m_Weapon = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("bd440fff6bfc3954aac8b6e59a9d7489");
                });
            };
        }

        private Action<BlueprintBuff> ApplyCollossalFormPolymorphSettings(Polymorph c)
        {
            var shifterFormBuff = BlueprintTools.GetBlueprint<BlueprintBuff>(ShifterFormHugeId);
            var shifterFormBuffPolymorph = (Polymorph)(shifterFormBuff.Components.Single(cmp => cmp is Polymorph));

            c.m_Prefab = shifterFormBuffPolymorph.m_Prefab;
            c.m_PrefabFemale = shifterFormBuffPolymorph.m_PrefabFemale;
            c.m_Portrait = shifterFormBuffPolymorph.m_Portrait;
            c.m_EnterTransition = shifterFormBuffPolymorph.m_EnterTransition;
            c.m_ExitTransition = shifterFormBuffPolymorph.m_ExitTransition;
            c.m_TransitionExternal = shifterFormBuffPolymorph.m_TransitionExternal;

            c.StrengthBonus = 16;
            c.DexterityBonus = -4;
            c.ConstitutionBonus = 8;

            c.m_AdditionalLimbs = new[]
            {
                // Bite 2d6 // Todo: upgrade to 4d8
                new BlueprintItemWeaponReference
                {
                    deserializedGuid = BlueprintGuid.Parse("2abc1dc6172759c42971bd04b8c115cb")
                },
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

            return (buff) =>
            {
                // Because slots are kept but weapons are locked
                buff.AddComponent<EmptyHandWeaponOverride>(cmp =>
                {
                    // Claw 2d8 // Todo: upgrade to 4d6
                    cmp.m_Weapon = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("bd440fff6bfc3954aac8b6e59a9d7489");
                });
            };
        }

        private BlueprintFeature GetBonusSpellsFeature(DragonAge age)
        {
            var ageCategory = age.Name.Replace(" ", "");
            var feature = Helpers.CreateBlueprint<BlueprintFeature>(DragonModContext, $"DragonBloodline{BloodlineName}BonusSpells{ageCategory}Feature", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(DragonModContext, $"DragonBloodline{BloodlineName}BonusSpells{ageCategory}Feature.Name", $"{BloodlineName} Dragon Bonus Spells ({ageCategory})");
                bp.m_Description = Helpers.CreateString(DragonModContext, $"DragonBloodline{BloodlineName}BonusSpells{ageCategory}LevelFeature.Description", "At certain levels, you gain access to spells that represent the power of your dragon heritage. These spells are added to your spellbook and can be prepared and cast as normal.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                foreach (var spell in age.BonusSpells)
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

        private void ProcessAgeBasedStatBonusesForBaseCharacter(DragonAge age, LevelEntry levelEntry)
        {
            // Here's how the math works
            // Wyrmlings are the baseline. Age number is 1. Very Young is 2, Young is 3, etc.
            // See "Table: Dragon Ability Scores" on https://www.d20pfsrd.com/bestiary/monster-listings/dragons/dragon/
            // For every dragon after Wyrmling,
            // STR = Age * 2 + 2
            // (I'm not finishing the table since I did different math on paper)
            // When we combine the above table with "Table: Ability Adjustments from Size Changes" on https://www.d20pfsrd.com/magic#TOC-Transmutation,
            // and fudge it a little, we get stat adjustments to apply to a Medium character
            // if we pretend the dragon is the true form and the base character is the polymorph
            //  1: (base)
            //  2: STR+2, DEX+0, CON+2
            //  3: STR+4, DEX+0, CON+2
            //  4: STR+4, DEX+0, CON+2
            //  5: STR+4, DEX+0, CON+2
            //  6: STR+6, DEX+0, CON+4
            //  7: STR+6, DEX-2, CON+4
            //  8: STR+8, DEX-2, CON+4
            //  9: STR+8, DEX-2, CON+4
            // 10: STR+10, DEX-2, CON+6
            // 11: STR+10, DEX-4, CON+6
            // 12: STR+12, DEX-4, CON+6
            // We will then apply "Table: Ability Adjustments from Size Changes" in reverse to get the stats for the true dragon forms
            // Mental abilities are easy since shapeshifting doesn't affect them

            // Natural armor is simply increased by 3 each level

            // Tl;dr: these increases represent the increases to the dragon form minus the polymorph adjustment
            // You're a true dragon, but limited by the mortal form you've polymorphed into
            // We'll use the dragon form to put things back as Apsu intended


            switch (age.Name)
            {
                // 2
                case DragonAgeName.VeryYoung:
                    levelEntry.m_Features.Add(DragonNaturalArmorFeature.GetReference<BlueprintFeatureBaseReference>());
                    levelEntry.m_Features.Add(DragonStrengthFeature.GetReference<BlueprintFeatureBaseReference>());
                    levelEntry.m_Features.Add(DragonConstitutionFeature.GetReference<BlueprintFeatureBaseReference>());
                    levelEntry.m_Features.Add(DragonIntelligenceFeature.GetReference<BlueprintFeatureBaseReference>());
                    levelEntry.m_Features.Add(DragonWisdomFeature.GetReference<BlueprintFeatureBaseReference>());
                    levelEntry.m_Features.Add(DragonCharismaFeature.GetReference<BlueprintFeatureBaseReference>());
                    break;
                // 3
                case DragonAgeName.Young:
                    levelEntry.m_Features.Add(DragonNaturalArmorFeature.GetReference<BlueprintFeatureBaseReference>());
                    levelEntry.m_Features.Add(DragonStrengthFeature.GetReference<BlueprintFeatureBaseReference>());
                    break;
                // 4
                case DragonAgeName.Juvenile:
                    levelEntry.m_Features.Add(DragonNaturalArmorFeature.GetReference<BlueprintFeatureBaseReference>());
                    levelEntry.m_Features.Add(DragonIntelligenceFeature.GetReference<BlueprintFeatureBaseReference>());
                    levelEntry.m_Features.Add(DragonWisdomFeature.GetReference<BlueprintFeatureBaseReference>());
                    levelEntry.m_Features.Add(DragonCharismaFeature.GetReference<BlueprintFeatureBaseReference>());
                    break;
                // 5
                case DragonAgeName.YoungAdult:
                    levelEntry.m_Features.Add(DragonNaturalArmorFeature.GetReference<BlueprintFeatureBaseReference>());
                    break;
                // 6
                case DragonAgeName.Adult:
                    levelEntry.m_Features.Add(DragonNaturalArmorFeature.GetReference<BlueprintFeatureBaseReference>());
                    levelEntry.m_Features.Add(DragonStrengthFeature.GetReference<BlueprintFeatureBaseReference>());
                    levelEntry.m_Features.Add(DragonConstitutionFeature.GetReference<BlueprintFeatureBaseReference>());
                    levelEntry.m_Features.Add(DragonIntelligenceFeature.GetReference<BlueprintFeatureBaseReference>());
                    levelEntry.m_Features.Add(DragonWisdomFeature.GetReference<BlueprintFeatureBaseReference>());
                    levelEntry.m_Features.Add(DragonCharismaFeature.GetReference<BlueprintFeatureBaseReference>());
                    break;
                // 7
                case DragonAgeName.MatureAdult:
                    levelEntry.m_Features.Add(DragonNaturalArmorFeature.GetReference<BlueprintFeatureBaseReference>());
                    levelEntry.m_Features.Add(DragonDexterityFeature.GetReference<BlueprintFeatureBaseReference>());
                    break;
                // 8
                case DragonAgeName.Old:
                    levelEntry.m_Features.Add(DragonNaturalArmorFeature.GetReference<BlueprintFeatureBaseReference>());
                    levelEntry.m_Features.Add(DragonStrengthFeature.GetReference<BlueprintFeatureBaseReference>());
                    levelEntry.m_Features.Add(DragonIntelligenceFeature.GetReference<BlueprintFeatureBaseReference>());
                    levelEntry.m_Features.Add(DragonWisdomFeature.GetReference<BlueprintFeatureBaseReference>());
                    levelEntry.m_Features.Add(DragonCharismaFeature.GetReference<BlueprintFeatureBaseReference>());
                    break;
                // 9
                case DragonAgeName.VeryOld:
                    levelEntry.m_Features.Add(DragonNaturalArmorFeature.GetReference<BlueprintFeatureBaseReference>());
                    levelEntry.m_Features.Add(DragonStrengthFeature.GetReference<BlueprintFeatureBaseReference>());
                    break;
                // 10
                case DragonAgeName.Ancient:
                    levelEntry.m_Features.Add(DragonNaturalArmorFeature.GetReference<BlueprintFeatureBaseReference>());
                    levelEntry.m_Features.Add(DragonStrengthFeature.GetReference<BlueprintFeatureBaseReference>());
                    levelEntry.m_Features.Add(DragonConstitutionFeature.GetReference<BlueprintFeatureBaseReference>());
                    levelEntry.m_Features.Add(DragonIntelligenceFeature.GetReference<BlueprintFeatureBaseReference>());
                    levelEntry.m_Features.Add(DragonWisdomFeature.GetReference<BlueprintFeatureBaseReference>());
                    levelEntry.m_Features.Add(DragonCharismaFeature.GetReference<BlueprintFeatureBaseReference>());
                    break;
                // 11
                case DragonAgeName.Wyrm:
                    levelEntry.m_Features.Add(DragonNaturalArmorFeature.GetReference<BlueprintFeatureBaseReference>());
                    levelEntry.m_Features.Add(DragonDexterityFeature.GetReference<BlueprintFeatureBaseReference>());
                    break;
                // 12
                case DragonAgeName.GreatWyrm:
                    levelEntry.m_Features.Add(DragonNaturalArmorFeature.GetReference<BlueprintFeatureBaseReference>());
                    levelEntry.m_Features.Add(DragonIntelligenceFeature.GetReference<BlueprintFeatureBaseReference>());
                    levelEntry.m_Features.Add(DragonWisdomFeature.GetReference<BlueprintFeatureBaseReference>());
                    levelEntry.m_Features.Add(DragonCharismaFeature.GetReference<BlueprintFeatureBaseReference>());
                    break;
            }
        }
    }
}
