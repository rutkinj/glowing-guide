using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSphere : MonoBehaviour
{
  [Range(0, 25)][SerializeField] int maxSpeed = 2;
  [Range(0, 25)][SerializeField] int maxAcceleration = 2;
  [Range(0f, 1f)][SerializeField] float bounce = 0.5f;
  [SerializeField] Rect allowedArea = new Rect(-5f, -5f, 10f, 10f);

  Vector3 velocity;

  private void Update()
  {
    Vector2 playerInput = GetInput();

    MoveSphereSimulatePhys2D(playerInput);
  }

  private Vector2 GetInput()
  {
    //get input and assign to vector2
    Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    //normalize input; prevent diagonal input from exceeding 1
    input = Vector3.ClampMagnitude(input, 1f);
    return input;
  }

  private void MoveSphereSimulatePhys2D(Vector2 playerInput)
  {
    Vector3 desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
    float maxSpeedChange = maxAcceleration * Time.deltaTime;

    velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
    velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

    Vector3 displacement = velocity * Time.deltaTime;

    Vector3 newPos = transform.localPosition + displacement;
    if (newPos.x > allowedArea.xMax)
    {
      newPos.x = allowedArea.xMax;
      velocity.x = -velocity.x * bounce;
    }
    else if (newPos.x < allowedArea.xMin)
    {
      newPos.x = allowedArea.xMin;
      velocity.x = -velocity.x * bounce;
    }

    if (newPos.z > allowedArea.yMax)
    {
      newPos.z = allowedArea.yMax;
      velocity.z = -velocity.z * bounce;
    }
    else if (newPos.z < allowedArea.yMin)
    {
      newPos.z = allowedArea.yMin;
      velocity.z = -velocity.z * bounce;
    }
    transform.localPosition = newPos;
  }
}
