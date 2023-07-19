using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RPG.Inventories;

namespace RPG.UI.Inventories
{
  [RequireComponent(typeof(Image))]
  public class ItemIcon : MonoBehaviour
  {
    [SerializeField] GameObject countContainer = null;
    [SerializeField] TextMeshProUGUI countText = null;

    public void SetItem(InventoryItem item, int itemCount)
    {
      Image iconImage = GetComponent<Image>();
      if (item == null)
      {
        iconImage.enabled = false;
        countContainer.SetActive(false);
      }
      else
      {
        iconImage.enabled = true;
        iconImage.sprite = item.GetIcon();
        iconImage.color = item.GetIconColor();

        if (itemCount > 1)
        {
          countContainer.SetActive(true);
          countText.SetText(itemCount.ToString());
        }
        else
        {
          countContainer.SetActive(false);
        }
      }
    }
  }
}

