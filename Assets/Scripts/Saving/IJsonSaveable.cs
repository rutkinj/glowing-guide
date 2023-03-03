using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace RPG.Saving
{
  public interface IJsonSaveable
  {
    JToken CaptureAsJToken();

    void RestoreFromJToken(JToken state);
  }
}
