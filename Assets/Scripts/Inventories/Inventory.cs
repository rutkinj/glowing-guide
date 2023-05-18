﻿using System;
using UnityEngine;
using GameDevTV.Saving;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace RPG.Inventories
{
  /// <summary>
  /// Provides storage for the player inventory. A configurable number of
  /// slots are available.
  ///
  /// This component should be placed on the GameObject tagged "Player".
  /// </summary>
  public class Inventory : MonoBehaviour, ISaveable
  {
    // CONFIG DATA
    [Tooltip("Allowed size")]
    [SerializeField] int inventorySize = 16;

    // STATE
    InventoryItem[] slots;

    // PUBLIC

    /// <summary>
    /// Broadcasts when the items in the slots are added/removed.
    /// </summary>
    public event Action inventoryUpdated;

    /// <summary>
    /// Convenience for getting the player's inventory.
    /// </summary>
    public static Inventory GetPlayerInventory()
    {
      var player = GameObject.FindWithTag("Player");
      return player.GetComponent<Inventory>();
    }

    /// <summary>
    /// Could this item fit anywhere in the inventory?
    /// </summary>
    public bool HasSpaceFor(InventoryItem item)
    {
      return FindSlot(item) >= 0;
    }

    /// <summary>
    /// How many slots are in the inventory?
    /// </summary>
    public int GetSize()
    {
      return slots.Length;
    }

    /// <summary>
    /// Attempt to add the items to the first available slot.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <returns>Whether or not the item could be added.</returns>
    public bool AddToFirstEmptySlot(InventoryItem item)
    {
      int i = FindSlot(item);

      if (i < 0)
      {
        return false;
      }

      slots[i] = item;
      if (inventoryUpdated != null)
      {
        inventoryUpdated();
      }
      return true;
    }

    /// <summary>
    /// Is there an instance of the item in the inventory?
    /// </summary>
    public bool HasItem(InventoryItem item)
    {
      for (int i = 0; i < slots.Length; i++)
      {
        if (object.ReferenceEquals(slots[i], item))
        {
          return true;
        }
      }
      return false;
    }

    /// <summary>
    /// Return the item type in the given slot.
    /// </summary>
    public InventoryItem GetItemInSlot(int slot)
    {
      return slots[slot];
    }

    /// <summary>
    /// Remove the item from the given slot.
    /// </summary>
    public void RemoveFromSlot(int slot)
    {
      slots[slot] = null;
      if (inventoryUpdated != null)
      {
        inventoryUpdated();
      }
    }

    /// <summary>
    /// Will add an item to the given slot if possible. If there is already
    /// a stack of this type, it will add to the existing stack. Otherwise,
    /// it will be added to the first empty slot.
    /// </summary>
    /// <param name="slot">The slot to attempt to add to.</param>
    /// <param name="item">The item type to add.</param>
    /// <returns>True if the item was added anywhere in the inventory.</returns>
    public bool AddItemToSlot(int slot, InventoryItem item)
    {
      if (slots[slot] != null)
      {
        return AddToFirstEmptySlot(item); ;
      }

      slots[slot] = item;
      if (inventoryUpdated != null)
      {
        inventoryUpdated();
      }
      return true;
    }

    // PRIVATE

    private void Awake()
    {
      slots = new InventoryItem[inventorySize];
      slots[0] = InventoryItem.GetFromID("5beb0a33-1ab8-4276-aa3f-7aa7db7366f7");
      slots[1] = InventoryItem.GetFromID("bc1c5aa3-b194-4a9c-9534-d95f9e6d9e3a");
    }

    /// <summary>
    /// Find a slot that can accomodate the given item.
    /// </summary>
    /// <returns>-1 if no slot is found.</returns>
    private int FindSlot(InventoryItem item)
    {
      return FindEmptySlot();
    }

    /// <summary>
    /// Find an empty slot.
    /// </summary>
    /// <returns>-1 if all slots are full.</returns>
    private int FindEmptySlot()
    {
      for (int i = 0; i < slots.Length; i++)
      {
        if (slots[i] == null)
        {
          return i;
        }
      }
      return -1;
    }

    object ISaveable.CaptureState()
    {
      var slotStrings = new string[inventorySize];
      for (int i = 0; i < inventorySize; i++)
      {
        if (slots[i] != null)
        {
          slotStrings[i] = slots[i].GetItemID();
        }
      }
      return slotStrings;
    }

    void ISaveable.RestoreState(object state)
    {
      var slotStrings = (string[])state;
      for (int i = 0; i < inventorySize; i++)
      {
        slots[i] = InventoryItem.GetFromID(slotStrings[i]);
      }
      if (inventoryUpdated != null)
      {
        inventoryUpdated();
      }
    }

    // public JToken CaptureAsJToken()
    // {
    //   JObject state = new JObject();
    //   IDictionary<string, JToken> stateDict = state;
    //   for (int i = 0; i < inventorySize; i++)
    //   {
    //     if (slots[i] != null)
    //     {
    //       JObject itemState = new JObject();
    //       IDictionary<string, JToken> itemStateDict = itemState;
    //       itemState["item"] = JToken.FromObject(slots[i].GetItemID());
    //     //   itemState["number"] = JToken.FromObject(slots[i].number);
    //       stateDict[i.ToString()] = itemState;
    //     }
    //   }
    //   return state;
    // }


    // public void RestoreFromJToken(JToken state)
    // {
    //   if (state is JObject stateObject)
    //   {
    //     slots = new InventoryItem[inventorySize];
    //     IDictionary<string, JToken> stateDict = stateObject;
    //     for (int i = 0; i < inventorySize; i++)
    //     {
    //       if (stateDict.ContainsKey(i.ToString()) && stateDict[i.ToString()] is JObject itemState)
    //       {
    //         IDictionary<string, JToken> itemStateDict = itemState;
    //         slots[i] = InventoryItem.GetFromID(itemStateDict["item"].ToObject<string>());
    //         // slots[i].number = itemStateDict["number"].ToObject<int>();
    //       }
    //     }
    //     inventoryUpdated?.Invoke();
    //   }
    // }
  }
}