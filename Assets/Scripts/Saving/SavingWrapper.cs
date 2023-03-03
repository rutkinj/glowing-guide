using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{
  public class SavingWrapper : MonoBehaviour
  {
    const string defaultSaveFile = "defaultSave";

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.S))
      {
        GetComponent<JsonSavingSystem>().Save(defaultSaveFile);
      }
      if (Input.GetKeyDown(KeyCode.L))
      {
        GetComponent<JsonSavingSystem>().Load(defaultSaveFile);
      }
    }
  }
}

