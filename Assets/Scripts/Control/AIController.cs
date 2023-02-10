using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;

namespace RPG.Control
{
  public class AIController : MonoBehaviour
  {
    [SerializeField] float chaseDistance = 5f;
    [SerializeField] float suspicionTime = 2f;

    Fighter fighter;
    HealthPoints healthPoints;
    GameObject player;

    Vector3 guardPosition;
    float timeSinceSeenPlayer = Mathf.Infinity;

    private void Start()
    {
      fighter = GetComponent<Fighter>();
      healthPoints = GetComponent<HealthPoints>();
      player = GameObject.FindWithTag("Player");
      guardPosition = transform.position;
    }


    private void Update()
    {
      if (healthPoints.GetIsDead()) return;
      if (InAttackRange(player) && fighter.CanAttack(player))
      {
        //give chase
        timeSinceSeenPlayer = 0f;
        fighter.Attack(player);
      }
      else if (timeSinceSeenPlayer < suspicionTime)
      {
        GetComponent<ActionScheduler>().CancelCurrentAction();
      }
      else
      {
        GetComponent<Mover>().StartMoveAction(guardPosition);
      }

      timeSinceSeenPlayer += Time.deltaTime;
    }

    private bool InAttackRange(GameObject player)
    {
      return Vector3.Distance(transform.position, player.transform.position) < chaseDistance;
    }

    private void OnDrawGizmosSelected()
    {
      Gizmos.color = Color.blue;
      Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
  }
}

