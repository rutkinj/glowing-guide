using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats.ResourcePools
{
  public class HealthBar : MonoBehaviour
  {
    [SerializeField] HealthPoints hpComponent = null;
    [SerializeField] RectTransform hpBar = null;
    [SerializeField] Canvas canvas = null;

    void Update()
    {
      float percent = hpComponent.GetHPPercentage() / 100;
      if (percent == 0 || percent == 1)
      {
        canvas.enabled = false;
        return;
      }
      else if (percent != 0 && canvas.enabled == false)
      {
        canvas.enabled = true;
      }
      hpBar.localScale = new Vector3(percent, 1, 1);
    }
  }
}
