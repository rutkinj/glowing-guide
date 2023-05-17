using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Inventories;

namespace RPG.UI.Inventories
{
  public class InvUIManager : MonoBehaviour
  {

    [SerializeField] InvSlotUI invItemPrefab = null;

    Inventory playerInv;

    void Awake()
    {
      playerInv = Inventory.GetPlayerInventory();
      playerInv.inventoryUpdated += Redraw;
    }

    void Start()
    {
      Redraw();
    }

    void Redraw()
    {
      foreach (Transform child in transform)
      {
        Destroy(child.gameObject);
      }

      for (int i = 0; i < playerInv.GetSize(); i++)
      {
        InvSlotUI slot = Instantiate(invItemPrefab, transform);
        slot.Setup(playerInv, i);
      }
    }
  }
}


