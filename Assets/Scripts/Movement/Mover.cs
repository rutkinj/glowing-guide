using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Stats.ResourcePools;
using RPG.Saving;

namespace RPG.Movement
{
  public class Mover : MonoBehaviour, IAction, IJsonSaveable
  {
    [SerializeField] Transform target;
    [SerializeField] float maxSpeed = 3f;
    NavMeshAgent navMeshAgent;
    HealthPoints healthPoints;

    private void Awake()
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

    private void AnimationControl()
    {
      Vector3 velocity = navMeshAgent.velocity;
      Vector3 localVelocity = transform.InverseTransformDirection(velocity);
      float speed = localVelocity.magnitude;
      GetComponent<Animator>().SetFloat("forwardSpeed", speed);
    }

    public void Cancel()
    {
      navMeshAgent.isStopped = true;
    }

    public JToken CaptureAsJToken()
    {
      return transform.position.ToToken();
    }

    public void RestoreFromJToken(JToken state)
    {
      navMeshAgent.enabled = false;
      transform.position = state.ToVector3();
      navMeshAgent.enabled = true;
      GetComponent<ActionScheduler>().CancelCurrentAction();
    }
  }
}
