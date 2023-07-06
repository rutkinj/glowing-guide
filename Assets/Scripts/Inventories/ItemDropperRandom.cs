using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Inventories
{
  public class ItemDropperRandom : ItemDropper
  {
    [SerializeField] float scatterDist = 1f;

    [SerializeField] InventoryItem[] dropList;
    [SerializeField] int maxDrops = 1;

    const int ATTEMPTS = 15;

    protected override Vector3 GetDropLocation()
    {
      for (int i = 0; i < ATTEMPTS; i++)
      {

        Vector3 randomPoint = transform.position + Random.insideUnitSphere * scatterDist;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 0.1f, NavMesh.AllAreas))
        {
          return hit.position + Vector3.up / 2;
        }
      }
      return transform.position;
    }

    public void DropLoot()
    {
      float numOfDrops = Random.Range(0, maxDrops);
      for (int i = 0; i < numOfDrops; i++)
      {
        DropItem(dropList[Random.Range(0, dropList.Length)], 1);
      }
    }
  }
}

