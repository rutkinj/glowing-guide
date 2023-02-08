using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Movement;

namespace RPG.Combat
{
  public class Fighter : MonoBehaviour, IAction
  {
    [SerializeField] float weaponRange = 2f;
    [SerializeField] float attackDelay = 1f;
    [SerializeField] float weaponDamage = 12f;
    HealthPoints target;
    float timeSinceLastAttack = 0f;

    private void Update()
    {
      timeSinceLastAttack += Time.deltaTime;

      if (target == null) return;

      if (target != null && !GetIsInRange())
      {
        GetComponent<Mover>().MoveTo(target.transform.position);
      }
      else
      {
        GetComponent<Mover>().Cancel();
        AttackBehavior();
      }
    }

    private void AttackBehavior()
    {
      if (timeSinceLastAttack >= attackDelay && !target.GetIsDead())
      {
        GetComponent<Animator>().SetTrigger("attack"); //And triggers Hit() found below
        timeSinceLastAttack = 0f;
      }
    }

    //Animation Event
    private void Hit()
    {
      target.TakeDamage(weaponDamage);
    }

    public void Attack(CombatTarget combatTarget)
    {
      GetComponent<ActionScheduler>().StartAction(this);
      target = combatTarget.GetComponent<HealthPoints>();
    }

    public void Cancel()
    {
      GetComponent<Animator>().SetTrigger("cancelAttack");
      target = null;
    }

    private bool GetIsInRange()
    {
      return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
    }
  }
}
