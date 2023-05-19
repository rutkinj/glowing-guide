using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
  public class Pickup : MonoBehaviour
  {
    public InventoryItem item = null;

    public void Setup(InventoryItem item)
    {
      this.item = item;
    }
  }
}


