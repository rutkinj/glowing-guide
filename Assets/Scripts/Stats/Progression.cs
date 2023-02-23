using UnityEngine;

namespace RPG.Stats
{
  [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
  public class Progression : ScriptableObject
  {
    [SerializeField] ProgressionCharacterClass[] characterClasses;

    public float GetHealth(CharacterClass characterClass, int level)
    {
      if (level < 1) return 0;

      foreach (ProgressionCharacterClass charClass in characterClasses)
      {
        if (charClass.getClass() == characterClass)
        {
          // return charClass.MaxHealth[level - 1];
        }
      }
      return 0;
    }

    [System.Serializable]
    class ProgressionCharacterClass
    {
      [SerializeField] CharacterClass className;
      [SerializeField] ProgressionStat stats;

      // public int[] MaxHealth { get => maxHealth; set => maxHealth = value; }

      public CharacterClass getClass()
      {
        return className;
      }
    }

    [System.Serializable]
    class ProgressionStat
    {
      [SerializeField] Stat[] stat;
      [SerializeField] float[] levels;
    }

  }
}

