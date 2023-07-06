using System.Collections;
using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;

namespace RPG.Inventories
{
  [CreateAssetMenu(menuName = ("Inventory/EquipableItem"))]
  public class EquipableItem : InventoryItem, IModifierProvider
  {
    [SerializeField] EquipLocation equipLocation;

    [SerializeField] Modifier[] additiveMods;
    [SerializeField] Modifier[] percentileMods;

    [System.Serializable]
    struct Modifier
    {
      public Stat stat;
      public float value;
    }

    public EquipLocation GetEquipLocation()
    {
      return equipLocation;
    }

    public IEnumerable<float> GetAdditiveModifiers(Stat stat)
    {
      foreach (Modifier mod in additiveMods)
      {
        if (mod.stat == stat)
        {
          yield return mod.value;
        }
      }
    }

    public IEnumerable<float> GetPercentileModifiers(Stat stat)
    {
      foreach (Modifier mod in percentileMods)
      {
        if (mod.stat == stat)
        {
          yield return mod.value;
        }
      }
    }
  }
}