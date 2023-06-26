using System.Collections;
using System.Collections.Generic;
using RPG.Inventories;
using UnityEngine;

namespace RPG.UI.Inventories
{
  public class EquipSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
  {
    [SerializeField] EquipLocation equipLocation;
    [SerializeField] ItemIcon icon = null;

    EquipableItem item = null;
    Equipment equipment;

    public void AddItems(InventoryItem item, int number)
    {
      //if the item matches the slot TODO
      this.item = item as EquipableItem;
      icon.SetItem(item, 1);
    }

    public InventoryItem GetItem()
    {
      return item;
    }

    public int GetNumber()
    {
      if (item == null)
      {
        return 0;
      }
      return 1;
    }

    public int MaxAcceptable(InventoryItem item)
    {
      if (!(item is EquipableItem) || this.item != null)
      {
        return 0;
      }
      // EquipableItem equipableItem = item as EquipableItem;
      // if (equipableItem.GetEquipLocation() != equipLocation)
      // {
      //   return 0;
      // }
      return 1;
    }

    public void RemoveItems(int number)
    {
      this.item = null;
      icon.SetItem(null);
    }

    InventoryItem IItemHolder.GetItem()
    {
      throw new System.NotImplementedException();
    }
  }
}
