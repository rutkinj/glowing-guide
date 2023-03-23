using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Stats;
using RPG.Saving;
using UnityEngine;
using Newtonsoft.Json.Linq;

namespace RPG.Attributes
{
  public class HealthPoints : MonoBehaviour, IJsonSaveable
  {
    BaseStats baseStats;
    float currentHealth = -999;
    float maxHealth;
    bool isDead = false;

    private void Start()
    {
      baseStats = GetComponent<BaseStats>();
      baseStats.onLevelUp += CalcHealthOnLevelUp;

      maxHealth = baseStats.GetStat(Stat.Health);
      if (currentHealth < 0)
      {
        currentHealth = maxHealth;
      }
    }

    public void GainHealth(float hpGain){
      currentHealth += hpGain;
      if(currentHealth > maxHealth){
        currentHealth = maxHealth;
      }
    }
    public void LoseHealth(GameObject instigator, float damage)
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
      if (experience != null)
      {
        float expAmount = GetComponent<BaseStats>().GetStat(Stat.ExperienceReward);
        experience.GainExperience(expAmount);
      }
    }
    public void CalcHealthOnLevelUp(){
      float currentHpPercent = GetHPPercentage()/100;
      maxHealth = baseStats.GetStat(Stat.Health);
      currentHealth = maxHealth * currentHpPercent;
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

    public JToken CaptureAsJToken()
    {
      return JToken.FromObject(currentHealth);
    }

    public void RestoreFromJToken(JToken state)
    {
      currentHealth = state.ToObject<float>();
      if(currentHealth == 0){
        DeathBehavior();
      }
    }
  }
}
