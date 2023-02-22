using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
  public class HealthPoints : MonoBehaviour
  {
    [SerializeField] float health = 100f;
    bool isDead = false;

    private void Start()
    {
      health = GetComponent<BaseStats>().GetHealth();
    }
    public void TakeDamage(float damage)
    {
      health -= damage;
      if (health <= 0 && !isDead)
      {
        DeathBehavior();
      }
      print("hp: " + health);
    }

    private void DeathBehavior()
    {
      GetComponent<Animator>().SetTrigger("die");
      GetComponent<ActionScheduler>().CancelCurrentAction();
      isDead = true;
      health = 0;
    }

    public bool GetIsDead()
    {
      return isDead;
    }
  }
}
