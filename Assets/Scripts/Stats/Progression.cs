using UnityEngine;

namespace RPG.Stats
{
  [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
  public class Progression : ScriptableObject
  {
    [SerializeField] ProgressionCharacterClass[] characterClass;
    [System.Serializable]
    class ProgressionCharacterClass
    {
      [SerializeField] CharacterClass className;
      [SerializeField] int[] maxHealth;
    }
  }
}

