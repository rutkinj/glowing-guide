using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;

namespace RPG.Inventories
{
  public class Pickup : MonoBehaviour, IRaycastable
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
    
    /// IRaycastable
    public CursorType GetCursorType()
    {
      return CursorType.pickup;
    }

    public bool HandleRaycast(PlayerController callingController)
    {
      if (Input.GetMouseButtonDown(0))
      {
        return false;
      }
      return true;
    }
  }
}


