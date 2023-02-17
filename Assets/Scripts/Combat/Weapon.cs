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

    public void Spawn(Transform handTransform, Animator animator)
    {
      Instantiate(weaponPrefab, handTransform);
      animator.runtimeAnimatorController = animationOverride;
    }
  }
}
