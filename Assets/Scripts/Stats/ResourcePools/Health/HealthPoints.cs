
using UnityEngine;
using UnityEngine.Events;
using RPG.Core;
using RPG.Utils;
using RPG.Saving;
using Newtonsoft.Json.Linq;

namespace RPG.Stats.ResourcePools
{
  public class HealthPoints : MonoBehaviour, IJsonSaveable
  {
    [SerializeField] UnityEvent<float> eTakeDamage;
    [SerializeField] UnityEvent eDie;
    BaseStats baseStats;
    LazyValue<float> currentHealth;
    LazyValue<float> maxHealthCached;
    bool isDead = false;

    private void Awake()
    {
      baseStats = GetComponent<BaseStats>();
      currentHealth = new LazyValue<float>(GetMaxHealth);
      maxHealthCached = new LazyValue<float>(GetMaxHealth);
    }

    private void Start()
    {
      currentHealth.ForceInit();
      maxHealthCached.ForceInit();
    }

    private void OnEnable()
    {
      baseStats.onLevelUp += CalcHealthOnLevelUp;
    }

    private void OnDisable()
    {
      baseStats.onLevelUp -= CalcHealthOnLevelUp;
    }

    private float GetMaxHealth()
    {
      return baseStats.GetStat(Stat.Health);
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
      // maxHealthCached.value = GetMaxHealth();
      // currentHealth = GetMaxHealth();

      // //// maintain current % //// WONT WORK IN CURRENT STATE
      // float currentHpPercent = GetHPPercentage()/100;
      // maxHealth.value = baseStats.GetStat(Stat.Health);
      // currentHealth.value = maxHealth.value * currentHpPercent;

      //// add increase to current hp ////
      float newMax = GetMaxHealth();
      float maxHpDiff = newMax - maxHealthCached.value;
      if (maxHpDiff > 0) // bugfix for loading a save where your level is lower
      {
        currentHealth.value += maxHpDiff;
      }
      maxHealthCached.value = newMax;
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
      maxHealthCached.value = GetMaxHealth();
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
