using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
  [CreateAssetMenu(menuName = ("Inventory/EquipableItem"))]
  public class EquipableItem : InventoryItem
  {
    [SerializeField] EquipLocation equipLocation;

    public EquipLocation GetEquipLocation()
    {
      return equipLocation;
    }
  }
}