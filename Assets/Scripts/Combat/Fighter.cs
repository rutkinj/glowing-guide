using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Attributes;
using RPG.Movement;
using RPG.Saving;
using Newtonsoft.Json.Linq;

namespace RPG.Combat
{
  public class Fighter : MonoBehaviour, IAction, IJsonSaveable
  {
    [SerializeField] Transform rightHandTransform = null;
    [SerializeField] Transform leftHandTransform = null;
    [SerializeField] Weapon defaultWeapon = null;
    HealthPoints target;
    Mover mover;
    float timeSinceLastAttack = Mathf.Infinity;
    Weapon currentWeapon = null;

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

    private void AttackBehavior()
    {
      transform.LookAt(target.transform.position);
      if (timeSinceLastAttack >= currentWeapon.AttackDelay && !target.GetIsDead())
      {
        GetComponent<Animator>().ResetTrigger("cancelAttack");
        GetComponent<Animator>().SetTrigger("attack"); //And triggers Hit() found below
        timeSinceLastAttack = 0f;
      }
    }

    //Animation Event
    private void Hit()
    {
      if (target == null) return;
      if (currentWeapon.HasProjectile())
      {
        currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject);
      }
      else
      {
        target.LoseHealth(gameObject, currentWeapon.WeaponDamage);
      }
    }

    void Shoot()
    {
      Hit();
    }

    public void Attack(GameObject combatTarget)
    {
      GetComponent<ActionScheduler>().StartAction(this);
      target = combatTarget.GetComponent<HealthPoints>();
    }

    public void Cancel()
    {
      GetComponent<Animator>().ResetTrigger("attack");
      GetComponent<Animator>().SetTrigger("cancelAttack");
      target = null;
    }

    private bool GetIsInRange()
    {
      return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.WeaponRange;
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

    public HealthPoints GetTarget(){
      return target;
    }

    public JToken CaptureAsJToken()
    {
      return JToken.FromObject(currentWeapon.name);
    }

    public void RestoreFromJToken(JToken state)
    {
      string weaponName = state.ToObject<string>();
      Weapon weapon = UnityEngine.Resources.Load<Weapon>(weaponName);
      EquipWeapon(weapon);
    }
  }
}
