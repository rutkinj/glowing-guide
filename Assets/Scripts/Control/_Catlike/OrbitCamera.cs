using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OrbitCamera : MonoBehaviour
{
  [SerializeField] Transform target;
  [SerializeField] float distance;

  [Header("Orbit")]
  [SerializeField, Range(1f, 360f)] float rotationSpeed = 90f;

  [Header("Focus")]
  [SerializeField, Min(0f)] float focusRadius = 1f;
  [SerializeField, Range(0f, 0.99f)] float centeringFactor = 0.5f;

  Vector3 camFocus;
  Vector2 orbitAngles = new Vector2(45f, 0f);

  private void Awake()
  {
    camFocus = target.position;
  }

  private void LateUpdate()
  {
    UpdateTargetPos();
    ManualRotation();

    Quaternion lookRotation = Quaternion.Euler(orbitAngles);
    Vector3 lookDirection = lookRotation * Vector3.forward;
    Vector3 lookPosisition = camFocus - lookDirection * distance;
    transform.SetPositionAndRotation(lookPosisition, lookRotation);
  }

  private void UpdateTargetPos()
  {
    Vector3 targetPosCurrent = target.position;
    if (focusRadius > 0f)
    {
      float dist = Vector3.Distance(targetPosCurrent, camFocus);
      float lerpTime = 1f;
      if (dist > 0.01f && centeringFactor > 0f)
      {
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

  private void ManualRotation()
  {
    Vector2 input = new Vector2(Input.GetAxis("Vertical Camera"), Input.GetAxis("Horizontal Camera"));
    const float deadZone = 0.001f;
    if (input.x < -deadZone || input.x > deadZone || input.y < -deadZone || input.y > deadZone)
    {
      orbitAngles += rotationSpeed * Time.unscaledDeltaTime * input;
    }
  }
}
