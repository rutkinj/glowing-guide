using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
  public class BaseStats : MonoBehaviour
  {
    [SerializeField] Progression progression = null;
    [SerializeField] CharacterClass characterClass;
    [Range(1, 3)][SerializeField] int startLevel = 1;

    public float GetStat(Stat stat)
    {
      return progression.GetStat(stat, characterClass, startLevel);
    }
  }
}

