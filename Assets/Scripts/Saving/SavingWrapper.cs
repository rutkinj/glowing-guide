using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{
  public class SavingWrapper : MonoBehaviour
  {
    const string defaultSaveFile = "defaultSave";

    JsonSavingSystem save;

    private void Awake()
    {
      StartCoroutine(LoadLastScene());
    }

    private IEnumerator LoadLastScene()
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
      if (Input.GetKeyDown(KeyCode.D))
      {
        Delete();
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

    public void Delete()
    {
      GetComponent<JsonSavingSystem>().Delete(defaultSaveFile);
    }
  }
}

