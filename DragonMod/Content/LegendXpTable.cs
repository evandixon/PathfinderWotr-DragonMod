using Kingmaker.Blueprints.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;

namespace DragonMod.Content
{
    public static class LegendXpTable
    {
        public static void Patch()
        {
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
            xpTable.Bonuses = new int[41]
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
    }
}
