using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
  public class PersistantObjSpawner : MonoBehaviour
  {
    [SerializeField] GameObject persistantObjectPrefab;

    static bool hasSpawned = false;
    private void Awake()
    {
      if (hasSpawned) return;

      hasSpawned = true;
      SpawnPersistanceObjects();
    }

    private void SpawnPersistanceObjects()
    {
      GameObject persistantObject = Instantiate(persistantObjectPrefab);
      DontDestroyOnLoad(persistantObject);
    }
  }
}
