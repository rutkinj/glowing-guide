using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;

namespace RPG.Control
{
  public class AIController : MonoBehaviour
  {
    [SerializeField] float chaseDistance = 5f;
    [SerializeField] float suspicionTime = 2f;
    [SerializeField] PatrolPath patrolPath;
    [SerializeField] float waypointTolerance = 1f;

    Fighter fighter;
    HealthPoints healthPoints;
    GameObject player;
    Vector3 guardPosition;
    int currentWaypointIndex = 0;
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
        PatrolBehavior();
      }

      timeSinceSeenPlayer += Time.deltaTime;
    }

    private void PatrolBehavior()
    {
      Vector3 nextPosition = guardPosition;

      if (patrolPath != null)
      {
        if (AtWaypoint())
        {
          CycleWaypoint();
        }
        nextPosition = GetCurrentWaypoint();
      }

      GetComponent<Mover>().StartMoveAction(nextPosition);
    }

    private Vector3 GetCurrentWaypoint()
    {
      return patrolPath.GetWaypoint(currentWaypointIndex);
    }

    private void CycleWaypoint()
    {
      currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
    }

    private bool AtWaypoint()
    {
      float distance = Vector3.Distance(transform.position, GetCurrentWaypoint());
      return distance < waypointTolerance;
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

