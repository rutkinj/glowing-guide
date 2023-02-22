using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Attributes;

namespace RPG.Movement
{
  public class Mover : MonoBehaviour, IAction
  {
    [SerializeField] Transform target;
    [SerializeField] float maxSpeed = 3f;
    NavMeshAgent navMeshAgent;
    HealthPoints healthPoints;

    private void Start()
    {
      navMeshAgent = GetComponent<NavMeshAgent>();
      healthPoints = GetComponent<HealthPoints>();
    }
    void Update()
    {
      navMeshAgent.enabled = !healthPoints.GetIsDead();
      AnimationControl();
    }


    public void StartMoveAction(Vector3 destination, float speedFraction = 1f)
    {
      GetComponent<ActionScheduler>().StartAction(this);
      GetComponent<Animator>().SetTrigger("cancelAttack");
      MoveTo(destination, speedFraction);
    }
    public void MoveTo(Vector3 destination, float speedFraction = 1f)
    {
      navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
      navMeshAgent.destination = destination;
      navMeshAgent.isStopped = false;
    }
    public void Cancel()
    {
      navMeshAgent.isStopped = true;
    }
    private void AnimationControl()
    {
      Vector3 velocity = navMeshAgent.velocity;
      Vector3 localVelocity = transform.InverseTransformDirection(velocity);
      float speed = localVelocity.z;
      GetComponent<Animator>().SetFloat("forwardSpeed", speed);
    }
  }
}
