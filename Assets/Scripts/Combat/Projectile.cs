using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;


public class Projectile : MonoBehaviour
{
  [SerializeField] float projectileSpeed;
  [SerializeField] float projectileLifetime = 10f;
  [SerializeField] bool isHoming = false;
  HealthPoints target;
  float damage;
  void Start()
  {
    transform.LookAt(GetAimLocation());
    Destroy(gameObject, projectileLifetime);
  }
  void Update()
  {
    if (isHoming && !target.GetIsDead()) transform.LookAt(GetAimLocation());
    transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.GetComponent<HealthPoints>() != target)
    {
      print("not the target");
      return;
    }
    if (target.GetIsDead()) return;
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
    return target.transform.position + Vector3.up * targetCapsule.height / 2;
  }
}
