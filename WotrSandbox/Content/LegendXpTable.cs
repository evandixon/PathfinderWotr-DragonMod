using Kingmaker.Blueprints.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;

namespace WotrSandbox.Content
{
    public static class LegendXpTable
    {
        public static void Patch()
        {
            var xpTable = BlueprintTools.GetBlueprint<BlueprintStatProgression>("11c77f6853ac46aa8e2d004d6dca5f9f");

            /* For comparison, this is the base XP table:
              0,
              0,
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
              3600000
            */
            xpTable.Bonuses = new int[41]
            {
                0,
                0,
                1000,
                3000,
                2000,
                3500,
                5000,
                9000,
                9000,
                12000,
                15000,
                19000,
                23000,
                29000,
                35000,
                43000,
                51000,
                63000,
                75000,
                90000,
                105000,
                130000,
                155000,
                167000,
                220000,
                257500,
                315000,
                380000,
                445000,
                540000,
                635000,
                762500,
                890000,
                1095000,
                1300000,
                1550000,
                1800000,
                2175000,
                2550000,
                3075000,
                3600000
            };
        }
    }
}
