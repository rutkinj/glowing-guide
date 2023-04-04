using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Stats;
using RPG.Saving;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System;
using RPG.Utils;
using UnityEngine.Events;

namespace RPG.Attributes
{
  public class HealthPoints : MonoBehaviour, IJsonSaveable
  {
    [SerializeField] UnityEvent takeDamage;
    BaseStats baseStats;
    LazyValue<float> currentHealth;
    LazyValue<float> maxHealth;
    bool isDead = false;

    private void Awake()
    {
      baseStats = GetComponent<BaseStats>();
      maxHealth = new LazyValue<float>(GetMaxHealth);
      currentHealth = new LazyValue<float>(SetCurrentHealthToMax);
    }

    private void Start() {
      maxHealth.ForceInit();
      currentHealth.ForceInit();
    }

    private float GetMaxHealth()
    {
      return baseStats.GetStat(Stat.Health);
    }

    private float SetCurrentHealthToMax()
    {
      return maxHealth.value;
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
      if (currentHealth.value > maxHealth.value)
      {
        currentHealth.value = maxHealth.value;
      }
    }
    public void LoseHealth(GameObject instigator, float damage)
    {
      print(gameObject.name + " took damage: " + damage);
      takeDamage.Invoke();
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
      // maxHealth.value = baseStats.GetStat(Stat.Health);
      // currentHealth = maxHealth.value;

      // //// maintain current % ////
      // float currentHpPercent = GetHPPercentage()/100;
      // maxHealth.value = baseStats.GetStat(Stat.Health);
      // currentHealth = maxHealth.value * currentHpPercent;

      //// add increase to current hp ////
      float newMax = baseStats.GetStat(Stat.Health);
      float maxHpDiff = newMax - maxHealth.value;
      maxHealth.value = newMax;
      if (maxHpDiff > 0) // bugfix for loading a save where your level is lower
      {
        currentHealth.value += maxHpDiff;
      }
    }
    public float GetHPPercentage()
    {
      return (currentHealth.value / maxHealth.value) * 100;
    }

    public string CurrentHealthAsString()
    {
      return currentHealth.value.ToString() + " / " + maxHealth.value.ToString();
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
