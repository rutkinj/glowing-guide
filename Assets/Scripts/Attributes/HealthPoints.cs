using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Stats;
using RPG.Saving;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System;
using RPG.Utils;

namespace RPG.Attributes
{
  public class HealthPoints : MonoBehaviour, IJsonSaveable
  {
    BaseStats baseStats;
    LazyValue<float> currentHealth;
    float maxHealth;
    bool isDead = false;

    private void Awake()
    {
      baseStats = GetComponent<BaseStats>();
      currentHealth = new LazyValue<float>(GetInitialHealth);
      maxHealth = currentHealth.value;
    }

    private float GetInitialHealth()
    {
      return baseStats.GetStat(Stat.Health);
    }

    private void OnEnable()
    {
      baseStats.onLevelUp += CalcHealthOnLevelUp;
    }

    private void OnDisable()
    {
      baseStats.onLevelUp -= CalcHealthOnLevelUp;
    }

    public void GainHealth(float hpGain)
    {
      currentHealth.value += hpGain;
      if (currentHealth.value > maxHealth)
      {
        currentHealth.value = maxHealth;
      }
    }
    public void LoseHealth(GameObject instigator, float damage)
    {
      print(gameObject.name + " took damage: " + damage);
      if (gameObject.tag != "PunchingBag") currentHealth.value -= damage;
      if (currentHealth.value <= 0 && !isDead)
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
      if (maxHpDiff > 0) // bugfix for loading a save where your level is lower
      {
        currentHealth.value += maxHpDiff;
      }
    }
    public float GetHPPercentage()
    {
      return (currentHealth.value / maxHealth) * 100;
    }

    public string CurrentHealthAsString()
    {
      return currentHealth.value.ToString() + " / " + maxHealth.ToString();
    }

    private void DeathBehavior()
    {
      GetComponent<Animator>().SetTrigger("die");
      GetComponent<ActionScheduler>().CancelCurrentAction();
      isDead = true;
      currentHealth.value = 0;
    }

    public bool GetIsDead()
    {
      return isDead;
    }

    public JToken CaptureAsJToken()
    {
      return JToken.FromObject(currentHealth.value);
    }

    public void RestoreFromJToken(JToken state)
    {
      currentHealth.value = state.ToObject<float>();
      if (currentHealth.value <= 0)
      {
        DeathBehavior();
      }
      else
      {
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
