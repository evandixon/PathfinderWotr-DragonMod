using DragonMod.Infrastructure;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using System.Collections.Generic;
using TabletopTweaks.Core.Utilities;

namespace DragonMod.Content.Dragon.Bloodlines
{
    public class BaseDragonSpellbook
    {
        public static BlueprintSpellbook Add(string bloodlineName, int startingLevel)
        {
            var spellsKnown = Helpers.CreateBlueprint<BlueprintSpellsTable>(Main.DragonModContext, $"DragonBloodline{bloodlineName}SpellsKnown", bp =>
            {
                bp.Levels = new SpellsLevelEntry[]
                {
                    new SpellsLevelEntry { Count = new int[] {} }, // Level 0
                    //new SpellsLevelEntry { Count = new int[] {} }, // Level 1
                    //new SpellsLevelEntry { Count = new int[] {} }, // Level 2
                    //new SpellsLevelEntry { Count = new int[] {} }, // Level 3
                    //new SpellsLevelEntry { Count = new int[] {} }, // Level 4
                    //new SpellsLevelEntry { Count = new int[] {} }, // Level 5
                    //new SpellsLevelEntry { Count = new int[] {} }, // Level 6
                    //new SpellsLevelEntry { Count = new int[] {} }, // Level 7
                    //new SpellsLevelEntry { Count = new int[] {} }, // Level 8
                    //new SpellsLevelEntry { Count = new int[] {} }, // Level 9
                    //new SpellsLevelEntry { Count = new int[] {} }, // Level 10
                    new SpellsLevelEntry { Count = new int[] {0,1} }, // Level 11
                    new SpellsLevelEntry { Count = new int[] {0,1} }, // Level 12
                    new SpellsLevelEntry { Count = new int[] {0,1,1} }, // Level 13
                    new SpellsLevelEntry { Count = new int[] {0,1,1} }, // Level 14
                    new SpellsLevelEntry { Count = new int[] {0,1,1,1} }, // Level 15
                    new SpellsLevelEntry { Count = new int[] {0,1,1,1} }, // Level 16
                    new SpellsLevelEntry { Count = new int[] {0,1,1,1,1} }, // Level 17
                    new SpellsLevelEntry { Count = new int[] {0,1,1,1,1} }, // Level 18
                    new SpellsLevelEntry { Count = new int[] {0,1,1,1,1,1} }, // Level 19
                    new SpellsLevelEntry { Count = new int[] {0,1,1,1,1,1} }, // Level 20
                    new SpellsLevelEntry { Count = new int[] {0,1,1,1,1,1,1} }, // Level 21
                    new SpellsLevelEntry { Count = new int[] {0,1,1,1,1,1,1} }, // Level 22
                    new SpellsLevelEntry { Count = new int[] {0,1,1,1,1,1,1,1} }, // Level 23
                    new SpellsLevelEntry { Count = new int[] {0,1,1,1,1,1,1,1} }, // Level 24
                    new SpellsLevelEntry { Count = new int[] {0,1,1,1,1,1,1,1,1} }, // Level 25
                    new SpellsLevelEntry { Count = new int[] {0,1,1,1,1,1,1,1,1} }, // Level 26
                    new SpellsLevelEntry { Count = new int[] {0,1,1,1,1,1,1,1,1,1} }, // Level 27
                    new SpellsLevelEntry { Count = new int[] {0,1,1,1,1,1,1,1,1,1} }, // Level 28 - Wyrm
                    new SpellsLevelEntry { Count = new int[] {0,1,1,1,1,1,1,1,1,1} }, // Level 29
                    new SpellsLevelEntry { Count = new int[] {0,1,1,1,1,1,1,1,1,1} }, // Level 30 - Great Wyrm
                    new SpellsLevelEntry { Count = new int[] {0,1,1,1,1,1,1,1,1,1} }, // Level 31
                    new SpellsLevelEntry { Count = new int[] {0,1,1,1,1,1,1,1,1,1} }, // Level 32
                    new SpellsLevelEntry { Count = new int[] {0,1,1,1,1,1,1,1,1,1} }, // Level 33
                    new SpellsLevelEntry { Count = new int[] {0,1,1,1,1,1,1,1,1,1} }, // Level 34
                    new SpellsLevelEntry { Count = new int[] {0,1,1,1,1,1,1,1,1,1} }, // Level 35
                    new SpellsLevelEntry { Count = new int[] {0,1,1,1,1,1,1,1,1,1} }, // Level 36
                    new SpellsLevelEntry { Count = new int[] {0,1,1,1,1,1,1,1,1,1} }, // Level 37
                    new SpellsLevelEntry { Count = new int[] {0,1,1,1,1,1,1,1,1,1} }, // Level 38
                    new SpellsLevelEntry { Count = new int[] {0,1,1,1,1,1,1,1,1,1} }, // Level 39
                    new SpellsLevelEntry { Count = new int[] {0,1,1,1,1,1,1,1,1,1} }, // Level 40
                };
            });
            var spellsPerDay = Helpers.CreateBlueprint<BlueprintSpellsTable>(Main.DragonModContext, $"DragonBloodline{bloodlineName}SpellsPerDay", bp =>
            {
                bp.Levels = new SpellsLevelEntry[]
                {
                    new SpellsLevelEntry { Count = new int[] {} }, // Level 0
                    //new SpellsLevelEntry { Count = new int[] {0,3} }, // Level 1
                    //new SpellsLevelEntry { Count = new int[] {0,3} }, // Level 2
                    //new SpellsLevelEntry { Count = new int[] {0,3} }, // Level 3
                    //new SpellsLevelEntry { Count = new int[] {0,3} }, // Level 4
                    //new SpellsLevelEntry { Count = new int[] {0,3} }, // Level 5
                    //new SpellsLevelEntry { Count = new int[] {0,3} }, // Level 6
                    //new SpellsLevelEntry { Count = new int[] {0,3} }, // Level 7
                    //new SpellsLevelEntry { Count = new int[] {0,3} }, // Level 8
                    //new SpellsLevelEntry { Count = new int[] {0,3} }, // Level 9
                    //new SpellsLevelEntry { Count = new int[] {0,3} }, // Level 10
                    new SpellsLevelEntry { Count = new int[] {0,3} }, // Level 11
                    new SpellsLevelEntry { Count = new int[] {0,4} }, // Level 12 (level 1 adjusted to 4 with stats)
                    new SpellsLevelEntry { Count = new int[] {0,4,3} }, // Level 13
                    new SpellsLevelEntry { Count = new int[] {0,5,3} }, // Level 14
                    new SpellsLevelEntry { Count = new int[] {0,5,5,3} }, // Level 15
                    new SpellsLevelEntry { Count = new int[] {0,6,5,3} }, // Level 16
                    new SpellsLevelEntry { Count = new int[] {0,6,5,4,3} }, // Level 17
                    new SpellsLevelEntry { Count = new int[] {0,6,6,5,3} }, // Level 18
                    new SpellsLevelEntry { Count = new int[] {0,6,6,5,3,3} }, // Level 19
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,4,3} }, // Level 20
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,5,3,3} }, // Level 21
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,4,3} }, // Level 22
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,5,3,3} }, // Level 23
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,6,4,3} }, // Level 24
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,6,5,3,3} }, // Level 25
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,6,6,4,3} }, // Level 26
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,6,6,4,3,3} }, // Level 27
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,6,6,6,4,3} }, // Level 28
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,6,6,6,5,3} }, // Level 29
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,6,6,6,6,4} }, // Level 30
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,6,6,6,6,4} }, // Level 31
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,6,6,6,6,4} }, // Level 32
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,6,6,6,6,4} }, // Level 33
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,6,6,6,6,4} }, // Level 34
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,6,6,6,6,4} }, // Level 35
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,6,6,6,6,4} }, // Level 36
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,6,6,6,6,4} }, // Level 37
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,6,6,6,6,4} }, // Level 38
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,6,6,6,6,4} }, // Level 39
                    new SpellsLevelEntry { Count = new int[] {0,6,6,6,6,6,6,6,6,4} }, // Level 40
                };
            });
            var spellsList = Helpers.CreateBlueprint<BlueprintSpellList>(Main.DragonModContext, $"DragonBloodline{bloodlineName}SpellsList", bp =>
            {
                bp.SpellsByLevel = new SpellLevelList[10]
                {
                    new SpellLevelList(0) { m_Spells = new List<BlueprintAbilityReference>() },
                    new SpellLevelList(1) { m_Spells = new List<BlueprintAbilityReference>() },
                    new SpellLevelList(2) { m_Spells = new List<BlueprintAbilityReference>() },
                    new SpellLevelList(3) { m_Spells = new List<BlueprintAbilityReference>() },
                    new SpellLevelList(4) { m_Spells = new List<BlueprintAbilityReference>() },
                    new SpellLevelList(5) { m_Spells = new List<BlueprintAbilityReference>() },
                    new SpellLevelList(6) { m_Spells = new List<BlueprintAbilityReference>() },
                    new SpellLevelList(7) { m_Spells = new List<BlueprintAbilityReference>() },
                    new SpellLevelList(8) { m_Spells = new List<BlueprintAbilityReference>() },
                    new SpellLevelList(9) { m_Spells = new List<BlueprintAbilityReference>() },
                };
            });

            var spellbook = Helpers.CreateBlueprint<BlueprintSpellbook>(Main.DragonModContext, $"DragonBloodline{bloodlineName}Spellbook", bp =>
            {
                bp.Name = Helpers.CreateString(Main.DragonModContext, $"DragonBloodline{bloodlineName}Spellbook.Name", $"Dragon {bloodlineName} Spellbook");
                bp.m_CharacterClass = DragonClass.GetReference();
                bp.m_SpellsPerDay = spellsPerDay.ToReference<BlueprintSpellsTableReference>();
                bp.m_SpellsKnown = spellsKnown.ToReference<BlueprintSpellsTableReference>();
                //bp.m_SpellList = spellsList.ToReference<BlueprintSpellListReference>();
                // Use sorcerer (aka wizard) spell list for now
                bp.m_SpellList = BlueprintTools.GetBlueprintReference<BlueprintSpellListReference>("ba0401fdeb4062f40a7aa95b6f07fe89");
                bp.CastingAttribute = StatType.Charisma;
                bp.CantripsType = CantripsType.Cantrips;
                bp.IsArcane = true;
                bp.Spontaneous = true;
                bp.CasterLevelModifier = (startingLevel - 1) * -1;

                // These relate to special spell slots (like wizard's favourite school spell slots or shaman's spirit magic slots)
                bp.HasSpecialSpellList = false;
                bp.SpecialSpellListName = StaticReferences.Strings.Null;
            });
            PatchTools.RegisterSpellbook(spellbook);
            return spellbook;
        }
    }
}
