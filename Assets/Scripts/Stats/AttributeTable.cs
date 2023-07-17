using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
  [CreateAssetMenu(fileName = "Progression", menuName = "Stats/AttributeTable")]
  public class AttributeTable : ScriptableObject
  {
    [SerializeField] ProgressionAttribute[] attributes;
    Dictionary<E_Attribute, Dictionary<Stat, float[]>> lookupTable = null;

    public float GetStat(Stat stat, BaseAttributes baseAtt)
    {
      BuildLookup();
      float result = 0;
      foreach (var kv in lookupTable)
      { // check if the attribute affects the given stat
        if (kv.Value.TryGetValue(stat, out float[] lvlArr))
        { // if it does, find the attributes level and add the related amount
          result += lvlArr[baseAtt.GetAttributeLevel(kv.Key) - 1];
        }
      }
      return result;
    }

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
