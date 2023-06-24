using System.Collections;
using System.Collections.Generic;
using RPG.UI.Inventories;
using UnityEngine;

namespace RPG.Inventories
{
  public class DropTarget : MonoBehaviour, IDragDestination<InventoryItem>
  {
    public void AddItems(InventoryItem item, int amount){
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<ItemDropper>().DropItem(item, amount);
    }

    public int MaxAcceptable(InventoryItem item){
        return int.MaxValue;
    }
  }
}
