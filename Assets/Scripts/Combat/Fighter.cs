using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Attributes;
using RPG.Movement;
using RPG.Saving;
using RPG.Stats;
using Newtonsoft.Json.Linq;
using RPG.Utils;
using RPG.Inventories;
using System;

namespace RPG.Combat
{
  public class Fighter : MonoBehaviour, IAction, IJsonSaveable
  {
    [SerializeField] Transform rightHandTransform = null;
    [SerializeField] Transform leftHandTransform = null;
    [SerializeField] WeaponConfig defaultWeapon = null;
    HealthPoints target;
    Equipment equipment;
    Mover mover;
    WeaponConfig currentWeaponConfig;
    LazyValue<Weapon> currentWeapon;
    float timeSinceLastAttack = Mathf.Infinity;

    private void Awake()
    {
      mover = GetComponent<Mover>();
      equipment = GetComponent<Equipment>();

      currentWeaponConfig = defaultWeapon;
      currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);

      if (equipment)
      {
        equipment.equipmentUpdated += UpdateWeapon;
      }
    }

    private Weapon SetupDefaultWeapon()
    {
      return AttachWeapon(defaultWeapon);
    }

    private void Start()
    {
      currentWeapon.ForceInit();
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

    private void UpdateWeapon()
    {
      var weaponInSlot = equipment.GetItemInSlot(EquipLocation.PrimaryHand);
      if (weaponInSlot)
      {
        EquipWeapon(weaponInSlot as WeaponConfig);
      }
      else
      {
        EquipWeapon(defaultWeapon);
      }
    }

    public void EquipWeapon(WeaponConfig weapon)
    {
      if (weapon == null) return;
      currentWeaponConfig = weapon;
      currentWeapon.value = AttachWeapon(weapon);
    }

    private Weapon AttachWeapon(WeaponConfig weapon)
    {
      Animator animator = GetComponent<Animator>();
      return weapon.Spawn(rightHandTransform, leftHandTransform, animator);
    }

    public HealthPoints GetTarget()
    {
      return target;
    }

    private void AttackBehavior()
    {
      transform.LookAt(target.transform.position);
      if (timeSinceLastAttack >= currentWeaponConfig.AttackDelay && !target.GetIsDead())
      {
        GetComponent<Animator>().ResetTrigger("cancelAttack");
        GetComponent<Animator>().SetTrigger("attack");
        timeSinceLastAttack = 0f;
      }
    }

    private bool GetIsInRange()
    {
      return Vector3.Distance(transform.position, target.transform.position) < currentWeaponConfig.WeaponRange;
    }

    ///// ### Animation Events ###
    private void Hit()
    {
      if (target == null) return;
      float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

      if (currentWeapon.value != null)
      {
        currentWeapon.value.OnHit();
      }

      if (currentWeaponConfig.HasProjectile())
      {
        currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
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
      // GetComponent<Mover>().Cancel();
    }

    ///// IJsonSaveable
    public JToken CaptureAsJToken()
    {
      return JToken.FromObject(currentWeapon.value.name);
    }

    ///// IJsonSaveable
    public void RestoreFromJToken(JToken state)
    {
      string weaponName = state.ToObject<string>();
      WeaponConfig weapon = UnityEngine.Resources.Load<WeaponConfig>(weaponName);
      EquipWeapon(weapon);
    }
  }
}
