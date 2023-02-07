using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
  public class HealthPoints : MonoBehaviour
  {
    [SerializeField] float health = 100f;

    public void TakeDamage(float damage){
        if(health == 0) return;
        health -= damage;
        if (health <= 0){
            health = 0;
        }
        print("hp: " + health);
    }
  }
}
