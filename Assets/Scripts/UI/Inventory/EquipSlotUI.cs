using System.Collections;
using System.Collections.Generic;
using RPG.Inventories;
using UnityEngine;

namespace RPG.UI.Inventories
{
  public class EquipSlotUI : MonoBehaviour, IItemHolder, IDragContainer<EquipableItem>
  {
    [SerializeField] EquipLocation equipLocation;
    [SerializeField] ItemIcon icon;
    
    public void AddItems(EquipableItem item, int number)
    {
      throw new System.NotImplementedException();
    }

    public EquipableItem GetItem()
    {
      throw new System.NotImplementedException();
    }

    public int GetNumber()
    {
      throw new System.NotImplementedException();
    }

    public int MaxAcceptable(EquipableItem item)
    {
      throw new System.NotImplementedException();
    }

    public void RemoveItems(int number)
    {
      throw new System.NotImplementedException();
    }

    InventoryItem IItemHolder.GetItem()
    {
      throw new System.NotImplementedException();
    }
  }
}
