using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Inventories;
using UnityEngine;

namespace RPG.UI.Inventories
{
  public class EquipSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
  {
    [SerializeField] EquipLocation equipLocation;
    [SerializeField] ItemIcon icon = null;

    EquipableItem item = null;
    Equipment equipment;

    private void Awake() {
      equipment = GameObject.FindGameObjectWithTag("Player").GetComponent<Equipment>();
      equipment.equipmentUpdated += RedrawEquipment;
    }

    private void Start() {
      RedrawEquipment();
    }

    private void RedrawEquipment()
    {
      icon.SetItem(equipment.GetItemInSlot(equipLocation),1);
    }

    public void AddItems(InventoryItem item, int number)
    {
      //if the item matches the slot TODO
      this.item = item as EquipableItem;
      icon.SetItem(item, number);
    }

    public InventoryItem GetItem()
    {
      return item;
    }

    public int GetNumber()
    {
      if (item == null)
      {
        return 0;
      }
      return 1;
    }

    public int MaxAcceptable(InventoryItem item)
    {
      EquipableItem equipableItem = item as EquipableItem;
      if (equipableItem == null || this.item != null)
      {
        return 0;
      }
      if (equipableItem.GetEquipLocation() != equipLocation)
      {
        return 0;
      }
      return 1;
    }

    public void RemoveItems(int number)
    {
      this.item = null;
      icon.SetItem(null, number);
    }

    InventoryItem IItemHolder.GetItem()
    {
      return item as InventoryItem;
    }
  }
}
