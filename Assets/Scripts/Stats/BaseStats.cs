using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Utils;
using RPG.Stats.ResourcePools;
using UnityEngine;

namespace RPG.Stats
{
  public class BaseStats : MonoBehaviour
  {
    [SerializeField] Progression progression = null;
    [SerializeField] AttributeTable attTable = null;
    [SerializeField] CharacterClass characterClass;
    [SerializeField] GameObject levelUpEffect;
    [Range(1, 3)][SerializeField] int startLevel = 1;
    [SerializeField] bool useModifiers;
    Experience experience = null;
    BaseAttributes baseAttributes = null;
    LazyValue<int> currentLevel;

    public event Action onLevelUp;

    private void Awake()
    {
      experience = GetComponent<Experience>();
      baseAttributes = GetComponent<BaseAttributes>();
      currentLevel = new LazyValue<int>(CalculateLevel);
    }

    private void Start()
    {
      currentLevel.ForceInit();
    }

    private void OnEnable()
    {
      if (experience != null)
      {
        experience.onExpGain += UpdateLevel;
      }
    }

    private void OnDisable()
    {
      if (experience != null)
      {
        experience.onExpGain -= UpdateLevel;
      }
    }

    private void UpdateLevel()
    {
      int newLevel = CalculateLevel();
      if (newLevel > currentLevel.value)
      {
        currentLevel.value = newLevel;
        LevelUpEffect();
        onLevelUp();
      }
      else if (newLevel < currentLevel.value)
      {
        currentLevel.value = newLevel;
        onLevelUp();
      }
    }

    private void LevelUpEffect()
    {
      Instantiate(levelUpEffect, transform);
    }

    public float GetStat(Stat stat)
    {
      return GetBaseStat(stat) + GetAdditiveMods(stat) * GetPercentileMods(stat);
    }

    private float GetBaseStat(Stat stat)
    {
      return progression.GetStat(stat, characterClass, GetLevel()) + attTable.GetStat(stat, baseAttributes);
    }

    private float GetAdditiveMods(Stat stat)
    {
      float totalModifier = 0;
      if (!useModifiers) return totalModifier;

      IModifierProvider[] providers = GetComponents<IModifierProvider>();
      foreach (var provider in providers)
      {
        IEnumerable<float> mods = provider.GetAdditiveModifiers(stat);
        foreach (float mod in mods)
        {
          totalModifier += mod;
        }
      }
      return totalModifier;
    }

    private float GetPercentileMods(Stat stat)
    {
      float totalModifier = 1;
      if (!useModifiers) return totalModifier;

      IModifierProvider[] providers = GetComponents<IModifierProvider>();
      foreach (var provider in providers)
      {
        IEnumerable<float> mods = provider.GetPercentileModifiers(stat);
        foreach (float mod in mods)
        {
          totalModifier += mod;
        }
      }

      return totalModifier;
    }

    public int GetLevel()
    {
      return currentLevel.value;
    }

    public int CalculateLevel()
    {
      if (experience == null) return startLevel;

      float currentExp = experience.GetExperiencePoints();
      int maxLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);

      for (int levels = 1; levels <= maxLevel; levels++)
      {
        float expToLevel = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, levels);
        if (currentExp < expToLevel) return levels;
      }
      return maxLevel + 1;
    }
  }
}

