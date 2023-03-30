using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
  public class WeaponPickup : MonoBehaviour, IRaycastable
  {
    [SerializeField] Weapon weapon = null;
    [SerializeField] float repawnTime = 5f;

    private void OnTriggerEnter(Collider other)
    {
      if (other.CompareTag("Player"))
      {
        Pickup(other);
      }
    }

    private void Pickup(Collider other)
    {
      other.GetComponent<Fighter>().EquipWeapon(weapon);
      StartCoroutine(HideForSeconds(repawnTime));
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

    public bool HandleRaycast()
    {
      if (Input.GetMouseButtonDown(0))
      {
        Pickup(FindObjectOfType<PlayerController>().GetComponent<Collider>());
        return true;
      }
      return false;
    }
  }
}
