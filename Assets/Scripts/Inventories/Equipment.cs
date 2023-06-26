using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Saving;
using UnityEngine;

namespace RPG.Inventories
{
  public class Equipment : MonoBehaviour, ISaveable
  {

    Dictionary<EquipLocation, EquipableItem> equippedItems = new Dictionary<EquipLocation, EquipableItem>();

    public event Action equipmentUpdated;

    public EquipableItem GetItemInSlot(EquipLocation equipLocation)
    {
      if (equippedItems.ContainsKey(equipLocation))
      {
        return equippedItems[equipLocation];
      }
      return null;
    }

    public void AddItem(EquipLocation equipLocation, EquipableItem item)
    {
      if (item.GetEquipLocation() != equipLocation) return;


      equippedItems[equipLocation] = item;

      if (equipmentUpdated != null)
      {
        equipmentUpdated();
      }
    }

    public void RemoveItem(EquipLocation equipLocation)
    {
      equippedItems.Remove(equipLocation);

      if (equipmentUpdated != null)
      {
        equipmentUpdated();
      }
    }

    public object CaptureState()
    {
      throw new System.NotImplementedException();
    }

    public void RestoreState(object state)
    {
      throw new System.NotImplementedException();
    }
  }
}
