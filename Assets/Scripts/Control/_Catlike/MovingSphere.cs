using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSphere : MonoBehaviour
{
  Vector3 playerInput;

  void Update()
  {
    playerInput.x = Input.GetAxis("Horizontal");
    playerInput.z = Input.GetAxis("Vertical");
    playerInput = Vector3.ClampMagnitude(playerInput, 1f);
    playerInput.y = 0.5f;
    transform.localPosition = playerInput;
  }
}
