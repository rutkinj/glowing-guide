using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RPG.Attributes;

namespace RPG.Combat
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
        healthDisplayText.SetText(target.CurrentHealthAsString());
      }
      else
      {
        healthDisplayText.SetText("No Target");
      }
    }
  }
}


