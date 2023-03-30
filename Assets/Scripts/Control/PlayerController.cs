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

    [System.Serializable]
    struct CursorMapping
    {
      public CursorType type;
      public Texture2D texture;
      public Vector2 hotspot;

    }
    [SerializeField] CursorMapping[] cursorMappings = null;

    private CursorMapping cachedCursorMapping;

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
      if (InteractWithComponent()) return;
      // if (InteractWithCombat()) return;
      if (InteractWithMovement()) return;
      SetCursor(CursorType.none);
    }

    private bool InteractWithComponent()
    {
      RaycastHit[] hits = RaycastHitsSorted();
      foreach (RaycastHit hit in hits)
      {
        IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();

        foreach (IRaycastable raycastable in raycastables)
        {
          if (raycastable.HandleRaycast(this))
          {
            SetCursor(raycastable.GetCursorType());
            return true;
          }
        }
      }
      return false;
    }

    RaycastHit[] RaycastHitsSorted(){
      RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
      float[] distances = new float[hits.Length];
      for(int i = 0; i < hits.Length; i++){
        distances[i] = hits[i].distance;
      }
      Array.Sort(distances, hits);

      return hits;
    }

    private bool InteractWithUI()
    {
      SetCursor(CursorType.none);
      return EventSystem.current.IsPointerOverGameObject();
    }

    // private bool InteractWithCombat()
    // {
    //   RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
    //   foreach (RaycastHit hit in hits)
    //   {
    //     CombatTarget target = hit.transform.GetComponent<CombatTarget>();
        
    //   }
    //   return false;
    // }

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
      if(cachedCursorMapping.type == type) return;

      cachedCursorMapping = GetCursorMapping(type);
      Cursor.SetCursor(cachedCursorMapping.texture, cachedCursorMapping.hotspot, CursorMode.Auto);
    }

    private CursorMapping GetCursorMapping(CursorType type)
    {
      return cursorMappings[((int)type)];
    }
  }
}
