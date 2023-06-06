using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Controller : MonoBehaviour
{
  [SerializeField] private InputActionReference moveControl;
  [SerializeField] private InputActionReference jumpControl;
  [SerializeField] private float playerSpeed = 2.0f;
  [SerializeField] private float jumpHeight = 1.0f;
  [SerializeField] private float gravityValue = -9.81f;

  private CharacterController controller;
  private Vector3 playerVelocity;
  private bool groundedPlayer;
  private Transform cameraMain;

  private void OnEnable() {
    moveControl.action.Enable();
    jumpControl.action.Enable();
  }

  private void OnDisable()
  {
    moveControl.action.Disable();
    jumpControl.action.Disable();
  }

  private void Start()
  {
    controller = gameObject.GetComponent<CharacterController>();
    cameraMain = Camera.main.transform;
  }

  void Update()
  {
    groundedPlayer = controller.isGrounded;
    if (groundedPlayer && playerVelocity.y < 0)
    {
      playerVelocity.y = 0f;
    }

    Vector2 moveInput = moveControl.action.ReadValue<Vector2>();
    Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
    move = cameraMain.transform.forward * move.z + cameraMain.right * move.x;
    move.y = 0;
    controller.Move(move * Time.deltaTime * playerSpeed);

    // Changes the height position of the player..
    if (jumpControl.action.triggered && groundedPlayer)
    {
      playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
    }

    playerVelocity.y += gravityValue * Time.deltaTime;
    controller.Move(playerVelocity * Time.deltaTime);
  }
}
