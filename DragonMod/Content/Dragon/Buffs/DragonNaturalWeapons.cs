using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace DragonMod.Content.Dragon.Buffs
{
    public static class DragonNaturalWeapons
    {
        private static readonly LocalizedString Name = Helpers.CreateString(Main.DragonModContext, $"DragonNaturalWeaponsBuff.Name", "Half Dragon Natural Weapons");
        private static readonly LocalizedString Description = Helpers.CreateString(Main.DragonModContext, $"DragonNaturalWeaponsBuff.Description",
            "A half dragon gets 1 bite and 2x claws");
        private static readonly LocalizedString DescriptionShort = Helpers.CreateString(Main.DragonModContext, $"DragonNaturalWeaponsBuff.DescriptionShort",
            "Half dragon natural weapons.");
        public static void Add()
        {
            var naturalWeaponsBuff = Helpers.CreateBlueprint<BlueprintBuff>(Main.DragonModContext, "DragonNaturalWeaponsBuff", bp =>
            {
                bp.m_DisplayName = Name;
                bp.m_Description = Description;
                bp.m_DescriptionShort = DescriptionShort;

                bp.IsClassFeature = true;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi | BlueprintBuff.Flags.StayOnDeath;
                bp.Stacking = StackingType.Replace;
                bp.Frequency = Kingmaker.UnitLogic.Mechanics.DurationRate.Rounds;

                var bite1d6 = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("a000716f88c969c499a535dadcf09286");
                var claw1d4 = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("118fdd03e569a66459ab01a20af6811a");
                bp.AddComponent<AddAdditionalLimb>(l =>
                {
                    // Bite 1d6
                    l.m_Weapon = bite1d6;
                });

                // Maybe redundant with EmptyHandWeaponOverride
                //bp.AddComponent<AddAdditionalLimb>(l =>
                //{
                //    // Claw 1d4
                //    l.m_Weapon = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("118fdd03e569a66459ab01a20af6811a");
                //});
                //bp.AddComponent<AddAdditionalLimb>(l =>
                //{
                //    // Claw 1d4
                //    l.m_Weapon = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("118fdd03e569a66459ab01a20af6811a");
                //});

                bp.AddComponent<EmptyHandWeaponOverride>(c =>
                {
                    c.m_Weapon = claw1d4;
                });
            });
        }

        public static BlueprintBuffReference GetReference()
        {
            return BlueprintTools.GetModBlueprintReference<BlueprintBuffReference>(Main.DragonModContext, "DragonNaturalWeaponsBuff");
        }
    }
}
