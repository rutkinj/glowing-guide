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
      print("Saving to: " + path);
      using (FileStream stream = File.Open(path, FileMode.Create))
      {
        stream.WriteByte(42);
      }
    }
    public void Load(string saveName)
    {
      string path = GetPathFromSaveName(saveName);
      print("Loading from: " + path);
      using (FileStream stream = File.Open(path, FileMode.Open))
      {
        byte[] buffer = new byte[stream.Length];
        stream.Read(buffer, 0, buffer.Length);
      }
    }

    private string GetPathFromSaveName(string saveName)
    {
      return Path.Combine(Application.persistentDataPath, saveName + ".sav");
    }
  }
}
