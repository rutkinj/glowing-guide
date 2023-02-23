using UnityEngine;

namespace RPG.Stats
{
  [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
  public class Progression : ScriptableObject
  {
    [SerializeField] ProgressionCharacterClass[] characterClasses;

    public float GetStat(Stat stat, CharacterClass characterClass, int level)
    {
      if (level < 1) return 0;

      foreach (ProgressionCharacterClass charClass in characterClasses)
      {
        if (charClass.className == characterClass)
        {
          foreach(ProgressionStat pStat in charClass.stats){
            if(pStat.levels.Length < level) continue;
            if (pStat.stat == stat) return pStat.levels[level-1];
          }
        }
      }
      return 0;
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

