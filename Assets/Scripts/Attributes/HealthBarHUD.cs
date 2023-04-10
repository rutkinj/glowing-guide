using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
  public class HealthBarHUD : MonoBehaviour
  {
    [SerializeField] RectTransform hpBar = null;
    HealthPoints hpComponent;

    private void Awake()
    {
      hpComponent = GameObject.FindWithTag("Player").GetComponent<HealthPoints>();
    }
    void Update()
    {
      float percent = hpComponent.GetHPPercentage() / 100;
      hpBar.localScale = new Vector3(percent, 1, 1);
    }
  }
}


