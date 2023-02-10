using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
  public class HealthPoints : MonoBehaviour
  {
    [SerializeField] float health = 100f;
    bool isDead = false;

    public void TakeDamage(float damage){
        health -= damage;
        if (health <= 0 && !isDead){
            DeathBehavior();
        }
        print("hp: " + health);
    }

    private void DeathBehavior(){
      GetComponent<Animator>().SetTrigger("die");
      isDead = true;
      health = 0;
    }

    public bool GetIsDead(){
      return isDead;
    }
  }
}
