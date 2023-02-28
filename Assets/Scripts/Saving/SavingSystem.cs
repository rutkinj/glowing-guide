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
      string path = GetPathFromSaveName(saveName);
      print(GetPathFromSaveName(saveName));
      FileStream stream = File.Open(path, FileMode.Create);
      stream.WriteByte(42);
      stream.Close();
    }
    public void Load(string saveName)
    {
      print(GetPathFromSaveName(saveName));
    }

    private string GetPathFromSaveName(string saveName)
    {
      return Path.Combine(Application.persistentDataPath, saveName + ".sav");
    }
  }
}
