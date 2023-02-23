using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
  public class HealthPoints : MonoBehaviour
  {
    float currentHealth;
    float maxHealth;
    bool isDead = false;

    private void Start()
    {
      maxHealth = GetComponent<BaseStats>().GetHealth();
      currentHealth = maxHealth;
    }
    public void TakeDamage(GameObject instigator, float damage)
    {
      currentHealth -= damage;
      if (currentHealth <= 0 && !isDead)
      {
        GiveExp(instigator);
        DeathBehavior();
      }
      print("hp: " + currentHealth);
    }

    private void GiveExp(GameObject instigator)
    {
      Experience experience = instigator.GetComponent<Experience>();
      float expAmount = GetComponent<BaseStats>().GetExperienceReward();
      if (experience != null)
      {
        experience.GainExperience(expAmount);
      }
    }

    public float GetHPPercentage()
    {
      return (currentHealth / maxHealth) * 100;
    }

    private void DeathBehavior()
    {
      GetComponent<Animator>().SetTrigger("die");
      GetComponent<ActionScheduler>().CancelCurrentAction();
      isDead = true;
      currentHealth = 0;
    }

    public bool GetIsDead()
    {
      return isDead;
    }
  }
}
