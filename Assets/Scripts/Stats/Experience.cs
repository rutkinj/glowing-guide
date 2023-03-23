using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RPG.Saving;
using UnityEngine;

namespace RPG.Attributes
{
  public class Experience : MonoBehaviour, IJsonSaveable
  {
    [SerializeField] float experiencePoints = 0;

    public float GetExperiencePoints()
    {
      return experiencePoints;
    }
    public void GainExperience(float exp)
    {
      experiencePoints += exp;
    }
    public JToken CaptureAsJToken()
    {
      return JToken.FromObject(experiencePoints);
    }

    public void RestoreFromJToken(JToken state)
    {
      experiencePoints = state.ToObject<float>();
    }
  }

}