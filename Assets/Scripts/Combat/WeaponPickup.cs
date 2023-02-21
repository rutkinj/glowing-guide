using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat{
  public class WeaponPickup : MonoBehaviour
  {
    [SerializeField] Weapon weapon = null;
    [SerializeField] float repawnTime = 5f;

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            other.GetComponent<Fighter>().EquipWeapon(weapon);
            StartCoroutine(HideForSeconds(repawnTime));
        }
    }

    private IEnumerator HideForSeconds(float timeToWait){
      GetComponent<CapsuleCollider>().enabled = false;
      foreach(Transform child in transform){
        child.gameObject.SetActive(false);
      }
      yield return new WaitForSeconds(timeToWait);
      GetComponent<CapsuleCollider>().enabled = true;
      foreach (Transform child in transform)
      {
        child.gameObject.SetActive(true);
      }
    }
  }
}
