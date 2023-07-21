using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace RPG.Control
{
  [RequireComponent(typeof(NavMeshAgent))]
  public class PlayerControllerNewInput : MonoBehaviour
  {
    [SerializeField] InputActionAsset inputActions;
    private InputActionMap playerActionMap;
    private InputAction movement;

    [SerializeField] Transform cameraTransform;
    private NavMeshAgent agent;
    [SerializeField, Range(0, 0.99f)] float smoothing = 0.25f;
    [SerializeField] float targetLerpSpeed = 1;

    private Vector3 targetDir;
    private float lerpTime = 0f;
    private Vector3 lastDir;
    private Vector3 moveVector;

    private void Awake()
    {
      agent = GetComponent<NavMeshAgent>();
      playerActionMap = inputActions.FindActionMap("BasicMap");
      movement = playerActionMap.FindAction("Move");
      movement.started += HandleMoveAction;
      movement.performed += HandleMoveAction;
      movement.canceled += HandleMoveAction;
    }

    private void Update()
    {
      moveVector.Normalize();
      if (moveVector != lastDir)
      {
        lerpTime = 0f;
      }
      lastDir = moveVector;
      targetDir = Vector3.Lerp(targetDir, moveVector, Mathf.Clamp01(lerpTime * targetLerpSpeed * (1 - smoothing)));

      agent.Move(targetDir * agent.speed * Time.deltaTime);

      Vector3 lookDir = moveVector;
      if (lookDir != Vector3.zero)
      {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookDir), Mathf.Clamp01(lerpTime * targetLerpSpeed * (1 - smoothing)));
      }

      lerpTime += Time.deltaTime;
    }

    private void OnEnable()
    {
      movement.Enable();
      playerActionMap.Enable();
      inputActions.Enable();
    }

    private void OnDisable()
    {
      movement.Disable();
      playerActionMap.Disable();
      inputActions.Disable();
    }

    private void HandleMoveAction(InputAction.CallbackContext context)
    {
      Vector2 input = context.ReadValue<Vector2>();

      if (cameraTransform)
      {
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;

        moveVector = (camForward * input.y + camRight * input.x);

      }
      else
      {
        moveVector = new Vector3(input.x, 0, input.y);
      }
    }
  }
}
