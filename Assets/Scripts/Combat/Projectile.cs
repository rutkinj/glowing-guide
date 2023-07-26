using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Stats.ResourcePools;


public class Projectile : MonoBehaviour
{
  [SerializeField] float projectileSpeed;
  [SerializeField] float projectileLifetime = 10f;
  [SerializeField] bool isHoming = false;
  [SerializeField] AudioClip hitSFX = null;

  Vector3 target;
  GameObject instigator = null;
  float damage;

  void Start()
  {
    transform.LookAt(GetAimLocation());
    Destroy(gameObject, projectileLifetime);
  }

  void Update()
  {
    // TODO make homing work
    // if (isHoming && !target.GetIsDead()) transform.LookAt(GetAimLocation());
    transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
  }

  private void OnTriggerEnter(Collider other)
  {
    // projectiles cant hit their creators
    if (other.gameObject == instigator) return;

    HealthPoints hitHP = other.GetComponent<HealthPoints>();
    if (hitHP == null) return;
    if (hitHP.GetIsDead()) return;
    //TODO projectiles fly through anything w/o hp; props, enviro, etc

    DoHit(hitHP);
  }

  public void SetTarget(Vector3 target, GameObject instigator, float damage)
  {
    this.target = target;
    this.damage = damage;
    this.instigator = instigator;
  }

  private Vector3 GetAimLocation()
  {
    if (!Mathf.Approximately(target.y, 1))
    {
      return new Vector3(target.x, 1, target.z);
    }
    else
    {
      return target;
    }
  }

  private void DoHit(HealthPoints hitHP)
  {
    // deal damage
    hitHP.LoseHealth(instigator, damage);
    // hide mesh, disable collider; TODO onHit effect? projectile stuck in target?
    foreach (Transform child in transform)
    {
      child.gameObject.SetActive(false);
    }
    // play hit audio
    GetComponent<AudioSource>().PlayOneShot(hitSFX);
    //destroy self after audio finishes
    Destroy(gameObject, 2);
  }
}
