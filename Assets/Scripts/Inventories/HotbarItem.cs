using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
  [CreateAssetMenu(menuName = ("Inventory/HotbarItem"))]
  public class HotbarItem : InventoryItem
  {
    [SerializeField] bool consumable;

    public bool isConsumable()
    {
      return consumable;
    }
  }
}
