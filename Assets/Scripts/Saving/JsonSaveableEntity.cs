using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace RPG.Saving
{
  [ExecuteAlways]
  public class JsonSaveableEntity : MonoBehaviour
  {
    [SerializeField] string uniqueID = "";

    static Dictionary<string, JsonSaveableEntity> globalLookup = new Dictionary<string, JsonSaveableEntity>();

    public string GetUniqueID()
    {
      return uniqueID;
    }

    public JToken CaptureAsJToken()
    {
      JObject state = new JObject();
      IDictionary<string, JToken> stateDict = state;
      foreach (IJsonSaveable saveable in GetComponents<IJsonSaveable>())
      {
        JToken token = saveable.CaptureAsJToken();
        string component = saveable.GetType().ToString();
        Debug.Log($"{name} Capture {component} = {token.ToString()}");
        stateDict[saveable.GetType().ToString()] = token;
      }
      return null;
    }

    public void RestoreFromJToken(object state)
    {

    }

#if UNITY_EDITOR
    private void Update()
    {
      if (Application.IsPlaying(gameObject)) return;
      if (string.IsNullOrEmpty(gameObject.scene.path)) return;

      SerializedObject serializedObject = new SerializedObject(this);
      SerializedProperty property = serializedObject.FindProperty("uniqueID");

      if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
      {
        property.stringValue = System.Guid.NewGuid().ToString();
        serializedObject.ApplyModifiedProperties();
      }
      globalLookup[property.stringValue] = this;
    }

#endif

    private bool IsUnique(string key)
    {
      if (!globalLookup.ContainsKey(key)) return true;
      if (globalLookup[key] == this) return true;
      if (globalLookup[key] == null)
      {
        globalLookup.Remove(key);
        return true;
      }
      if (globalLookup[key].GetUniqueID() != key)
      {
        globalLookup.Remove(key);
        return true;
      }
      return false;
    }
  }
}