using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RPG.Combat;

namespace RPG.Attributes
{
  public class EnemyHealthDisplay : MonoBehaviour
  {
    Fighter player;
    TextMeshProUGUI healthDisplayText;

    private void Awake()
    {
      player = GameObject.FindWithTag("Player").GetComponent<Fighter>();
      healthDisplayText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
      HealthPoints target = player.GetTarget();
      if (target != null)
      {
        healthDisplayText.SetText(string.Format("{0:0}%", target.GetHPPercentage()));
      }
      else
      {
        healthDisplayText.SetText("No Target");
      }
    }
  }
}


