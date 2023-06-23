using System.Collections;
using System.Collections.Generic;
using RPG.Inventories;
using TMPro;
using UnityEngine;

namespace RPG.UI
{

  public class ItemTooltip : MonoBehaviour
  {
    [SerializeField] TextMeshProUGUI title = null;
    [SerializeField] TextMeshProUGUI body = null;

    public void Setup(InventoryItem item){
        title.text = item.GetDisplayName();
        body.text = item.GetDescription();
    }
  }
}
