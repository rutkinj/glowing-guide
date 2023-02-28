using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RPG.Saving
{
  public class SavingSystem : MonoBehaviour
  {
    public void Save(string saveName)
    {
      print(GetPathFromSaveName(saveName));
    }
    public void Load(string saveName)
    {
      print(GetPathFromSaveName(saveName));
    }

    private string GetPathFromSaveName(string saveName)
    {
      return Path.Combine(Application.persistentDataPath, "saves", saveName + ".sav");
    }
  }
}
