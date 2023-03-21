using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{
  public class SavingWrapper : MonoBehaviour
  {
    const string defaultSaveFile = "defaultSave";

    JsonSavingSystem save;

    private IEnumerator Start()
    {
      save = GetComponent<JsonSavingSystem>();
      yield return save.LoadLastScene(defaultSaveFile);
    }
    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.S))
      {
        Save();
      }
      if (Input.GetKeyDown(KeyCode.L))
      {
        Load();
      }
    }
    public void Save()
    {
      GetComponent<JsonSavingSystem>().Save(defaultSaveFile);
    }
    public void Load()
    {
      GetComponent<JsonSavingSystem>().Load(defaultSaveFile);
    }
  }
}

