using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Stats.ResourcePools;
using System;
using RPG.Inventories;

namespace RPG.Combat
{
  [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
  public class WeaponConfig : EquipableItem
  {
    [SerializeField] AnimatorOverrideController animationOverride = null;
    [SerializeField] Weapon equippedPrefab = null;
    [SerializeField] bool rightHandEquip = true;
    [SerializeField] float weaponDamage = 0f;
    [SerializeField] float weaponRange = 2f;
    [SerializeField] float attackDelay = 1f;
    [SerializeField] Projectile projectile = null;
    const string weaponName = "Weapon";
    public float WeaponDamage { get => weaponDamage; }

    [Range(-1,1)] public float DamagePercentMod = 0;
    public float WeaponRange { get => weaponRange; }
    public float AttackDelay { get => attackDelay; }

    public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
    {
      DestroyOldWeapon(rightHand, leftHand);

      Weapon weapon = null;
      if (equippedPrefab != null)
      {
        Transform hand = rightHandEquip ? rightHand : leftHand;
        weapon = Instantiate(equippedPrefab, hand);
        weapon.gameObject.name = weaponName;
      }
      if (animationOverride != null)
      {
        animator.runtimeAnimatorController = animationOverride;
      }
      else
      {
        var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
        if (overrideController != null)
        {
          animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
        }
      }
      return weapon;
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

    public void LaunchProjectile(Transform rightHand, Transform leftHand, HealthPoints target, GameObject instigator, float calculatedDamage)
    {
      Transform hand = rightHandEquip ? rightHand : leftHand;
      Projectile projectileInstance = Instantiate(projectile, hand.position, Quaternion.identity);
      projectileInstance.SetTarget(target, instigator, calculatedDamage);
    }
    
  }
}
