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
    Transform target;
    float timeSinceLastAttack = 0f;
    private void Update()
    {
      timeSinceLastAttack += Time.deltaTime;

      if (target == null) return;

      if (target != null && !GetIsInRange())
      {
        GetComponent<Mover>().MoveTo(target.position);
      }
      else
      {
        GetComponent<Mover>().Cancel();
        AttackBehavior();
      }
    }

    private void AttackBehavior()
    {
      if (timeSinceLastAttack >= attackDelay)
      {
        GetComponent<Animator>().SetTrigger("attack");
        timeSinceLastAttack = 0f;
      }
    }

    public void Attack(CombatTarget combatTarget)
    {
      GetComponent<ActionScheduler>().StartAction(this);
      target = combatTarget.transform;
    }
    public void Cancel()
    {
      target = null;
    }
    private bool GetIsInRange()
    {
      return Vector3.Distance(transform.position, target.position) < weaponRange;
    }
    // Animation Event
    private void Hit()
    {

    }
  }
}
