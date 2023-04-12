using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OrbitCamera : MonoBehaviour
{
  [SerializeField] Transform target;
  [SerializeField] float distance;
  [SerializeField, Min(0f)] float focusRadius = 1f;
  [SerializeField, Range(0f, 0.99f)] float centeringFactor = 0.5f;
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
    Vector3 targetPosCurrent = target.position;
    if (focusRadius > 0f)
    {
      float dist = Vector3.Distance(targetPosCurrent, camFocus);
      float lerpTime = 1f;
        if(dist > 0.01f && centeringFactor > 0f){
            lerpTime = Mathf.Pow(1f - centeringFactor, Time.unscaledDeltaTime);
        }

      if (dist > focusRadius)
      {
        lerpTime = Mathf.Min(lerpTime, focusRadius / dist);
      }
        camFocus = Vector3.Lerp(targetPosCurrent, camFocus, lerpTime);
    }
    else camFocus = targetPosCurrent;
  }
}
