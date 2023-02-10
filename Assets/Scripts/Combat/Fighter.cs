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
    float timeSinceLastAttack = Mathf.Infinity;

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
      transform.LookAt(target.transform.position);
      if (timeSinceLastAttack >= attackDelay && !target.GetIsDead())
      {
        GetComponent<Animator>().ResetTrigger("cancelAttack");
        GetComponent<Animator>().SetTrigger("attack"); //And triggers Hit() found below
        timeSinceLastAttack = 0f;
      }
    }

    //Animation Event
    private void Hit()
    {
      if(target == null) return;
      target.TakeDamage(weaponDamage);
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
      return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
    }

    public bool CanAttack(GameObject combatTarget){
      if(combatTarget == null) return false;
      HealthPoints testTarget = combatTarget.GetComponent<HealthPoints>();
      return testTarget != null && !testTarget.GetIsDead();
    }
  }
}
