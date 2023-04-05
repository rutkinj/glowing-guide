using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;


public class Projectile : MonoBehaviour
{
  [SerializeField] float projectileSpeed;
  [SerializeField] float projectileLifetime = 10f;
  [SerializeField] bool isHoming = false;
  [SerializeField] AudioClip hitSFX = null;
  HealthPoints target;
  GameObject instigator = null;
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
      return;
    }
    if (target.GetIsDead()) return;
    target.LoseHealth(instigator, damage);

    foreach (Transform child in transform)
    {
      child.gameObject.SetActive(false);
    }

    GetComponent<AudioSource>().PlayOneShot(hitSFX);
    Destroy(gameObject, 2);
  }

  public void SetTarget(HealthPoints target, GameObject instigator, float damage)
  {
    this.target = target;
    this.damage = damage;
    this.instigator = instigator;
  }

  private Vector3 GetAimLocation()
  {
    CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
    return target.transform.position + Vector3.up * targetCapsule.height / 2;
  }
}
