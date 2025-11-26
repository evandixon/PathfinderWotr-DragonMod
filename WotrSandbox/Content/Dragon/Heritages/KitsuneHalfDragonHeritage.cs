using WotrSandbox.Infrastructure;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using static WotrSandbox.Main;

namespace WotrSandbox.Content.Dragon.Heritages {

    internal class KitsuneHalfDragonHeritage {
        private static readonly BlueprintFeature DestinyBeyondBirthMythicFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("325f078c584318849bfe3da9ea245b9d");

        public static void Add() {
            var IsekaiFurryHeritage = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, "KitsuneHalfDragonHeritage", bp => {
                bp.SetName(IsekaiContext, "Kitsune Half-Dragon");
                bp.SetDescription(IsekaiContext, "A kitsune who has been altered in some way to become a half dragon.");
                bp.m_Icon = null;

                // Attributes
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Dexterity;
                    c.Value = 2;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Charisma;
                    c.Value = 2;
                });
                bp.AddComponent<AddStatBonusIfHasFact>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Strength;
                    c.Value = -2;
                    c.InvertCondition = true;
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] { DestinyBeyondBirthMythicFeat.ToReference<BlueprintUnitFactReference>() };
                });

                // Natural weapons
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[]
                    {
                        BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(IsekaiContext, "DragonNaturalWeaponsBuff")
                    };
                });

                // Dragon type (e.g. immunities)
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[]
                    {
                        BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("455ac88e22f55804ab87c2467deff1d6")
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


                //// Add Fast Healing
                //bp.AddComponent<AddEffectFastHealing>(c => {
                //    c.Heal = 0;
                //    c.Bonus = Values.CreateContextRankValue(AbilityRankType.StatBonus);
                //});
                //bp.AddComponent<ContextRankConfig>(c => {
                //    c.m_Type = AbilityRankType.StatBonus;
                //    c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
                //});

                //// Extra Speed
                //bp.AddComponent<AddStatBonus>(c => {
                //    c.Descriptor = ModifierDescriptor.Racial;
                //    c.Stat = StatType.Speed;
                //    c.Value = 10;
                //});

                bp.Groups = new FeatureGroup[0];
                bp.ReapplyOnLevelUp = true;
            });

            FeatTools.Selections.KitsuneHeritageSelection.AddToSelection(IsekaiFurryHeritage);
        }
    }
}