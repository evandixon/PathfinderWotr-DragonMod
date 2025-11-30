using DragonMod.Content.Dragon;
using DragonMod.Content.Dragon.Bloodlines;
using DragonMod.Content.Dragon.Features;
using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Root;
using Kingmaker.Controllers.Combat;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.Dungeon;
using Kingmaker.Dungeon.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using UnityEngine;
using static TabletopTweaks.Core.Utilities.BloodlineTools;
using static UnityEngine.UI.GridLayoutGroup;

namespace DragonMod.Content
{
    public static class XpTablePatcher
    {
        public static void Patch()
        {
            return;
            var xpTable = BlueprintTools.GetBlueprint<BlueprintStatProgression>("11c77f6853ac46aa8e2d004d6dca5f9f");

            /* For comparison, this is the base XP table:
               00: 0, (dummy)
               01: 0,
               02: 2000,
               03: 5000,
               04: 9000,
               05: 15000,
               06: 23000,
               07: 35000,
               08: 51000,
               09: 75000,
               10: 105000,
               11: 155000,
               12: 220000,
               13: 315000,
               14: 445000,
               15: 635000,
               16: 890000,
               17: 1300000,
               18: 1800000,
               19: 2550000,
               20: 3600000

              And this is the default Legend table:
              00: 0, (dummy)
              01: 0,
              02: 2000,
              03: 5000,
              04: 9000,
              05: 15000,
              06: 23000,
              07: 35000,
              08: 51000,
              -- here is where it first diverges from the default
              09: 55000,
              10: 62000,
              11: 68000,
              12: 75000,
              13: 85000,
              14: 96000,
              15: 105000,
              16: 115000,
              17: 130000,
              18: 155000,
              19: 180000,
              20: 200000,
              21: 220000,
              22: 260000,
              23: 280000,
              24: 315000,
              25: 370000,
              26: 445000,
              27: 500000,
              28: 635000,
              29: 720000,
              30: 890000,
              31: 1000000,
              32: 1300000,
              33: 1550000,
              34: 1800000,
              35: 2000000,
              36: 2550000,
              37: 3000000,
              38: 3600000,
              39: 4050000,
              40: 4700000
            */
            xpTable.Bonuses = CustomLegendTable;
        }

        public static int[] GetCustomXPTable(int startingHD, int maxLevel)
        {
            // Chat GPT's rough approximation of XP for a given level is XP(L) ≈ 185 × (L − 1)^3.1
            // This is only an approximation, but for a first pass it should be close enough to begin playtesting

            var xpTable = new List<int>(maxLevel);

            // Start out at the wyrmling dragon's starting HD
            // Character builder will start out at 1, and the player will have the opportunity to level up to 7 more levels afterward
            for (int i = 0; i <= startingHD; i++)
            {
                xpTable.Add(0);
            }

            // After the start, we have 11 more age categories spaced 2 HD apart
            // Basically we have 22 more levels
            // For a first pass, let's keep this similar to the base 1-20 curve
            // For the last two levels we'll borrow from the legend table
            xpTable.AddRange(new[]
            {
                // Base XP curve starting at 2
                2000,
                5000,
                9000,
                15000,
                23000,
                35000,
                51000,
                75000,
                105000,
                155000,
                220000,
                315000,
                445000,
                635000,
                890000,
                1300000,
                1800000,
                2550000,
                3600000, // Level 20

                // Legend numbers to continue the curve
                4050000,
                4700000
            });

            return xpTable.ToArray();
        }

        public static readonly int[] CustomLegendTable = new int[41]
            {
                /*00:*/ 0,

                // The first few levels before shapeshifting is unlocked
                // This should move quick
                /*01:*/ 0,
                /*02:*/ 1000,
                /*03:*/ 3000,
                /*04:*/ 2000,
                /*05:*/ 3500,
                /*06:*/ 5000,

                // Wyrmling form is unlocked around here
                /*07:*/ 7000,
                /*08:*/ 9000,

                /*09:*/ 12000,
                /*10:*/ 15000,
                /*11:*/ 19000,
                /*12:*/ 23000,
                /*13:*/ 29000,
                /*14:*/ 35000,
                /*15:*/ 43000,
                /*16:*/ 51000,
                /*17:*/ 63000,
                /*18:*/ 75000,
                /*19:*/ 90000,
                /*20:*/ 105000,
                /*21:*/ 130000,
                /*22:*/ 155000,
                /*23:*/ 167000,
                /*24:*/ 220000,
                /*25:*/ 257500,
                /*26:*/ 315000,
                /*27:*/ 380000,
                /*28:*/ 445000,
                /*29:*/ 540000,
                /*30:*/ 635000,
                /*31:*/ 762500,
                /*32:*/ 890000,
                /*33:*/ 1095000,
                /*34:*/ 1300000,
                /*35:*/ 1550000,
                /*36:*/ 1800000,
                /*37:*/ 2175000,
                /*38:*/ 2550000,
                /*39:*/ 3075000,
                /*40:*/ 3600000
            };
    }

    // Control which XP table to use
    [HarmonyPatch(typeof(UnitProgressionData), nameof(UnitProgressionData.ExperienceTable), MethodType.Getter)]
    public static class ExperienceTable_Getter_PrefixPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(UnitProgressionData __instance, ref BlueprintStatProgression __result)
        {
            //Main.Log("DragonMod patch - UnitProgressionData.ExperienceTable entered");
            if (!ContentAdder.BlueprintsCache_Init_Patch.Initialized)
            {
                //Main.Log("DragonMod patch - UnitProgressionData.ExperienceTable patch skipped");
                return true;
            }

            //Main.Log("DragonMod patch - UnitProgressionData.ExperienceTable - " + string.Join(",", __instance.Classes.Select(c => c.CharacterClass.AssetGuid.ToString())));

            if (ShouldOverride(__instance))
            {
                Main.Log("DragonMod patch - UnitProgressionData.ExperienceTable - Override applied");
                var expTable = GetCustomExperienceTable(__instance);
                if (expTable != null)
                {
                    __result = expTable;
                    return false;
                }
            }

            return true;
        }

        private static bool ShouldOverride(UnitProgressionData data)
        {
            var dragonClassReference = DragonClass.GetReference();
            return data.Classes.Any(c => c.CharacterClass.AssetGuid == dragonClassReference.Guid);
        }

        private static BlueprintStatProgression GetCustomExperienceTable(UnitProgressionData data)
        {
            var dragonClassReference = DragonClass.GetReference();
            var dragonClassInstance = data.Classes.First(c => c.CharacterClass.AssetGuid == dragonClassReference.Guid);
            foreach (var archetype in dragonClassInstance.Archetypes)
            {
                var bloodline = BloodlineList.Bloodlines.FirstOrDefault(b => b.GetReference<BlueprintArchetypeReference>().Guid == archetype.AssetGuid);
                if (bloodline == null)
                {
                    continue;
                }

                return new BlueprintStatProgression
                {
                    AssetGuid = BlueprintGuid.Parse("11c77f6853ac46aa8e2d004d6dca5f9f"),
                    Bonuses = bloodline.GetXpTable()
                };
            }

            return null;
        }
    }

    [HarmonyPatch(typeof(UnitProgressionData), nameof(UnitProgressionData.MaxAvailableCharacterLevel), MethodType.Getter)]
    public static class MaxAvailableCharacterLevelPatcher
    {
        [HarmonyPrefix]
        public static bool Prefix(UnitProgressionData __instance, ref int __result)
        {
            //Main.Log("DragonMod patch - UnitProgressionData.MaxAvailableCharacterLevel entered");
            if (!ContentAdder.BlueprintsCache_Init_Patch.Initialized)
            {
                //Main.Log("DragonMod patch - UnitProgressionData.MaxAvailableCharacterLevel patch skipped");
                return true;
            }

            //Main.Log("DragonMod patch - UnitProgressionData.MaxAvailableCharacterLevel - " + string.Join(",", __instance.Classes.Select(c => c.CharacterClass.AssetGuid.ToString())));
            if (ShouldOverride(__instance))
            {
                var level = GetCustomMaxLevel(__instance);
                if (level.HasValue)
                {
                    Main.Log("DragonMod patch - UnitProgressionData.MaxAvailableCharacterLevel - Override applied");
                    __result = level.Value;
                    return false;
                }
            }

            return true;
        }

        private static bool ShouldOverride(UnitProgressionData data)
        {
            var dragonClassReference = DragonClass.GetReference();
            return data.Classes.Any(c => c.CharacterClass.AssetGuid == dragonClassReference.Guid);
        }

        private static int? GetCustomMaxLevel(UnitProgressionData data)
        {
            var dragonClassReference = DragonClass.GetReference();
            var dragonClassInstance = data.Classes.First(c => c.CharacterClass.AssetGuid == dragonClassReference.Guid);
            foreach (var archetype in dragonClassInstance.Archetypes)
            {
                var bloodline = BloodlineList.Bloodlines.FirstOrDefault(b => b.GetReference<BlueprintArchetypeReference>().Guid == archetype.AssetGuid);
                if (bloodline == null)
                {
                    continue;
                }

                return bloodline.AgeCategories.Last().HitDice;
            }

            return null;
        }
    }

    //[HarmonyPatch(typeof(UnitProgressionData), "GetClassLevel")]
    //public static class GetClassLevel_Getter_PrefixPatch
    //{
    //    [HarmonyPostfix]
    //    public static void Postfix(UnitProgressionData __instance, BlueprintCharacterClass characterClass, ref int __result)
    //    {
    //        if (__result > 0)
    //        {
    //            return;
    //        }

    //        // The game uses GetClassLevel(BlueprintRoot.Instance.Progression.MythicLegen) sometimes instead of LegendaryHero
    //        if (characterClass == BlueprintRoot.Instance.Progression.MythicLegen && __instance.Owner.State.Features.LegendaryHero)
    //        {
    //            __result = 1;
    //        }
    //    }
    //}

    // Patch the game so that it consistently uses the property we patched

    [HarmonyPatch(typeof(UnitExperienceForLevel), nameof(UnitExperienceForLevel.GetValueInternal))]
    public static class UnitExperienceForLevelPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(UnitExperienceForLevel __instance, ref int __result)
        {
            //Main.Log("DragonMod patch - UnitExperienceForLevel.GetValueInternal");

            if (__instance.Unit == null || !__instance.Unit.CanEvaluate() || !__instance.Unit.TryGetValue(out UnitEntityData value))
            {
                __result = 0;
                return false;
            }
            if (__instance.Level == null || !__instance.Level.CanEvaluate() || !__instance.Level.TryGetValue(out int value2))
            {
                __result = 0;
                return false;
            }

            __result = value.Progression.ExperienceTable.GetBonus(value2);
            return false;
        }
    }

    [HarmonyPatch(typeof(AdvanceUnitLevel), nameof(AdvanceUnitLevel.RunAction))]
    public static class AdvanceUnitLevelPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(AdvanceUnitLevel __instance)
        {
            //Main.Log("DragonMod patch - AdvanceUnitLevel.RunAction");

            UnitEntityData value = __instance.Unit.GetValue();
            int[] obj = value.Progression.ExperienceTable.Bonuses;
            int targetExp = obj[Math.Min(obj.Length - 1, __instance.Level.GetValue())];
            value.Descriptor.Progression.AdvanceExperienceTo(targetExp, log: false);
            return false;
        }
    }

    [HarmonyPatch(typeof(LevelUpUnit), nameof(LevelUpUnit.RunAction))]
    public static class LevelUpUnitPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(LevelUpUnit __instance)
        {
            //Main.Log("DragonMod patch - LevelUpUnit.RunAction");

            UnitEntityData value = __instance.Unit.GetValue();
            int bonus = value.Progression.ExperienceTable.GetBonus(__instance.TargetLevel.GetValue());
            value.Descriptor.Progression.AdvanceExperienceTo(bonus);
            return false;
        }
    }

    [HarmonyPatch(typeof(DungeonController), nameof(DungeonController.CreateMainCharacter))]
    public static class DungeonControllerPatch 
    {
        [HarmonyPrefix]
        public static bool Prefix()
        {
            //Main.Log("DragonMod patch - DungeonController.CreateMainCharacter");

            Vector3 position = Game.Instance.Player.MainCharacter.Value.Position;
            float orientation = Game.Instance.Player.MainCharacter.Value.Orientation;
            UnitEntityData unit = Game.Instance.CreateUnitVacuum(Game.Instance.Player.StartPreset.PlayerCharacter);
            Game.Instance.Player.RemoveEverybody();
            LevelUpConfig.Create(unit, LevelUpState.CharBuildMode.CharGen).SetLockInUi(lockInUi: true).SetOnCommit(delegate (LevelUpController _controller)
            {
                unit = _controller.Unit;
                Game.Instance.Player.CrossSceneState.AddEntityData(unit);
                Game.Instance.Player.RemoveCompanions();
                Game.Instance.Player.UpdateCharacterLists();
                if (Game.Instance.Player.DungeonState.LevelUps != 0)
                {
                    int bonus = ((unit.Progression.ExperienceTable).GetBonus(unit.Descriptor.Progression.CharacterLevel + Game.Instance.Player.DungeonState.LevelUps));
                    unit.Descriptor.Progression.AdvanceExperienceTo(bonus);
                    Game.Instance.Player.DungeonState.LevelUps = 0;
                }
                unit.Position = position;
                unit.Orientation = orientation;
                unit.State.Size = unit.OriginalSize;
                unit.AttachToViewOnLoad(null);
                Game.Instance.DynamicRoot.Add(unit.View.transform);
                Game.Instance.GetController<GameOverController>(includeInactive: true).IsGameOverIgnored.Release();
                Game.Instance.Player.DungeonState.ApplyBoon(unit);
                BlueprintDungeonRoot.Instance.ActionsAfterChargen?.Run();
            })
                .OpenUI();
            return false;
        }


        [HarmonyPatch(typeof(LevelUpController), nameof(LevelUpController.GetEffectiveLevel))]
        public static class LevelUpControllerPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(UnitEntityData unit, ref int __result)
            {
                //Main.Log("DragonMod patch - LevelUpController.GetEffectiveLevel");

                unit = (unit ?? Game.Instance.Player.MainCharacter.Value);
                if (unit == null)
                {
                    __result = 1;
                    return false;
                }
                int i = unit.Progression.CharacterLevel;
                int experience = unit.Progression.Experience;
                for (BlueprintStatProgression blueprintStatProgression = (unit.Progression.ExperienceTable); i < 20 && blueprintStatProgression.GetBonus(i + 1) <= experience; i++)
                {
                }
                __result = i;
                return false;
            }
        }
    }
}
