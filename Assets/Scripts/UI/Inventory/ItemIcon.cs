using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RPG.Inventories;

namespace RPG.UI.Inventory
{
  [RequireComponent(typeof(Image))]
  public class ItemIcon : MonoBehaviour
  {
    public void SetItem(InventoryItem item)
    {
      Image iconImage = GetComponent<Image>();
      if (item == null)
      {
        iconImage.enabled = false;
      }
      else
      {
        iconImage.enabled = true;
        iconImage.sprite = item.GetIcon();
      }
    }

    // public Sprite GetItem()
    // {
    //   Image iconImage = GetComponent<Image>();
    //   if (!iconImage.enabled)
    //   {
    //     return null;
    //   }
    //   else
    //   {
    //     return iconImage.sprite;
    //   }
    // }
  }
}

