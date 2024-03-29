using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG.Stats.ResourcePools
{
  public class ExperienceDisplay : MonoBehaviour
  {
    Experience exp;
    TextMeshProUGUI expDisplayText;
    private void Awake()
    {
      exp = GameObject.FindWithTag("Player").GetComponent<Experience>();
      expDisplayText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
      expDisplayText.SetText(exp.GetExperiencePoints().ToString());
    }
  }
}
