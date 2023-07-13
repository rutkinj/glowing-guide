using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG.Stats
{
  public class LevelDisplay : MonoBehaviour
  {
    BaseStats baseStats;
    TextMeshProUGUI lvlDisplayText;
    private void Awake()
    {
      baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
      lvlDisplayText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
      lvlDisplayText.SetText(baseStats.GetLevel().ToString());
    }
  }
}
