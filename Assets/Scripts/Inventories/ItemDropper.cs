using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RPG.Saving;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Inventories
{
  /// <summary>
  /// To be placed on anything that wishes to drop pickups into the world.
  /// Tracks the drops for saving and restoring.
  /// </summary>
  public class ItemDropper : MonoBehaviour, IJsonSaveable
  {
    // STATE
    public List<Pickup> droppedItems = new List<Pickup>();

    /// <summary>
    /// Create a pickup at the current position.
    /// </summary>
    /// <param name="item">The item type for the pickup.</param>
    public void DropItem(InventoryItem item, int itemCount)
    {
      SpawnPickup(item, GetDropLocation(), itemCount);
    }

    /// <summary>
    /// Override to set a custom method for locating a drop.
    /// </summary>
    /// <returns>The location the drop should be spawned.</returns>
    protected virtual Vector3 GetDropLocation()
    {
      return transform.position + Vector3.back + Vector3.up;
      //offset so player doesnt immediately pickup what they drop
    }

    public void SpawnPickup(InventoryItem item, Vector3 spawnLocation, int itemCount)
    {
      var pickup = item.SpawnPickup(spawnLocation, itemCount);
      droppedItems.Add(pickup);
    }

    // [System.Serializable]
    // private struct DropRecord
    // {
    //   public string itemID;
    //   public Vector3 position;
    // }

    // object ISaveable.CaptureState()
    // {
    //   RemoveDestroyedDrops();
    //   var droppedItemsList = new DropRecord[droppedItems.Count];
    //   for (int i = 0; i < droppedItemsList.Length; i++)
    //   {
    //     droppedItemsList[i].itemID = droppedItems[i].GetItem().GetItemID();
    //     droppedItemsList[i].position = new SerializableVector3(droppedItems[i].transform.position);
    //   }
    //   return droppedItemsList;
    // }

    // void ISaveable.RestoreState(object state)
    // {
    //   var droppedItemsList = (DropRecord[])state;
    //   foreach (var item in droppedItemsList)
    //   {
    //     var pickupItem = InventoryItem.GetFromID(item.itemID);
    //     Vector3 position = item.position.ToVector();
    //     SpawnPickup(pickupItem, position);
    //   }
    // }

    // Save Structure
    class otherSceneDropRecord
    {
      public string id;
      public int count;
      public Vector3 location;
      public int scene;
    }

    private List<otherSceneDropRecord> otherSceneDrops = new List<otherSceneDropRecord>();

    private List<otherSceneDropRecord> MergeDroppedItemsLists()
    {
      List<otherSceneDropRecord> mergedList = new List<otherSceneDropRecord>();
      mergedList.AddRange(otherSceneDrops);
      foreach (var item in droppedItems)
      {
        otherSceneDropRecord drop = new otherSceneDropRecord();
        drop.id = item.item.GetItemID();
        drop.count = item.itemCount;
        drop.location = item.transform.position;
        drop.scene = SceneManager.GetActiveScene().buildIndex;
        mergedList.Add(drop);
      }
      return mergedList;
    }

    public JToken CaptureAsJToken()
    {
      RemoveDestroyedDrops();
      var drops = MergeDroppedItemsLists();
      JArray state = new JArray();
      IList<JToken> stateList = state;

      foreach (var drop in drops)
      {
        JObject dropState = new JObject();
        IDictionary<string, JToken> dropStateDict = dropState;
        dropStateDict["id"] = JToken.FromObject(drop.id);
        dropStateDict["count"] = drop.count;
        dropStateDict["location"] = drop.location.ToToken();
        dropStateDict["scene"] = drop.scene;
        stateList.Add(dropState);
      }
      return state;
    }

    private void ClearExistingDrops()
    {
      foreach (var oldDrop in droppedItems)
      {
        if (oldDrop != null) Destroy(oldDrop.gameObject);
      }

      otherSceneDrops.Clear();
    }

    public void RestoreFromJToken(JToken state)
    {
      if (state is JArray stateArray)
      {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        IList<JToken> stateList = stateArray;
        ClearExistingDrops();
        foreach (var entry in stateList)
        {
          if (entry is JObject dropState)
          {
            IDictionary<string, JToken> dropStateDict = dropState;
            int scene = dropStateDict["scene"].ToObject<int>();
            InventoryItem item = InventoryItem.GetFromID(dropStateDict["id"].ToObject<string>());
            int count = dropStateDict["count"].ToObject<int>();
            Vector3 location = dropStateDict["location"].ToVector3();
            if (scene == currentScene)
            {
              SpawnPickup(item, location, count);
            }
            else
            {
              var otherDrop = new otherSceneDropRecord();
              otherDrop.id = item.GetItemID();
              otherDrop.count = count;
              otherDrop.location = location;
              otherDrop.scene = scene;
              otherSceneDrops.Add(otherDrop);
            }
          }
        }
      }
    }

    /// <summary>
    /// Remove any drops in the world that have subsequently been picked up.
    /// </summary>
    private void RemoveDestroyedDrops()
    {
      var newList = new List<Pickup>();
      foreach (var item in droppedItems)
      {
        if (item != null)
        {
          newList.Add(item);
        }
      }
      droppedItems = newList;
    }

  }
}
