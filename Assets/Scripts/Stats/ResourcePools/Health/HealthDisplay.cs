using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG.Stats.ResourcePools
{
  public class HealthDisplay : MonoBehaviour
  {
    HealthPoints healthPoints;
    TextMeshProUGUI healthDisplayText;

    private void Awake() {
        healthPoints = GameObject.FindWithTag("Player").GetComponent<HealthPoints>();
        healthDisplayText = GetComponent<TextMeshProUGUI>();
    }

    private void Update() {
        healthDisplayText.SetText(healthPoints.CurrentHealthAsString());
    }
  }
}

