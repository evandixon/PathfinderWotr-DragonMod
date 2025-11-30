using Kingmaker.UnitLogic.Buffs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonMod.Content.Dragon
{
    public class UnequipUnsupportedItemsComponent : UnitBuffComponentDelegate
    {
        public override void OnActivate()
        {
            var body = Owner.Body;

            body.Armor?.RemoveItem();
            body.PrimaryHand?.RemoveItem();
            body.SecondaryHand?.RemoveItem();
            body.Gloves?.RemoveItem();
            body.Feet?.RemoveItem();
            body.Ring1?.RemoveItem();
            body.Ring2?.RemoveItem();
        }
    }
}
