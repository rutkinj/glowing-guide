using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

public class Projectile : MonoBehaviour
{
  [SerializeField] HealthPoints target;
  [SerializeField] float projectileSpeed;
  void Start(){
    transform.LookAt(GetAimLocation());
  }
  void Update()
  {
    transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
  }

  public void SetTarget(HealthPoints target)
  {
    this.target = target;
  }

  private Vector3 GetAimLocation()
  {
    CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
    return target.transform.position + Vector3.up * targetCapsule.height/2;
  }
}
