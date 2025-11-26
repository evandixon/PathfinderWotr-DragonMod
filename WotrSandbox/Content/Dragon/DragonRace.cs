using Kingmaker.Blueprints;
using Kingmaker.Blueprints.CharGen;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums;
using Kingmaker.Localization;
using Kingmaker.ResourceLinks;
using Kingmaker.View.Animation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using WotrSandbox.Infrastructure;
using static WotrSandbox.Main;

namespace WotrSandbox.Content.Dragon
{
    public static class DragonRace
    {
        private static readonly LocalizedString Name = Helpers.CreateString(IsekaiContext, $"DragonRace.Name", "Dragon");
        private static readonly LocalizedString Description = Helpers.CreateString(IsekaiContext, $"DragonRace.Description",
            "Dragon");
        private static readonly LocalizedString DescriptionShort = Helpers.CreateString(IsekaiContext, $"DragonRace.DescriptionShort",
            "Dragon");
        private static BlueprintCharacterClass dragonClass;

        public static void Add()
        {
            var dragonRace = Helpers.CreateBlueprint<BlueprintRace>(IsekaiContext, "DragonRace", bp =>
            {
                // Basic fields
                bp.m_Overrides = new List<string>();
                bp.Components = Array.Empty<BlueprintComponent>();
                bp.Comment = "";
                bp.m_AllowNonContextActions = false;

                // DisplayName / Description
                bp.m_DisplayName = Name;
                bp.m_Description = Description;
                bp.m_DescriptionShort = DescriptionShort;
                bp.m_Icon = null;

                // UI flags
                bp.HideInUI = false;
                bp.HideInCharacterSheetAndLevelUp = false;
                bp.HideNotAvailibleInUI = false;

                // Groups / Ranks / Feature flags
                bp.Groups = Array.Empty<FeatureGroup>();
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = false;
                bp.IsClassFeature = true;

                // Prerequisites
                bp.IsPrerequisiteFor = new List<BlueprintFeatureReference>();

                // Race fields
                bp.SoundKey = "Human";
                bp.RaceId = Race.Kitsune;
                bp.SelectableRaceStat = false;
                bp.Size = Size.Medium;

                // Features & Presets
                bp.m_Features = new BlueprintFeatureBaseReference[0];

                bp.m_Presets = new BlueprintRaceVisualPresetReference[0];

                // Colors / Links
                bp.LinkHairAndSkinColorsCondition = null;

                // Male visual options
                bp.MaleOptions = new CustomizationOptions
                {
                    m_Heads = new EquipmentEntityLink[0],
                    m_Eyebrows = new EquipmentEntityLink[0],
                    m_Hair = new EquipmentEntityLink[0],
                    Beards = new EquipmentEntityLink[0],
                    Horns = new EquipmentEntityLink[0],
                    TailSkinColors = new EquipmentEntityLink[0]
                };

                // Female visual options
                bp.FemaleOptions = new CustomizationOptions
                {
                    m_Heads = new EquipmentEntityLink[0],
                    m_Eyebrows = new EquipmentEntityLink[0],
                    m_Hair = new EquipmentEntityLink[0],
                    Beards = new EquipmentEntityLink[0],
                    Horns = new EquipmentEntityLink[0],
                    TailSkinColors = new EquipmentEntityLink[0]
                };

                // Male speed settings
                bp.MaleSpeedSettings = new UnitAnimationSettings
                {
                    MovementSpeedCoeff = 1.0f,
                    OverrideSlowWalk = true,
                    SlowWalkCoeff = 2.0f,
                    OverrideSlowWalkNonCombat = true,
                    SlowWalkNonCombatCoeff = 1.8f,
                    OverrideNormal = true,
                    NormalCoeff = 0.85f,
                    OverrideNormalNonCombat = true,
                    NormalNonCombatCoeff = 0.85f,
                    OverrideCharge = true,
                    ChargeCoeff = 0.5f,
                    OverrideChargeNonCombat = false,
                    ChargeNonCombatCoeff = 1.0f,
                    OverrideStealth = false,
                    StealthCoeff = 1.0f,
                    OverrideStealthNonCombat = false,
                    StealthNonCombatCoeff = 1.0f
                };

                // Female speed settings
                bp.FemaleSpeedSettings = new UnitAnimationSettings
                {
                    MovementSpeedCoeff = 1.0f,
                    OverrideSlowWalk = true,
                    SlowWalkCoeff = 2.1f,
                    OverrideSlowWalkNonCombat = true,
                    SlowWalkNonCombatCoeff = 1.8f,
                    OverrideNormal = true,
                    NormalCoeff = 0.87f,
                    OverrideNormalNonCombat = true,
                    NormalNonCombatCoeff = 0.87f,
                    OverrideCharge = true,
                    ChargeCoeff = 0.5f,
                    OverrideChargeNonCombat = false,
                    ChargeNonCombatCoeff = 1.0f,
                    OverrideStealth = false,
                    StealthCoeff = 1.0f,
                    OverrideStealthNonCombat = false,
                    StealthNonCombatCoeff = 1.0f
                };

                // Special doll types
                bp.SpecialDollTypes = new BlueprintRace.SpecialDollTypeEntry[0];
            });

            TTCoreExtensions.ReggisterRace(dragonRace);
        }
    }
}
