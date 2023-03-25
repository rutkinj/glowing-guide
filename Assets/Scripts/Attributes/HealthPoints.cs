using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Stats;
using RPG.Saving;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System;

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

    public void GainHealth(float hpGain)
    {
      currentHealth += hpGain;
      if (currentHealth > maxHealth)
      {
        currentHealth = maxHealth;
      }
    }
    public void LoseHealth(GameObject instigator, float damage)
    {
      print(gameObject.name + " took damage: " + damage);
      if (gameObject.tag != "PunchingBag") currentHealth -= damage;
      if (currentHealth <= 0 && !isDead)
      {
        GiveExp(instigator);
        DeathBehavior();
      }
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
    public void CalcHealthOnLevelUp()
    {
      // //// full heal ////
      // maxHealth = baseStats.GetStat(Stat.Health);
      // currentHealth = maxHealth;

      // //// maintain current % ////
      // float currentHpPercent = GetHPPercentage()/100;
      // maxHealth = baseStats.GetStat(Stat.Health);
      // currentHealth = maxHealth * currentHpPercent;

      //// add increase to current hp ////
      float newMax = baseStats.GetStat(Stat.Health);
      float maxHpDiff = newMax - maxHealth;
      maxHealth = newMax;
      currentHealth += maxHpDiff;
    }
    public float GetHPPercentage()
    {
      return (currentHealth / maxHealth) * 100;
    }

    public string CurrentHealthAsString()
    {
      return currentHealth.ToString() + " / " + maxHealth.ToString();
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
      if (currentHealth <= 0)
      {
        DeathBehavior();
      }
      else{
        Revive();
      }
    }

    private void Revive()
    {
      isDead = false;
      GetComponent<Animator>().SetTrigger("revive");
    }
  }
}
