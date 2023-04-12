using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OrbitCamera : MonoBehaviour
{
  [SerializeField] Transform target;
  [SerializeField] float distance;
  [SerializeField, Min(0f)] float focusRadius = 1f;
  Vector3 camFocus;

  private void Awake()
  {
    camFocus = target.position;
  }

  private void LateUpdate()
  {
    UpdateTargetPos();
    Vector3 lookDir = transform.forward;
    transform.localPosition = camFocus - lookDir * distance;
  }

  private void UpdateTargetPos()
  {
    Vector3 targetPos = target.position;
    if (focusRadius > 0f)
    {
      float dist = Vector3.Distance(targetPos, camFocus);
      if (dist > focusRadius)
      {
        camFocus = Vector3.Lerp(targetPos, camFocus, focusRadius / dist);
      }
    }
    else camFocus = targetPos;
  }
}
