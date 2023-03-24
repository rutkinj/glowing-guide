using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Attributes;
using RPG.Movement;
using RPG.Saving;
using RPG.Stats;
using Newtonsoft.Json.Linq;

namespace RPG.Combat
{
  public class Fighter : MonoBehaviour, IAction, IJsonSaveable, IModifierProvider
  {
    [SerializeField] Transform rightHandTransform = null;
    [SerializeField] Transform leftHandTransform = null;
    [SerializeField] Weapon defaultWeapon = null;
    HealthPoints target;
    Mover mover;
    Weapon currentWeapon = null;
    float timeSinceLastAttack = Mathf.Infinity;

    private void Start()
    {
      mover = GetComponent<Mover>();
      EquipWeapon(defaultWeapon);
    }

    private void Update()
    {
      timeSinceLastAttack += Time.deltaTime;

      if (target == null) return;

      if (target != null && !GetIsInRange())
      {
        mover.MoveTo(target.transform.position);
      }
      else
      {
        mover.Cancel();
        AttackBehavior();
      }
    }

    public void Attack(GameObject combatTarget)
    {
      GetComponent<ActionScheduler>().StartAction(this);
      target = combatTarget.GetComponent<HealthPoints>();
    }

    public bool CanAttack(GameObject combatTarget)
    {
      if (combatTarget == null) return false;
      HealthPoints testTarget = combatTarget.GetComponent<HealthPoints>();
      return testTarget != null && !testTarget.GetIsDead();
    }

    public void EquipWeapon(Weapon weapon)
    {
      // if(weapon == null) return;
      currentWeapon = weapon;
      Animator animator = GetComponent<Animator>();
      weapon.Spawn(rightHandTransform, leftHandTransform, animator);
    }

    public HealthPoints GetTarget()
    {
      return target;
    }

    private void AttackBehavior()
    {
      transform.LookAt(target.transform.position);
      if (timeSinceLastAttack >= currentWeapon.AttackDelay && !target.GetIsDead())
      {
        GetComponent<Animator>().ResetTrigger("cancelAttack");
        GetComponent<Animator>().SetTrigger("attack");
        timeSinceLastAttack = 0f;
      }
    }

    private bool GetIsInRange()
    {
      return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.WeaponRange;
    }

    ///// ### Animation Events ###
    private void Hit()
    {
      if (target == null) return;
      float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
      if (currentWeapon.HasProjectile())
      {
        currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
      }
      else
      {

        target.LoseHealth(gameObject, damage);
      }
    }

    private void Shoot()
    {
      Hit();
    }

    ///// ### Interface Implementations ###
    ///// IAction
    public void Cancel()
    {
      GetComponent<Animator>().ResetTrigger("attack");
      GetComponent<Animator>().SetTrigger("cancelAttack");
      target = null;
    }

    ///// IJsonSaveable
    public JToken CaptureAsJToken()
    {
      return JToken.FromObject(currentWeapon.name);
    }

    ///// IJsonSaveable
    public void RestoreFromJToken(JToken state)
    {
      string weaponName = state.ToObject<string>();
      Weapon weapon = UnityEngine.Resources.Load<Weapon>(weaponName);
      EquipWeapon(weapon);
    }

    ///// IModifierProvider
    public IEnumerable<float> GetModifiers(Stat stat)
    {
      throw new System.NotImplementedException();
    }
  }
}
