using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
  [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
  public class Weapon : ScriptableObject
  {
    [SerializeField] AnimatorOverrideController animationOverride = null;
    [SerializeField] GameObject equippedPrefab = null;
    [SerializeField] float weaponDamage = 0f;
    [SerializeField] float weaponRange = 2f;
    [SerializeField] float attackDelay = 1f;

    public float WeaponDamage { get => weaponDamage; }
    public float WeaponRange { get => weaponRange; }
    public float AttackDelay { get => attackDelay; }

    public void Spawn(Transform handTransform, Animator animator)
    {
      if (equippedPrefab != null)
      {
        Instantiate(equippedPrefab, handTransform);
      }
      if (animationOverride != null)
      {
        animator.runtimeAnimatorController = animationOverride;
      }
    }
  }
}
