using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats.ResourcePools
{
  public class HealthPickup : MonoBehaviour
  {
    [SerializeField] float respawnTime = 3f;
    [SerializeField] float healAmount = 20f;

    private void OnTriggerEnter(Collider other)
    {
      if (other.CompareTag("Player"))
      {
        other.GetComponent<HealthPoints>().GainHealth(healAmount);
        StartCoroutine(HideForSeconds(respawnTime));
      }
    }

    private IEnumerator HideForSeconds(float timeToWait)
    {
      ShowPickup(false);
      yield return new WaitForSeconds(timeToWait);
      ShowPickup(true);
    }

    private void ShowPickup(bool showOrNo)
    {
      GetComponent<Collider>().enabled = showOrNo;
      foreach (Transform child in transform)
      {
        child.gameObject.SetActive(showOrNo);
      }
    }
  }
}

