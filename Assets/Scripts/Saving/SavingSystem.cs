using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{
  public class SavingSystem : MonoBehaviour
  {
    public void Save(string saveName)
    {
      print("Saving to: " + saveName);
    }
    public void Load(string saveName)
    {
      print("Loading from: " + saveName);
    }
  }
}
