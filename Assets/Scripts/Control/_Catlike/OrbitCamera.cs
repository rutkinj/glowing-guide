using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class OrbitCamera : MonoBehaviour
{
  [SerializeField] Transform target;
  [SerializeField] float distance;

  [Header("Orbit")]
  [SerializeField, Range(1f, 360f)] float rotationSpeed = 90f;
  [SerializeField, Range(-89f, 89f)] float minVertAngle = -30f;
  [SerializeField, Range(-89f, 89f)] float maxVertAngle = 60f;
  [SerializeField, Range(0f, 90f)] float alignSmoothRange = 45f;
  [SerializeField, Min(0f)] float alignDelay = 2f;

  [Header("Focus")]
  [SerializeField, Min(0f)] float focusRadius = 1f;
  [SerializeField, Range(0f, 0.99f)] float centeringFactor = 0.5f;

  PlayerInput playerInput;
  InputAction lookAction;

  Camera cam;
  Vector3 camFocus;
  Vector3 previousCamFocus;
  Vector2 orbitAngles = new Vector2(45f, 0f);
  float lastManualRotationTime;

  private void OnValidate()
  {
    if (maxVertAngle < minVertAngle)
    {
      maxVertAngle = minVertAngle;
    }
  }
  private void Awake()
  {
    cam = GetComponent<Camera>();
    camFocus = target.position;
    transform.localRotation = Quaternion.Euler(orbitAngles);
    playerInput = target.GetComponent<PlayerInput>();
    lookAction = playerInput.actions["Look"];
  }

  private void LateUpdate()
  {
    UpdateTargetPos();
    Quaternion lookRotation;
    if (ManualRotation() || AutoRotate())
    {
      ConstrainAngles();
      lookRotation = Quaternion.Euler(orbitAngles);
    }
    else
    {
      lookRotation = transform.localRotation;
    }

    Vector3 lookDirection = lookRotation * Vector3.forward;
    Vector3 lookPosition = camFocus - lookDirection * distance;

    Vector3 rectOffset = lookDirection * cam.nearClipPlane;
    Vector3 rectPosition = lookPosition + rectOffset;
    Vector3 castFrom = target.position;
    Vector3 castLine = rectPosition - castFrom;
    float castDistance = castLine.magnitude;
    Vector3 castDirection = castLine / castDistance;

    if (Physics.BoxCast(castFrom, CameraHalfExtends, castDirection, out RaycastHit hitInfo, lookRotation, castDistance))
    {
      rectPosition = castFrom + castDirection * hitInfo.distance;
      lookPosition = rectPosition - rectOffset;
    }

    transform.SetPositionAndRotation(lookPosition, lookRotation);
  }

  private void UpdateTargetPos()
  {
    previousCamFocus = camFocus;
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

  private bool ManualRotation()
  {
    // Vector2 input = new Vector2(Input.GetAxis("Vertical Camera"), Input.GetAxis("Horizontal Camera"));
    Vector2 rawInput = lookAction.ReadValue<Vector2>();
    Vector2 input = new Vector2(rawInput.y, rawInput.x);

    const float deadZone = 0.001f;

    if (input.x < -deadZone || input.x > deadZone || input.y < -deadZone || input.y > deadZone)
    {
      orbitAngles += rotationSpeed * Time.unscaledDeltaTime * input;
      lastManualRotationTime = Time.unscaledTime;
      return true;
    }
    return false;
  }

  private void ConstrainAngles()
  {
    orbitAngles.x = Mathf.Clamp(orbitAngles.x, minVertAngle, maxVertAngle);

    if (orbitAngles.y < 0f)
    {
      orbitAngles.y += 360f;
    }
    else if (orbitAngles.y >= 360f)
    {
      orbitAngles.y -= 360f;
    }
  }

  private bool AutoRotate()
  {
    if (Time.unscaledTime - lastManualRotationTime < alignDelay)
    {
      return false;
    }
    Vector2 movement = new Vector2(camFocus.x - previousCamFocus.x, camFocus.z - previousCamFocus.z);
    float movementDeltaSqr = movement.sqrMagnitude;

    if (movementDeltaSqr < 0.0001f)
    {
      return false;
    }
    float headingAngle = GetAngle(movement / Mathf.Sqrt(movementDeltaSqr));
    float deltaAbs = Mathf.Abs(Mathf.DeltaAngle(orbitAngles.y, headingAngle));
    float rotationChange = rotationSpeed * Mathf.Min(Time.unscaledDeltaTime, movementDeltaSqr);

    if (deltaAbs < alignSmoothRange)
    {
      rotationChange *= deltaAbs / alignSmoothRange;
    }
    else if (180f - deltaAbs < alignSmoothRange)
    {
      rotationChange *= 180f - deltaAbs / alignSmoothRange;
    }
    orbitAngles.y = Mathf.MoveTowardsAngle(orbitAngles.y, headingAngle, rotationChange);
    return true;
  }

  private Vector3 CameraHalfExtends
  {
    get
    {
      Vector3 halfExtends;
      halfExtends.y = cam.nearClipPlane * Mathf.Tan(0.5f * Mathf.Deg2Rad * cam.fieldOfView);
      halfExtends.x = halfExtends.y * cam.aspect;
      halfExtends.z = 0f;
      return halfExtends;
    }
  }
  static float GetAngle(Vector2 dir)
  {
    float angle = Mathf.Acos(dir.y) * Mathf.Rad2Deg;
    return dir.x < 0f ? 360f - angle : angle;
  }
}
