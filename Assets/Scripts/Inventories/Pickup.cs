using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
  public class Pickup : MonoBehaviour
  {
    public InventoryItem item = null;
    public int itemCount = 0;

    public void Setup(InventoryItem item, int itemCount)
    {
      this.item = item;
      this.itemCount = itemCount;
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.CompareTag("Player"))
      {
        if (other.gameObject.GetComponent<Inventory>().AddToFirstEmptySlot(item, itemCount))
        {
          Destroy(gameObject);
        }
      }
    }
  }
}


