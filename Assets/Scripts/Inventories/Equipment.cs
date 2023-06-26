using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RPG.Saving;
using UnityEngine;

namespace RPG.Inventories
{
  public class Equipment : MonoBehaviour, IJsonSaveable
  {

    public Dictionary<EquipLocation, EquipableItem> equippedItems = new Dictionary<EquipLocation, EquipableItem>();

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

    public JToken CaptureAsJToken()
    {
      JObject state = new JObject();
      IDictionary<string, JToken> stateDict = state;
      foreach (var kv in equippedItems)
      {
        stateDict[kv.Key.ToString()] = JToken.FromObject(kv.Value.GetItemID());
        print(kv.Key);
        print(kv.Value);
      }
      return state;
    }

    public void RestoreFromJToken(JToken state)
    {
      if (state is JObject stateObject)
      {
        IDictionary<string, JToken> stateDict = stateObject;

        equippedItems.Clear();
        foreach (var kv in stateObject)
        {
          if (Enum.TryParse(kv.Key, true, out EquipLocation equipLoc))
          {
            if (InventoryItem.GetFromID(kv.Value.ToObject<string>()) is EquipableItem item)
            {
              equippedItems[equipLoc] = item;
            }
          }
        }
      }
      equipmentUpdated?.Invoke();
    }
  }
}
