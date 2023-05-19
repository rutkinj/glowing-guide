using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RPG.Saving;
using UnityEngine;

namespace RPG.Inventories
{
  public class PickupSpawner : MonoBehaviour, IJsonSaveable
  {
    [SerializeField] InventoryItem item = null;
    private Pickup pickup = null;
    private bool isPickedUp = false;

    void Awake(){
      Spawn();
    }

    private void Spawn(){
      pickup = item.SpawnPickup(this.transform);
    }

    public JToken CaptureAsJToken()
    {
      throw new System.NotImplementedException();
    }

    public void RestoreFromJToken(JToken state)
    {
      throw new System.NotImplementedException();
    }
  }
}

