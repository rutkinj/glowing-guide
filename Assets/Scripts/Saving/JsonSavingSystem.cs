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

    public IEnumerator LoadLastScene(string saveFile)
    {
      JObject state = LoadJsonFromFile(saveFile);
      IDictionary<string, JToken> statedict = state;
      int buildIndex = SceneManager.GetActiveScene().buildIndex;
      if (statedict.ContainsKey("lastSceneBuildIndex"))
      {
        buildIndex = (int)statedict["lastSceneBuildIndex"];
      }
      yield return SceneManager.LoadSceneAsync(buildIndex);
      RestoreFromToken(state);
    }
    public void Save(string saveName)
    {
      JObject state = LoadJsonFromFile(saveName);
      CaptureAsToken(state);
      SaveFileAsJson(saveName, state);
    }
    public void Load(string saveName)
    {
      RestoreFromToken(LoadJsonFromFile(saveName));
    }

    public void Delete(string saveName)
    {
      File.Delete(GetPathFromSaveName(saveName));
    }

    private void CaptureAsToken(JObject state)
    {
      IDictionary<string, JToken> stateDict = state;
      foreach (JsonSaveableEntity saveable in FindObjectsOfType<JsonSaveableEntity>())
      {
        stateDict[saveable.GetUniqueID()] = saveable.CaptureAsJToken();
      }
    }

    private void RestoreFromToken(JObject state)
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

    private void SaveFileAsJson(string saveName, JObject state)
    {
      string path = GetPathFromSaveName(saveName);

      using (var textWriter = File.CreateText(path))
      {
        using (var writer = new JsonTextWriter(textWriter))
        {
          writer.Formatting = Formatting.Indented;

          state.WriteTo(writer);
        }
      }
    }

    private JObject LoadJsonFromFile(string saveName)
    {
      string path = GetPathFromSaveName(saveName);
      if (!File.Exists(path)) return new JObject();

      using (var textReader = File.OpenText(path))
      {
        using (var reader = new JsonTextReader(textReader))
        {
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
