using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using System;

namespace RPG.Control
{
  public class PlayerController : MonoBehaviour
  {
    void Update()
    {
      if (InteractWithCombat()) return;
      if (InteractWithMovement()) return;
      print("nothin");
    }

    private bool InteractWithCombat()
    {
      RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
      foreach (RaycastHit hit in hits)
      {
        if (hit.transform.GetComponent<CombatTarget>() && !hit.transform.GetComponent<HealthPoints>().GetIsDead())
        {
          if (Input.GetMouseButtonDown(0))
          {
            GetComponent<Fighter>().Attack(hit.transform.GetComponent<CombatTarget>());
          }
          return true;
        }
      }
      return false;
    }

    private bool InteractWithMovement()
    {
      RaycastHit hit;
      bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
      if (hasHit)
      {
        if (Input.GetMouseButton(0))
        {
          GetComponent<Mover>().StartMoveAction(hit.point);
        }
        return true;
      }
      return false;
    }

    private static Ray GetMouseRay()
    {
      return Camera.main.ScreenPointToRay(Input.mousePosition);
    }
  }
}
