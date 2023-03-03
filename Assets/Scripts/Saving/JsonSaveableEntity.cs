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
      // foreach (IJsonSaveable saveable in stateDict)
      // {
   
      // }
      return null;
    }

    public void RestoreFromJToken(object state)
    {

    }

    // #if UNITY_EDITOR
    //   private void Update(){
    //   if (Application.IsPlaying(gameObject)) return;
    //   if (string.IsNullOrEmpty(gameObject.scene.path)) return;

    // }
  }
}