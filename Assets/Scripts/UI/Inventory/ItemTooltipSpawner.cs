using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Inventories
{
  public class ItemTooltipSpawner : TooltipSpawner
  {

    public override bool CanCreateTooltip()
    {
      var item = GetComponent<InvSlotUI>().GetItem();
      if(!item) return false;

      return true;
    }

    public override void UpdateTooltip(GameObject tooltip)
    {
      var itemTooltip = tooltip.GetComponent<ItemTooltip>();
      if(!itemTooltip) return;

      var item = GetComponent<InvSlotUI>().GetItem();
      itemTooltip.Setup(item);
    }
  }
}
