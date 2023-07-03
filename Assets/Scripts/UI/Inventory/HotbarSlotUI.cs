using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Inventories;
using UnityEngine;

namespace RPG.UI.Inventories
{
  public class HotbarSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
  {
    [SerializeField] int index;
    [SerializeField] ItemIcon icon = null;

    HotbarItem item = null;
    Hotbar hotbar;

    private void Awake()
    {
      hotbar = GameObject.FindGameObjectWithTag("Player").GetComponent<Hotbar>();
      hotbar.hotbarUpdated += RedrawHotbar;
    }

    private void Start()
    {
      RedrawHotbar();
    }

    private void RedrawHotbar()
    {
      icon.SetItem(GetItem(), GetNumber());
    }

    public void AddItems(InventoryItem item, int number)
    {
      hotbar.AddItem((HotbarItem)item, index, number);
    }

    public InventoryItem GetItem()
    {
      return hotbar.GetItem(index);
    }

    public int GetNumber()
    {
      return hotbar.GetNumber(index);
    }

    public int MaxAcceptable(InventoryItem item)
    {
      return hotbar.MaxAcceptable(item, index);
      // HotbarItem hotItem = item as HotbarItem;
      // if (hotItem == null || this.item != null)
      // {
      //   return 0;
      // }
      // if (hotItem. != index)
      // {
      //   return 0;
      // }
      // if(GetItem() && item.GetItemID().Equals(GetItem().GetItemID())) return 0;
      // return 1;
    }

    public void RemoveItems(int number)
    {
      hotbar.RemoveItem(index, number);
    }

    InventoryItem IItemHolder.GetItem()
    {
      return item as InventoryItem;
    }
  }
}
