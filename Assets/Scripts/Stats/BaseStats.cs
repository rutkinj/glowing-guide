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

    public int GetLevel()
    {
      Experience experience = GetComponent<Experience>();
      if(experience == null) return startLevel;

      float currentExp = experience.GetExperiencePoints();
      int maxLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
      
      for(int levels = 1; levels < maxLevel; levels ++){
        float expToLevel = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, levels);
        if (currentExp < expToLevel) return levels;
      }
      return maxLevel + 1;
    }
  }
}

