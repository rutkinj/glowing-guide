using System.Collections;
using System.Collections.Generic;
using RPG.Inventories;
using UnityEngine;

namespace RPG.UI.Inventories
{
  public class InvSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
  {
    [SerializeField] ItemIcon icon = null;

    int index;
    InventoryItem item;
    Inventory inv;

    public void Setup(Inventory inv, int index)
    {
      this.inv = inv;
      this.index = index;
      icon.SetItem(inv.GetItemInSlot(index), inv.GetItemCountInSlot(index));
    }

    public int MaxAcceptable(InventoryItem item)
    {
      if (inv.HasSpaceFor(item))
      {
        return int.MaxValue;
      }
      return 0;
    }

    public void AddItems(InventoryItem item, int itemCount)
    {
      inv.AddItemToSlot(index, item, itemCount);
    }

    public InventoryItem GetItem()
    {
      return inv.GetItemInSlot(index);
    }

    public int GetNumber()
    {
      return inv.GetItemCountInSlot(index);
    }

    public void RemoveItems(int itemCount)
    {
      inv.RemoveFromSlot(index, itemCount);
    }
  }
}
