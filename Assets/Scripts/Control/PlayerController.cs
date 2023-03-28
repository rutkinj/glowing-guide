using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;
using System;

namespace RPG.Control
{
  public class PlayerController : MonoBehaviour
  {
    HealthPoints healthPoints;

    enum CursorType
    {
      none,
      move,
      combat
    }

    [System.Serializable]
    struct CursorMapping
    {
      public CursorType type;
      public Texture2D texture;
      public Vector2 hotspot;

    }
    [SerializeField] CursorMapping[] cursorMappings = null;

    private void Awake()
    {
      healthPoints = GetComponent<HealthPoints>();
    }

    void Update()
    {
      if (InteractWithUI()) return;
      if (healthPoints.GetIsDead())
      {
        SetCursor(CursorType.none);
        return;
      }
      if (InteractWithCombat()) return;
      if (InteractWithMovement()) return;
      SetCursor(CursorType.none);
    }

    private bool InteractWithUI()
    {
      SetCursor(CursorType.none);
      return EventSystem.current.IsPointerOverGameObject();
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
          SetCursor(CursorType.combat);
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
          SetCursor(CursorType.move);
          return true;
        }
      }
      return false;
    }

    private static Ray GetMouseRay()
    {
      return Camera.main.ScreenPointToRay(Input.mousePosition);
    }

    private void SetCursor(CursorType type)
    {
      CursorMapping mapping = GetCursorMapping(type);
      Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
    }

    private CursorMapping GetCursorMapping(CursorType type)
    {
      return cursorMappings[((int)type)];
    }
  }
}
