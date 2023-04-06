using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSphere : MonoBehaviour
{
  [SerializeField] controlType currentControl = controlType.none;
  [Range(0, 25)][SerializeField] int maxSpeed = 2;
  [Range(0, 25)][SerializeField] int maxAcceleration = 2;
  [Range(0, 25)][SerializeField] int maxAirAcceleration = 1;
  [Range(0f, 10f)][SerializeField] float jumpHeight = 5f;
  [Range(0, 3)][SerializeField] int airJumps = 1;
  [Range(0f, 1f)][SerializeField] float bounce = 0.5f;
  [SerializeField] Rect allowedArea = new Rect(-5f, -5f, 10f, 10f);
  Vector3 velocity;
  Vector3 desiredVelocity;
  Rigidbody rb;
  bool inputToJump;
  bool isGrounded;
  int jumpsSinceGrounded;

  private void Awake()
  {
    rb = GetComponent<Rigidbody>();
  }

  private void Update()
  {
    Vector2 playerInput = GetInput();
    desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;

    inputToJump |= Input.GetButtonDown("Jump");

    if (currentControl == controlType.fakePhysx)
    {
      MoveSphereSimulatePhys2D();
    }
  }

  private void FixedUpdate()
  {
    UpdateState();

    if (currentControl == controlType.physxRigidbody)
    {
      MoveSpherePhysxRigidBoy();
    }

    if (inputToJump)
    {
      inputToJump = false;
      Jump();
    }
    isGrounded = false;
    rb.velocity = velocity;
  }

  private void UpdateState()
  {
    velocity = rb.velocity;
    if (isGrounded)
    {
      jumpsSinceGrounded = 0;
    }
  }

  private void OnCollisionEnter(Collision other)
  {
    EvaluateCollision(other);
  }
  private void OnCollisionStay(Collision other)
  {
    EvaluateCollision(other);
  }
  private void EvaluateCollision(Collision collision)
  {
    for (int i = 0; i < collision.contactCount; i++)
    {
      Vector3 normal = collision.GetContact(i).normal;
      isGrounded |= normal.y >= 0.9f;
    }
  }

  private Vector2 GetInput()
  {
    //get input and assign to vector2
    Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    //normalize input; prevent diagonal input from exceeding 1
    input = Vector3.ClampMagnitude(input, 1f);
    return input;
  }

  private void MoveSphereSimulatePhys2D()
  {
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

  private void MoveSpherePhysxRigidBoy()
  {
    float acceleration = isGrounded ? maxAcceleration : maxAirAcceleration;
    float maxSpeedChange = acceleration * Time.deltaTime;

    velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
    velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

  }

  private void Jump()
  {
    if (isGrounded || jumpsSinceGrounded < airJumps)
    {
      jumpsSinceGrounded += 1;
      float jumpValue = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
      if (velocity.y > 0)
      {
        jumpValue = Mathf.Max(jumpValue - velocity.y, 0f);
      }
      velocity.y += jumpValue;
    }
  }

  public enum controlType
  {
    none,
    fakePhysx,
    physxRigidbody,
    physxKinematic
  }
}
