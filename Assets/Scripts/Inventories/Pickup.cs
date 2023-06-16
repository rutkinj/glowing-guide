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

    private void OnTriggerEnter(Collider other)
    {
      if (other.CompareTag("Player"))
      {
        if (other.gameObject.GetComponent<Inventory>().AddToFirstEmptySlot(item))
        {
          Destroy(gameObject);
        }
      }
    }
  }
}


