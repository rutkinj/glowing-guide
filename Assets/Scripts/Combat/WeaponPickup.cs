using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
  public class WeaponPickup : MonoBehaviour, IRaycastable
  {
    [SerializeField] WeaponConfig weapon = null;
    [SerializeField] float repawnTime = 5f;

    private void OnTriggerEnter(Collider other)
    {
      if (other.CompareTag("Player"))
      {
        Pickup(other.GetComponent<Fighter>());
      }
    }

    private void Pickup(Fighter fighter)
    {
      fighter.EquipWeapon(weapon);
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

    public CursorType GetCursorType(){
      return CursorType.pickup;
    }

    public bool HandleRaycast(PlayerController callingController)
    {
      if (Input.GetMouseButtonDown(0))
      {
        Pickup(callingController.GetComponent<Fighter>());
      }
      return true;
    }
  }
}
