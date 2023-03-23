using UnityEngine;
using System.Collections.Generic;
using System;

namespace RPG.Stats
{
  [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
  public class Progression : ScriptableObject
  {
    [SerializeField] ProgressionCharacterClass[] characterClasses;
    Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

    public float GetStat(Stat stat, CharacterClass characterClass, int level)
    {
      BuildLookup();
      var statTable = lookupTable[characterClass];
      float[] lvlArr = statTable[stat];
      
      if(lvlArr.Length < level) return 0;
      return lvlArr[level - 1];
    }

    private void BuildLookup()
    {
      if(lookupTable != null) return;

      lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

      foreach(ProgressionCharacterClass charClass in characterClasses){
        Dictionary<Stat, float[]> statDict = new Dictionary<Stat, float[]>();

        foreach(ProgressionStat pstat in charClass.stats){
          statDict.Add(pstat.stat, pstat.levels);
        }

        lookupTable.Add(charClass.className, statDict);
      }
    }

    [System.Serializable]
    class ProgressionCharacterClass
    {
      public CharacterClass className;
      public ProgressionStat[] stats;
    }

    [System.Serializable]
    class ProgressionStat
    {
      public Stat stat;
      public float[] levels;
    }

  }
}

