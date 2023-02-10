using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{
  public class AIController : MonoBehaviour
  {
    [SerializeField] float chaseDistance = 5f;

    Fighter fighter;
    HealthPoints healthPoints;
    GameObject player;

    private void Start()
    {
      fighter = GetComponent<Fighter>();
      healthPoints = GetComponent<HealthPoints>();
      player = GameObject.FindWithTag("Player");
    }


    private void Update()
    {
      if (healthPoints.GetIsDead()) return;
      if (InAttackRange(player) && fighter.CanAttack(player))
      {
        //give chase
        print(this + "shoudl give chase!");
        fighter.Attack(player);
      }
      else
      {
        fighter.Cancel();
      }
    }

    private bool InAttackRange(GameObject player)
    {
      return Vector3.Distance(transform.position, player.transform.position) < chaseDistance;
    }
  }
}

