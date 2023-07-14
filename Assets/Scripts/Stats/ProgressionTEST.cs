using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
  [CreateAssetMenu(fileName = "Progression", menuName = "Stats/TESTProgression")]
  public class ProgressionTEST : ScriptableObject
  {
    [SerializeField] ProgressionAttribute[] attributes;
    Dictionary<E_Attribute, Dictionary<Stat, float[]>> lookupTable = null;

    // public float GetStat(Stat stat, CharacterClass characterClass, int level)
    // {
    //   BuildLookup();
    //   var statTable = lookupTable[characterClass];
    //   float[] lvlArr = statTable[stat];

    //   if (lvlArr.Length < level) return 0;
    //   return lvlArr[level - 1];
    // }

    // public int GetLevels(Stat stat, CharacterClass characterClass)
    // {
    //   BuildLookup();
    //   float[] levels = lookupTable[characterClass][stat];
    //   return levels.Length;
    // }

    private void BuildLookup()
    {
      if (lookupTable != null) return;

      lookupTable = new Dictionary<E_Attribute, Dictionary<Stat, float[]>>();

      foreach (ProgressionAttribute att in attributes)
      {
        Dictionary<Stat, float[]> statDict = new Dictionary<Stat, float[]>();

        foreach (ProgressionStat pstat in att.stats)
        {
          statDict.Add(pstat.stat, pstat.levels);
        }

        lookupTable.Add(att.attribute, statDict);
      }
    }

    [System.Serializable]
    class ProgressionAttribute
    {
      public E_Attribute attribute;
      public ProgressionStat[] stats;
    }

    [System.Serializable]
    class ProgressionStat
    {
      public Stat stat;
      public float[] levels = new float[10];
    }

  }
}
