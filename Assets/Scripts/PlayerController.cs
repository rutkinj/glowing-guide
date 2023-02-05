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
      InteractWithCombat();
      InteractWithMovement();
    }

    private void InteractWithCombat()
    {
      if (Input.GetMouseButtonDown(0))
      {

        RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
        foreach (RaycastHit hit in hits)
        {
          if (hit.transform.GetComponent<CombatTarget>())
          {
            GetComponent<Fighter>().Attack();
          }
        }
      }
    }

    private void InteractWithMovement()
    {
      if (Input.GetMouseButton(0))
      {
        MoveToCursor();
      }
    }

    public void MoveToCursor()
    {
      RaycastHit hit;
      bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
      if (hasHit)
      {
        GetComponent<Mover>().MoveTo(hit.point);
      }
    }

    private static Ray GetMouseRay()
    {
      return Camera.main.ScreenPointToRay(Input.mousePosition);
    }
  }
}
