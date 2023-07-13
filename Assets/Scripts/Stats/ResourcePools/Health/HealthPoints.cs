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

namespace RPG.Stats.ResourcePools
{
  public class HealthPoints : MonoBehaviour, IJsonSaveable
  {
    [SerializeField] UnityEvent<float> eTakeDamage;
    [SerializeField] UnityEvent eDie;
    BaseStats baseStats;
    LazyValue<float> currentHealth;
    // LazyValue<float> maxHealth;
    bool isDead = false;

    private void Awake()
    {
      baseStats = GetComponent<BaseStats>();
      currentHealth = new LazyValue<float>(GetMaxHealth);
      // maxHealth = new LazyValue<float>(GetMaxHealth);
      // currentHealth = new LazyValue<float>(SetCurrentHealthToMax);
    }

    private void Start() {
      // maxHealth.ForceInit();
      currentHealth.ForceInit();
    }

    private float GetMaxHealth()
    {
      return baseStats.GetStat(Stat.Health);
    }

    // private float SetCurrentHealthToMax()
    // {
    //   return GetMaxHealth();
    // }

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
      if (currentHealth.value > GetMaxHealth())
      {
        currentHealth.value = GetMaxHealth();
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
      eTakeDamage.Invoke(damage);
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
      float maxHpDiff = newMax - GetMaxHealth();
      // maxHealth.value = newMax;
      if (maxHpDiff > 0) // bugfix for loading a save where your level is lower
      {
        currentHealth.value += maxHpDiff;
      }
    }
    public float GetHPPercentage()
    {
      return (currentHealth.value / GetMaxHealth()) * 100;
    }

    public string CurrentHealthAsString()
    {
      return currentHealth.value.ToString() + " / " + GetMaxHealth().ToString();
    }

    private void DeathBehavior()
    {
      GetComponent<Animator>().SetTrigger("die");
      GetComponent<ActionScheduler>().CancelCurrentAction();
      eDie.Invoke();
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
