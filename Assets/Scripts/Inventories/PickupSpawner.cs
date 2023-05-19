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

    void Awake(){
      Spawn();
    }

    private void Spawn(){
      item.SpawnPickup(this.transform);
    }

    public Pickup GetPickup(){
      return GetComponentInChildren<Pickup>();
    }

    private void DestroyPickup(){
      if(GetPickup()){
        Destroy(GetPickup().gameObject);
      }
    }

    public JToken CaptureAsJToken()
    {
      return JToken.FromObject((bool)GetPickup());
    }

    public void RestoreFromJToken(JToken state)
    {
      bool wasPresent = state.ToObject<bool>();

      if(!wasPresent && GetPickup()){
        DestroyPickup();
      }
      if(wasPresent && !GetPickup()){
        Spawn();
      }
    }
  }
}

