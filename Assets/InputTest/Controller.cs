using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
  private PlayerInput playerInput;
  private InputAction jump;

  [SerializeField] Material red;
  [SerializeField] Material green;
  [SerializeField] Material regular;

  private void Awake()
  {
    playerInput = GetComponent<PlayerInput>();
    jump = playerInput.actions["Jump"];
  }

  private void OnEnable()
  {
    jump.started += Jump;
    jump.performed += Jump;
    jump.canceled += Jump;
  }

  private void OnDisable()
  {
    jump.performed -= Jump;
  }

  private void Jump(InputAction.CallbackContext ctx)
  {
    if (ctx.started)
    {
      FindObjectOfType<SphereCollider>().GetComponent<MeshRenderer>().material = green;
    }
    else if (ctx.performed)
    {
      FindObjectOfType<SphereCollider>().GetComponent<MeshRenderer>().material = red;
    }
    else if(ctx.canceled){
      FindObjectOfType<SphereCollider>().GetComponent<MeshRenderer>().material = regular;

    }
  }

  //   private void OnJump()
  //   {
  //     FindObjectOfType<SphereCollider>().GetComponent<MeshRenderer>().material = red;
  //   }
}
