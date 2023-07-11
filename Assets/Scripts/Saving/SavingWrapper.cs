using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{
  public class SavingWrapper : MonoBehaviour
  {
    const string defaultSaveFile = "defaultSave";

    JsonSavingSystem saveSys;

    private void Awake()
    {
      saveSys = GetComponent<JsonSavingSystem>();
      StartCoroutine(LoadLastScene());
    }

    private IEnumerator LoadLastScene()
    {
      yield return saveSys.LoadLastScene(defaultSaveFile);
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
      saveSys.Save(defaultSaveFile);
    }

    public void Load()
    {
      saveSys.Load(defaultSaveFile);
    }

    public void Delete()
    {
      saveSys.Delete(defaultSaveFile);
    }
  }
}

