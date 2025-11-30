using DragonMod.Content.Dragon.Buffs;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using static DragonMod.Main;

namespace DragonMod.Content.Dragon.Features
{
    public static class HalfDragonFeature
    {
        public static void Add()
        {
            var halfDragonFeature = Helpers.CreateBlueprint<BlueprintFeature>(DragonModContext, "HalfDragonFeature", bp => {
                bp.SetName(DragonModContext, "Half-Dragon");
                bp.SetDescription(DragonModContext, "You have become a half dragon.");
                bp.m_Icon = null;

                // Attributes
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Strength;
                    c.Value = 8;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Constitution;
                    c.Value = 6;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Intelligence;
                    c.Value = 2;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Charisma;
                    c.Value = 2;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.NaturalArmor;
                    c.Stat = StatType.AC;
                    c.Value = 4;
                });

                // Natural weapons
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[]
                    {
                        new BlueprintUnitFactReference { deserializedGuid = DragonNaturalWeapons.GetReference().Guid }
                    };
                });

                //// Dragon type (e.g. immunities)
                //bp.AddComponent<AddFacts>(c => {
                //    c.m_Facts = new BlueprintUnitFactReference[]
                //    {
                //        BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("455ac88e22f55804ab87c2467deff1d6")
                //    };
                //});
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

                bp.Groups = new FeatureGroup[0];
                bp.ReapplyOnLevelUp = true;
            });
        }

        public static T GetReference<T>() where T : BlueprintReferenceBase
        {
            return BlueprintTools.GetModBlueprintReference<T>(DragonModContext, "HalfDragonFeature");
        }
    }
}
