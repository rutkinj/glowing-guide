using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;

namespace RPG.Combat
{
  public class Fighter : MonoBehaviour
  {
    [SerializeField] float weaponRange = 2f;
    Transform target;
    private void Update() {
        if (target != null){
            GetComponent<Mover>().MoveTo(target.position - new Vector3(0,0,weaponRange));
        }
    }
    public void Attack(CombatTarget combatTarget)
    {
      target = combatTarget.transform;
    }
  }
}
