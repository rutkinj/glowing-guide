using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RPG.Saving;
using UnityEngine;

namespace RPG.Stats.ResourcePools
{
  public class Experience : MonoBehaviour, IJsonSaveable
  {
    [SerializeField] float experiencePoints = 0;

    public event Action onExpGain;

    public float GetExperiencePoints()
    {
      return experiencePoints;
    }

    public void GainExperience(float exp)
    {
      experiencePoints += exp;
      onExpGain();
    }

    public JToken CaptureAsJToken()
    {
      return JToken.FromObject(experiencePoints);
    }

    public void RestoreFromJToken(JToken state)
    {
      experiencePoints = state.ToObject<float>();
      onExpGain();
    }
  }

}