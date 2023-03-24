using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
  public class BaseStats : MonoBehaviour
  {
    [SerializeField] Progression progression = null;
    [SerializeField] CharacterClass characterClass;
    [SerializeField] GameObject levelUpEffect;
    [Range(1, 3)][SerializeField] int startLevel = 1;
    Experience experience = null;
    int currentLevel = 0;
    
    public event Action onLevelUp;

    private void Start() {
      experience = GetComponent<Experience>();
      currentLevel = CalculateLevel();

      if(experience != null){
        experience.onExpGain += UpdateLevel;
      }
    }

    private void UpdateLevel() {
      int newLevel = CalculateLevel();
      if(newLevel > currentLevel){
        currentLevel = newLevel;
        LevelUpEffect();
        onLevelUp();
      }
    }

    private void LevelUpEffect()
    {
      Instantiate(levelUpEffect, transform);
    }

    public float GetStat(Stat stat)
    {
      return progression.GetStat(stat, characterClass, GetLevel())  + GetModifier(stat);
    }

    private float GetModifier(Stat stat)
    {
      throw new NotImplementedException();
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

