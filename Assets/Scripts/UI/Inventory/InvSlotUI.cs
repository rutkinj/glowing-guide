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
      icon.SetItem(inv.GetItemInSlot(index));
    }

    public int MaxAcceptable(InventoryItem item)
    {
      if (inv.HasSpaceFor(item))
      {
        return int.MaxValue;
      }
      return 0;
    }

    public void AddItems(InventoryItem item, int amount)
    {
      inv.AddItemToSlot(index, item);
    }

    public InventoryItem GetItem()
    {
      return inv.GetItemInSlot(index);
    }

    public int GetNumber()
    {
      return 1;
    }

    public void RemoveItems(int amount)
    {
      inv.RemoveFromSlot(index);
    }
  }
}
