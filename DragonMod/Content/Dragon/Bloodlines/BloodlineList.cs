using System.Collections.Generic;

namespace DragonMod.Content.Dragon.Bloodlines
{
    public static class BloodlineList
    {
        public static List<BaseDragonBloodline> Bloodlines { get; } = new List<BaseDragonBloodline>
        {
            DragonBloodlineGold.Instance,
            DragonBloodlineSilver.Instance
        };
    }
}
