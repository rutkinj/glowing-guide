using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
  [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
  public class Weapon : ScriptableObject
  {
    [SerializeField] AnimatorOverrideController animationOverride = null;
    [SerializeField] GameObject weaponPrefab = null;
    [SerializeField] float weaponDamage = 12f;
    [SerializeField] float weaponRange = 2f;
    [SerializeField] float attackDelay = 1f;

    public float WeaponDamage { get => weaponDamage; }
    public float WeaponRange { get => weaponRange; }
    public float AttackDelay { get => attackDelay; }

    public void Spawn(Transform handTransform, Animator animator)
    {
      Instantiate(weaponPrefab, handTransform);
      animator.runtimeAnimatorController = animationOverride;
    }
  }
}
