using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;

namespace RPG.Control
{
  public class PlayerController : MonoBehaviour
  {
    HealthPoints healthPoints;

    private void Start()
    {
      healthPoints = GetComponent<HealthPoints>();
    }
    void Update()
    {
      if (healthPoints.GetIsDead()) return;
      if (InteractWithCombat()) return;
      if (InteractWithMovement()) return;
    }

    private bool InteractWithCombat()
    {
      RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
      foreach (RaycastHit hit in hits)
      {
        CombatTarget target = hit.transform.GetComponent<CombatTarget>();
        if (target != null && !hit.transform.GetComponent<HealthPoints>().GetIsDead())
        {
          if (Input.GetMouseButtonDown(0))
          {
            GetComponent<Fighter>().Attack(target.gameObject);
          }
          return true;
        }
      }
      return false;
    }

    private bool InteractWithMovement()
    {
      RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
      foreach (RaycastHit hit in hits)
      {
        if (hit.transform.CompareTag("Ground"))
        {
          if (Input.GetMouseButton(0))
          {
            GetComponent<Mover>().StartMoveAction(hit.point);
          }
          return true;
        }
      }
      return false;
    }

    private static Ray GetMouseRay()
    {
      return Camera.main.ScreenPointToRay(Input.mousePosition);
    }
  }
}
