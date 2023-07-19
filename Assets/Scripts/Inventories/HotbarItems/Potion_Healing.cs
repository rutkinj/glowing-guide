using RPG.Stats.ResourcePools;
using UnityEngine;

namespace RPG.Inventories
{
  public class Potion_Healing : HotbarItem
  {
    [SerializeField] float healAmount = 0f;

    public override void Use(GameObject player){
        player.GetComponent<HealthPoints>().GainHealth(healAmount);
    }
  }
}
