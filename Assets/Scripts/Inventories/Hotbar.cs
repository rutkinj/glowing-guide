using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RPG.Saving;
using UnityEngine;

namespace RPG.Inventories
{
  public class Hotbar : MonoBehaviour, IJsonSaveable
  {
    Dictionary<int, HotbarSlot> hotbarItems = new Dictionary<int, HotbarSlot>();

    private class HotbarSlot
    {
      public HotbarItem item;
      public int itemCount;
    }

    public HotbarItem GetItem(int index)
    {
      if (hotbarItems.ContainsKey(index))
      {
        return hotbarItems[index].item;
      }
      return null;
    }

    public int GetNumber(int index)
    {
      if (hotbarItems.ContainsKey(index))
      {
        return hotbarItems[index].itemCount;
      }
      return 0;
    }

    public void AddItem(InventoryItem item, int index, int itemCount)
    {
      if ((HotbarItem)item == null) return;

      if (hotbarItems.ContainsKey(index))
      {
        if (object.ReferenceEquals(item, hotbarItems[index].item))
        {
          hotbarItems[index].itemCount += itemCount;
        }
      }
      else
      {
        var slot = new HotbarSlot();
        slot.item = (HotbarItem)item;
        slot.itemCount = itemCount;
        hotbarItems[index] = slot;
      }

      if (hotbarUpdated != null)
      {
        hotbarUpdated();
      }

    }

    public void RemoveItem(int index, int count)
    {
      if (hotbarItems.ContainsKey(index))
      {
        hotbarItems[index].itemCount -= count;
        if (hotbarItems[index].itemCount <= 0)
        {
          hotbarItems.Remove(index);
        }
      }

      if (hotbarUpdated != null)
      {
        hotbarUpdated();
      }
    }

    public bool UseItem(int index, GameObject player)
    {
      if (hotbarItems.ContainsKey(index))
      {
        hotbarItems[index].item.Use(player);
        if (hotbarItems[index].item.isConsumable())
        {
          RemoveItem(index, 1);
        }
        return true;
      }
      return false;
    }

    public int MaxAcceptable(InventoryItem item, int index)
    {
      var hotItem = item as HotbarItem;
      if (hotItem == null) return 0;

      if (hotbarItems.ContainsKey(index) && !object.ReferenceEquals(item, hotbarItems[index].item))
      {
        return 0;
      }

      if (hotItem.isConsumable())
      {
        return int.MaxValue;
      }

      if (hotbarItems.ContainsKey(index))
      {
        return 0;
      }
      return 1;
    }

    public event Action hotbarUpdated;

    public JToken CaptureAsJToken()
    {
      JObject state = new JObject();
      IDictionary<string, JToken> stateDict = state;

      foreach (var kv in hotbarItems)
      {
        JObject hotbarState = new JObject();
        IDictionary<string, JToken> hotbarStateDict = hotbarState;
        hotbarStateDict["item"] = JToken.FromObject(kv.Value.item.GetItemID());
        hotbarStateDict["count"] = JToken.FromObject(kv.Value.itemCount);
        stateDict[kv.Key.ToString()] = hotbarState;
      }
      return state;
    }

    public void RestoreFromJToken(JToken state)
    {
      if (state is JObject stateObject)
      {
        hotbarItems.Clear();

        IDictionary<string, JToken> stateDict = stateObject;
        foreach (var kv in stateDict)
        {
          if (kv.Value is JObject hotbarState)
          {
            int key = Int32.Parse(kv.Key);
            IDictionary<string, JToken> hotbarStateDict = hotbarState;
            var item = InventoryItem.GetFromID(hotbarStateDict["item"].ToObject<string>());
            int count = hotbarStateDict["count"].ToObject<int>();

            AddItem(item, key, count);
          }
        }
      }
    }

  }
}
