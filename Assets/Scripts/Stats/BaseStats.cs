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
    Experience experience = null;
    [SerializeField] int currentLevel = 0;

    private void Start() {
      experience = GetComponent<Experience>();
      currentLevel = CalculateLevel();

      if(experience != null){
        experience.onExpGain += UpdateLevel;
      }
    }

    private void UpdateLevel() {
      int newLevel = CalculateLevel();
      print("lvl: " + currentLevel);
      print("new lvl: " + newLevel);
      if(newLevel > currentLevel){
        currentLevel = newLevel;
      }
    }

    public float GetStat(Stat stat)
    {
      return progression.GetStat(stat, characterClass, startLevel);
    }

    public int GetLevel(){
      if(currentLevel < 1){
        currentLevel = CalculateLevel();
      }
      return currentLevel;
    }

    public int CalculateLevel()
    {
      if(experience == null) return startLevel;

      float currentExp = experience.GetExperiencePoints();
      print("currentExp: " + currentExp);
      int maxLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);

      for(int levels = 1; levels <= maxLevel; levels ++){
        float expToLevel = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, levels);
        if (currentExp < expToLevel) return levels;
      }
      return maxLevel + 1;
    }
  }
}

