using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
  public class BaseStats : MonoBehaviour
  {
    [SerializeField] Progression progression = null;
    [SerializeField] CharacterClass characterClass;
    [Range(1,99)][SerializeField] int startLevel = 1;

    public float GetHealth(){
        return progression.GetHealth(characterClass, startLevel);
    }
  }
}

