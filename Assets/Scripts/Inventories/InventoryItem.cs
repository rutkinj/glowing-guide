using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
  /// <summary>
  /// A ScriptableObject that represents any item that can be put in an
  /// inventory.
  /// </summary>
  /// <remarks>
  /// In practice, you are likely to use a subclass such as `ActionItem` or
  /// `EquipableItem`.
  /// </remarks>
  public abstract class InventoryItem : ScriptableObject, ISerializationCallbackReceiver
  {
    // CONFIG DATA
    [Tooltip("Auto-generated UUID for saving/loading. Clear this field if you want to generate a new one.")]
    [SerializeField] string itemID = null;
    [Tooltip("Item name to be displayed in UI.")]
    [SerializeField] string displayName = null;
    [Tooltip("Item description to be displayed in UI.")]
    [SerializeField][TextArea] string description = null;
    [Tooltip("The UI icon to represent this item in the inventory.")]
    [SerializeField] Sprite icon = null;
    [SerializeField] Color iconColor = Color.white;
    [Tooltip("If true, multiple items of this type can be stacked in the same inventory slot.")]
    [SerializeField] bool stackable = false;
    [SerializeField] Pickup pickup;

    // STATE
    static Dictionary<string, InventoryItem> itemLookupCache;

    // PUBLIC

    /// <summary>
    /// Get the inventory item instance from its UUID.
    /// </summary>
    /// <param name="itemID">
    /// String UUID that persists between game instances.
    /// </param>
    /// <returns>
    /// Inventory item instance corresponding to the ID.
    /// </returns>
    public static InventoryItem GetFromID(string itemID)
    {
      if (itemLookupCache == null)
      {
        itemLookupCache = new Dictionary<string, InventoryItem>();
        var itemList = Resources.LoadAll<InventoryItem>("");
        foreach (var item in itemList)
        {
          if (itemLookupCache.ContainsKey(item.itemID))
          {
            Debug.LogError(string.Format("Looks like there's a duplicate GameDevTV.UI.InventorySystem ID for objects: {0} and {1}", itemLookupCache[item.itemID], item));
            continue;
          }

          itemLookupCache[item.itemID] = item;
        }
      }

      if (itemID == null || !itemLookupCache.ContainsKey(itemID)) return null;
      return itemLookupCache[itemID];
    }

    public Sprite GetIcon()
    {
      return icon;
    }

    public Color GetIconColor()
    {
      return iconColor;
    }

    public string GetItemID()
    {
      return itemID;
    }

    public bool IsStackable()
    {
      return stackable;
    }

    public string GetDisplayName()
    {
      return displayName;
    }

    public string GetDescription()
    {
      return description;
    }

    public Pickup SpawnPickup(Vector3 position, int itemCount)
    {
      Pickup pickup = Instantiate(this.pickup);
      pickup.transform.position = position;
      pickup.Setup(this, itemCount);
      return pickup;
    }

    // PRIVATE

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
      // Generate and save a new UUID if this is blank.
      if (string.IsNullOrWhiteSpace(itemID))
      {
        itemID = System.Guid.NewGuid().ToString();
      }
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
      // Require by the ISerializationCallbackReceiver but we don't need
      // to do anything with it.
    }
  }
}
