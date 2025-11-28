using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using System;
using System.Collections.Generic;

namespace DragonMod.Content.Dragon.Bloodlines
{
    public class DragonAge
    {
        public string Name { get; set; }
        public Size Size { get; set; }
        public int HitDice { get; set; }
        public int DamageResistance { get; set; }
        public int SpellResistance { get; set; }
        public bool CanChangeShape { get; set; }
        public List<Spell> BonusSpells { get; set; }
        public List<Func<BlueprintFeature>> BonusFeatures { get; set; }
    }
}
