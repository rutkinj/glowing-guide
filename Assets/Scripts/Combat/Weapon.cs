using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Combat
{
  [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
  public class Weapon : ScriptableObject
  {
    [SerializeField] AnimatorOverrideController animationOverride = null;
    [SerializeField] GameObject equippedPrefab = null;
    [SerializeField] bool rightHandEquip = true;
    [SerializeField] float weaponDamage = 0f;
    [SerializeField] float weaponRange = 2f;
    [SerializeField] float attackDelay = 1f;
    [SerializeField] Projectile projectile = null;
    const string weaponName = "Weapon";
    public float WeaponDamage { get => weaponDamage; }
    public float WeaponRange { get => weaponRange; }
    public float AttackDelay { get => attackDelay; }

    public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
    {
      DestroyOldWeapon(rightHand, leftHand);
      if (equippedPrefab != null)
      {
        Transform hand = rightHandEquip ? rightHand : leftHand;
        GameObject weapon = Instantiate(equippedPrefab, hand);
        weapon.name = weaponName;
      }
      if (animationOverride != null)
      {
        animator.runtimeAnimatorController = animationOverride;
      }
    }

    private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
    {
      Transform oldWeapon = rightHand.Find(weaponName);
      if (oldWeapon == null)
      {
        oldWeapon = leftHand.Find(weaponName);
      }
      if (oldWeapon == null) return;

      oldWeapon.name = "DESTROYING";
      Destroy(oldWeapon.gameObject);
    }

    public bool HasProjectile()
    {
      return projectile != null;
    }

    public void LaunchProjectile(Transform rightHand, Transform leftHand, HealthPoints target)
    {
      Transform hand = rightHandEquip ? rightHand : leftHand;
      Projectile projectileInstance = Instantiate(projectile, hand.position, Quaternion.identity);
      projectileInstance.SetTarget(target, weaponDamage);
    }
  }
}
