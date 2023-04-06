using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Attributes;
using RPG.Movement;
using System;
using RPG.Utils;

namespace RPG.Control
{
  public class AIController : MonoBehaviour
  {
    [SerializeField] float chaseDistance = 5f;
    [SerializeField] float suspicionTime = 2f;
    [SerializeField] float aggroTimer = 2f;
    [SerializeField] float callAlliesDist = 100f;
    [SerializeField] PatrolPath patrolPath;
    [SerializeField] float waypointTolerance = 1f;
    [SerializeField] float patrolResttime = .5f;
    [Range(0f, 1f)][SerializeField] float patrolSpeedFraction = 0.5f;

    Fighter fighter;
    HealthPoints healthPoints;
    GameObject player;
    LazyValue<Vector3> guardPosition;
    int currentWaypointIndex = 0;
    float timeSinceSeenPlayer = Mathf.Infinity;
    float timeAtWaypoint = Mathf.Infinity;
    float timeSinceAggro = Mathf.Infinity;

    private void Awake()
    {
      fighter = GetComponent<Fighter>();
      healthPoints = GetComponent<HealthPoints>();
      player = GameObject.FindWithTag("Player");

      guardPosition = new LazyValue<Vector3>(GetGuardPosition);
    }

    private Vector3 GetGuardPosition()
    {
      return transform.position;
    }

    private void Start()
    {
      guardPosition.ForceInit();
    }

    private void Update()
    {
      if (healthPoints.GetIsDead()) return;
      ManageTimers();

      if (IsAggroed(player) && fighter.CanAttack(player))
      {
        timeSinceSeenPlayer = 0f;
        fighter.Attack(player);
        AggroNearbyAllies();
      }
      else if (timeSinceSeenPlayer < suspicionTime)
      {
        GetComponent<ActionScheduler>().CancelCurrentAction();
      }
      else
      {
        PatrolBehavior();
      }
    }

    private void ManageTimers()
    {
      timeSinceSeenPlayer += Time.deltaTime;
      timeAtWaypoint += Time.deltaTime;
      timeSinceAggro += Time.deltaTime;
      print(gameObject.name + " Aggro Time: " + timeSinceAggro);
    }

    public void Aggro()
    {
      if(IsAggroed(player)) return;
      timeSinceAggro = 0;
    }
    private void PatrolBehavior()
    {
      Vector3 nextPosition = guardPosition.value;

      if (patrolPath != null)
      {
        if (AtWaypoint())
        {
          CycleWaypoint();
          timeAtWaypoint = UnityEngine.Random.Range(0f, 2f);
        }
        nextPosition = GetCurrentWaypoint();
      }
      if (timeAtWaypoint > patrolResttime)
      {
        GetComponent<Mover>().StartMoveAction(nextPosition, patrolSpeedFraction);
      }
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

    private bool IsAggroed(GameObject player)
    {
      if (Vector3.Distance(transform.position, player.transform.position) < chaseDistance ||
          timeSinceAggro < aggroTimer)
      {
        return true;
      }
      return false;
    }

    private void AggroNearbyAllies(){
      RaycastHit[] hits = Physics.SphereCastAll(transform.position, callAlliesDist, Vector3.forward, 0);
      foreach(RaycastHit hit in hits){
        if (hit.collider.CompareTag("Enemy")){
          hit.collider.GetComponent<AIController>().Aggro();
        }
      }
    }

    private void OnDrawGizmosSelected()
    {
      Gizmos.color = Color.blue;
      Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
  }
}

