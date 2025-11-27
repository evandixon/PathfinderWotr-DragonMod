using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums;
using System;
using System.Collections.Generic;

namespace WotrSandbox.Content.Dragon.Bloodlines
{
    public class DragonAge
    {
        public string Name { get; set; }
        public Size Size { get; set; }
        public int HitDice { get; set; }
        public int StrengthBonus { get; set; }
        public int DexterityBonus { get; set; }
        public int ConstitutionBonus { get; set; }
        public int NaturalArmorBonus { get; set; }
        public int DamageResistance { get; set; }
        public int SpellResistance { get; set; }
        public List<Spell> BonusSpells { get; set; }
        public List<Func<BlueprintFeature>> BonusFeatures { get; set; }
    }
}
