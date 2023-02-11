using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
  public class PatrolPath : MonoBehaviour
  {
    private void OnDrawGizmos()
    {
      for (int i = 0; i < transform.childCount; i++)
      {
        Gizmos.color = Color.magenta;
        if (i == 0) Gizmos.color = Color.green;
        Gizmos.DrawSphere(GetWaypoint(i), .2f);
        Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(GetNextIndex(i)));
      }
    }

    private int GetNextIndex(int i)
    {
        if (i + 1 == transform.childCount){
            return 0;
        }
      return i + 1;
    }

    private Vector3 GetWaypoint(int i)
    {
      return transform.GetChild(i).transform.position;
    }
  }
}
