using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Saving
{
  public class JsonSavingSystem : MonoBehaviour
  {
    private const string ext = ".json";

    public IEnumerator LoadLastScene(string saveFile){
      JObject state = 
    }
    public void Save(string saveName)
    {
      string path = GetPathFromSaveName(saveName);
      print("Saving to: " + path);
      using (FileStream stream = File.Open(path, FileMode.Create))
      {
        //json magic capture state
        // stream.Write(buffer, 0, buffer.Length);
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

    private void CaptureAsToken(JObject state)
    {
      IDictionary<string, JToken> stateDict = state;
      foreach (JsonSaveableEntity saveable in FindObjectsOfType<JsonSaveableEntity>())
      {
        stateDict[saveable.GetUniqueID()] = saveable.CaptureAsJToken();
      }
    }

    private void RestoreState(JObject state)
    {
      IDictionary<string, JToken> stateDict = state;
      foreach (JsonSaveableEntity saveable in FindObjectsOfType<JsonSaveableEntity>())
      {
        string id = saveable.GetUniqueID();
        if (stateDict.ContainsKey(id))
        {
          saveable.RestoreFromJToken(stateDict[id]);
        }
      }

    }

    private JObject LoadJsonFromFile(string saveFile){
      string path = GetPathFromSaveName(saveFile);
      if(!File.Exists(path)) return new JObject();

      using (var textReader = File.OpenText(path)){
        using (var reader = new JsonTextReader(textReader)){
          reader.FloatParseHandling = FloatParseHandling.Double;

          return JObject.Load(reader);
        }
      }
    }

    private string GetPathFromSaveName(string saveName)
    {
      return Path.Combine(Application.persistentDataPath, saveName + ext);
    }
  }
}
