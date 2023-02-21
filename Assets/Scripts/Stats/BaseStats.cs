using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
  public class BaseStats : MonoBehaviour
  {
    [Range(1,99)][SerializeField] int startLevel = 1;
    [SerializeField] CharacterClass characterClass;
  }
}

