using System.Collections;
using System.Collections.Generic;
using RPG.Inventories;
using UnityEngine;

namespace RPG.UI.Inventories
{
  public class InvSlotUI : MonoBehaviour, IDragContainer<InventoryItem>
  {
    [SerializeField] ItemIcon icon = null;

    int index;
    InventoryItem item;
    Inventory inv;
    public int MaxAcceptable(Sprite item)
    {
      if (GetItem() == null)
      {
        return int.MaxValue;
      }
      return 0;
    }

    public void AddItems(InventoryItem item, int amount)
    {
      inve
    }

    public Sprite GetItem()
    {
      return icon.GetItem();
    }

    public int GetNumber()
    {
      return 1;
    }

    public void RemoveItems(int amount){
        icon.SetItem(null);
    }
  }
}
