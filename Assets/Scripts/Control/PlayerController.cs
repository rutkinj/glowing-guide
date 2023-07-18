using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using RPG.Movement;
using RPG.Combat;
using RPG.Inventories;
using RPG.Stats.ResourcePools;

namespace RPG.Control
{
  public class PlayerController : MonoBehaviour
  {
    HealthPoints healthPoints;
    Hotbar hotbar;

    [System.Serializable]
    struct CursorMapping
    {
      public CursorType type;
      public Texture2D texture;
      public Vector2 hotspot;
    }

    [SerializeField] CursorMapping[] cursorMappings = null;
    [SerializeField] float distToNavMeshTolerance = 1f;
    [SerializeField] float maxDistNavMeshPath = 40f;

    private CursorMapping cachedCursorMapping;

    private bool isDraggingUI = false;

    private void Awake()
    {
      healthPoints = GetComponent<HealthPoints>();
      hotbar = GetComponent<Hotbar>();
    }

    void Update()
    {
      CheckHotbarKeys();
      if (InteractWithUI()) return;
      if (healthPoints.GetIsDead())
      {
        SetCursor(CursorType.none);
        return;
      }
      if (InteractWithComponent()) return;
      if (InteractWithCombat()) return;
      if (InteractWithMovement()) return;
      SetCursor(CursorType.none);
    }

    private void CheckHotbarKeys()
    {
      if (Input.GetKeyDown(KeyCode.Alpha1))
      {
        hotbar.UseItem(0, gameObject);
      }
      if (Input.GetKeyDown(KeyCode.Alpha2))
      {
        hotbar.UseItem(1, gameObject);
      }
      if (Input.GetKeyDown(KeyCode.Alpha3))
      {
        hotbar.UseItem(2, gameObject);
      }
      if (Input.GetKeyDown(KeyCode.Alpha4))
      {
        hotbar.UseItem(3, gameObject);
      }
      if (Input.GetKeyDown(KeyCode.Alpha5))
      {
        hotbar.UseItem(4, gameObject);
      }
      if (Input.GetKeyDown(KeyCode.Alpha6))
      {
        hotbar.UseItem(5, gameObject);
      }

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

    private RaycastHit[] RaycastHitsSorted()
    {
      RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
      float[] distances = new float[hits.Length];
      for (int i = 0; i < hits.Length; i++)
      {
        distances[i] = hits[i].distance;
      }
      Array.Sort(distances, hits);

      return hits;
    }

    private bool InteractWithUI()
    {
      if (EventSystem.current.IsPointerOverGameObject())
      {
        SetCursor(CursorType.ui);
        if(Input.GetMouseButtonDown(0)){
          isDraggingUI = true;
        }
      }

      if(Input.GetMouseButtonUp(0)){
        isDraggingUI = false;
      }

      return isDraggingUI;
    }

    private bool InteractWithCombat()
    {
      RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
      foreach (RaycastHit hit in hits)
      {
        CombatTarget target = hit.transform.GetComponent<CombatTarget>();
      }
      return false;
    }

    private bool InteractWithMovement()
    {
      Vector3 target;
      bool hasHit = RaycastNavMesh(out target);

      if (hasHit)
      {
        if (Input.GetMouseButton(0))
        {
          GetComponent<Mover>().StartMoveAction(target);
        }
        SetCursor(CursorType.move);
        return true;
      }
      return false;
    }

    private bool RaycastNavMesh(out Vector3 target)
    {
      target = new Vector3();

      RaycastHit hit;
      bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
      if (!hasHit) return false;

      NavMeshHit hitSpot;
      bool isHitNavMesh = NavMesh.SamplePosition(hit.point, out hitSpot, distToNavMeshTolerance, NavMesh.AllAreas);
      if (!isHitNavMesh) return false;

      target = hitSpot.position;

      NavMeshPath path = new NavMeshPath();
      bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
      if (!hasPath) return false;
      if (path.status != NavMeshPathStatus.PathComplete) return false;
      if (GetPathLength(path) > maxDistNavMeshPath) return false;

      return true;
    }

    private float GetPathLength(NavMeshPath path)
    {
      float totalDistance = 0f;
      if (path.corners.Length < 2) return totalDistance;

      for (int i = 0; i < path.corners.Length - 1; i++)
      {
        totalDistance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
      }
      return totalDistance;
    }

    private static Ray GetMouseRay()
    {
      return Camera.main.ScreenPointToRay(Input.mousePosition);
    }

    private void SetCursor(CursorType type)
    {
      if (cachedCursorMapping.type == type) return;

      cachedCursorMapping = GetCursorMapping(type);
      Cursor.SetCursor(cachedCursorMapping.texture, cachedCursorMapping.hotspot, CursorMode.Auto);
    }

    private CursorMapping GetCursorMapping(CursorType type)
    {
      return cursorMappings[((int)type)];
    }
  }
}
