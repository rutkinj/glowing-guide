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

      if(hotbarItems.ContainsKey(index)){
        return 0;
      }
      return 1;
    }

    public event Action hotbarUpdated;

    public JToken CaptureAsJToken()
    {
      throw new System.NotImplementedException();
    }

    public void RestoreFromJToken(JToken state)
    {
      throw new System.NotImplementedException();
    }

  }
}
