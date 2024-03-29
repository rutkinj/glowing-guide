using UnityEngine;
using RPG.Stats.ResourcePools;
using RPG.Control;

namespace RPG.Combat
{
  [RequireComponent(typeof(HealthPoints))]
  public class CombatTarget : MonoBehaviour, IRaycastable
  {
    public CursorType GetCursorType()
    {
      return CursorType.combat;
    }

    public bool HandleRaycast(PlayerController callingController)
    {

      if (callingController.transform.GetComponent<Fighter>().CanAttack(gameObject))
      {
        if (Input.GetMouseButtonDown(0))
        {
          callingController.GetComponent<Fighter>().Attack(gameObject);
        }
        return true;
      }
      return false;
    }
  }
}
