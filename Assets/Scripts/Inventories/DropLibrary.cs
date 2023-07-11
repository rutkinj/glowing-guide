using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
  [CreateAssetMenu(menuName = "Inventory/Drop Library")]
  public class DropLibrary : ScriptableObject
  {
    [SerializeField] DropConfig[] dropPool;
    [SerializeField] float dropChancePercentage;
    [SerializeField] int minDrops;
    [SerializeField] int maxDrops;

    [System.Serializable]
    class DropConfig
    {
      public InventoryItem item;
      public float relativeChance;
      public int minCount;
      public int maxCount;
      public int GetRandomCount()
      {
        if (!item.IsStackable()) return 1;
        return Random.Range(minCount, maxCount + 1);
      }
    }

    public struct Dropped
    {
      public InventoryItem item;
      public int count;
    }

    public IEnumerable<Dropped> GetRandomDrops()
    {
      if (!ShouldRandomDrop())
      {
        yield break;
      }
      for (int i = 0; i < GetRandomNumberOfDrops(); i++)
      {
        yield return GetRandomDrop();
      }
    }

    bool ShouldRandomDrop()
    {
      if (dropPool.Length < 1 || maxDrops < 1)
      {
        return false;
      }
      return Random.Range(1,101) <= dropChancePercentage;
    }

    int GetRandomNumberOfDrops()
    {
      return Random.Range(minDrops, maxDrops + 1);
    }

    Dropped GetRandomDrop()
    {
        Dropped randomDrop = new Dropped();
        var itemDropConfig = SelectRandomItem();

        randomDrop.item = itemDropConfig.item;
        randomDrop.count = itemDropConfig.GetRandomCount();

        return randomDrop;
    }

    DropConfig SelectRandomItem()
    {
      float totalChance = GetTotalChance();
      float randomRoll = Random.Range(0, totalChance);
      float chanceTotal = 0;
      foreach (var drop in dropPool)
      {
        chanceTotal += drop.relativeChance;
        if (chanceTotal > randomRoll)
        {
          return drop;
        }
      }
      return null;
    }

    float GetTotalChance()
    {
      float totalChance = 0;
      foreach (var drop in dropPool)
      {
        totalChance += drop.relativeChance;
      }
      return totalChance;
    }
  }
}
