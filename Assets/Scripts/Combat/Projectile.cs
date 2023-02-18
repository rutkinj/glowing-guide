using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

public class Projectile : MonoBehaviour
{
  [SerializeField] float projectileSpeed;
  HealthPoints target;
  float damage;
  void Start(){
    transform.LookAt(GetAimLocation());
  }
  void Update()
  {
    transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
  }

  private void OnTriggerEnter(Collider other) {
    if(other.GetComponent<HealthPoints>() != target){
        print("not the target");
        return;
    }
    target.TakeDamage(damage);
    Destroy(gameObject);
  }

  public void SetTarget(HealthPoints target, float damage)
  {
    this.target = target;
    this.damage = damage;
  }

  private Vector3 GetAimLocation()
  {
    CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
    return target.transform.position + Vector3.up * targetCapsule.height/2;
  }
}
