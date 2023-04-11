using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour
{
  [SerializeField] controlType currentControl = controlType.none;
  [Range(0, 25)][SerializeField] float maxSpeed = 2;
  [Range(0, 25)][SerializeField] float maxAcceleration = 2;
  [Range(0, 25)][SerializeField] float maxAirAcceleration = 1;
  [Range(0, 25)][SerializeField] float maxSnapSpeed = 15f;
  [Range(0f, 10f)][SerializeField] float jumpHeight = 5f;
  [Range(0, 3)][SerializeField] int airJumps = 1;
  [Range(0, 90)][SerializeField] float maxGroundAngle = 25f;
  [SerializeField, Min(1f)] float snapProbeDistance = 2f;
  [SerializeField] LayerMask probeMask = -1;

  [Header("For fake physics controls")]
  [Range(0f, 1f)][SerializeField] float bounce = 0.5f;
  [SerializeField] Rect allowedArea = new Rect(-5f, -5f, 10f, 10f);
  Vector3 velocity;
  Vector3 desiredVelocity;
  Rigidbody rb;
  bool inputToJump;
  bool isGrounded;
  int jumpsSinceGrounded;
  int stepsSinceGrounded;
  int stepsSinceJumped;
  float minGroundDotProduct;
  Vector3 groundNormal;
  Vector3 steepNormal;
  int steepContactCount;
  bool OnSteep => steepContactCount > 0;

  private void OnValidate()
  {
    minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
    if (maxSnapSpeed == maxSpeed)
    {
      if (maxSpeed == 0) return;
      maxSnapSpeed = maxSpeed - 1;
    }
  }

  private void Awake()
  {
    rb = GetComponent<Rigidbody>();
    OnValidate();
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
      //   MoveSpherePhysxRigidBoy();
      AdjustVelocity();
    }

    if (inputToJump)
    {
      inputToJump = false;
      Jump();
    }

    rb.velocity = velocity;
    ClearState();
  }

  private void UpdateState()
  {
    stepsSinceGrounded += 1;
    stepsSinceJumped += 1;
    velocity = rb.velocity;
    if (isGrounded || SnapToGround() || CheckSteepContacts())
    {
      stepsSinceGrounded = 0;
      jumpsSinceGrounded = 0;
      groundNormal.Normalize();
    }
    else
    {
      groundNormal = Vector3.up;
    }
  }

  private void ClearState()
  {
    isGrounded = false;
    groundNormal = Vector3.zero;
    steepNormal = Vector3.zero;
    steepContactCount = 0;
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
      if (normal.y >= minGroundDotProduct)
      {
        isGrounded = true;
        groundNormal += normal;
      }
      else if (normal.y > -0.01f)
      {
        steepContactCount += 1;
        steepNormal += normal;
      }
    }
  }

  private bool CheckSteepContacts() //this is a fix for getting stuck off the ground
  {
    if (steepContactCount > 1)
    {
      steepNormal.Normalize();
      if (steepNormal.y >= minGroundDotProduct)
      {
        isGrounded = true;
        groundNormal = steepNormal;
        return true;
      }
    }
    return false;
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
      stepsSinceJumped = 0;
      jumpsSinceGrounded += 1;
      float jumpValue = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
      float alignedSpeed = Vector3.Dot(velocity, groundNormal);
      if (alignedSpeed > 0f)
      {
        jumpValue = Mathf.Max(jumpValue - alignedSpeed, 0f);
      }
      velocity += (jumpValue * groundNormal);
    }
  }

  private Vector3 ProjectOnContactPlane(Vector3 vector)
  {
    return vector - groundNormal * Vector3.Dot(vector, groundNormal);
  }

  private void AdjustVelocity()
  {
    Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
    Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;

    float currentX = Vector3.Dot(velocity, xAxis);
    float currentZ = Vector3.Dot(velocity, zAxis);

    float acceleration = isGrounded ? maxAcceleration : maxAirAcceleration;
    float maxSpeedChange = acceleration * Time.deltaTime;

    float newX = Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
    float newZ = Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange);

    velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
  }

  private bool SnapToGround()
  {
    float speed = velocity.magnitude;
    if (speed > maxSnapSpeed)
    {
      print("Too fast! no snap");
      return false;
    }

    if (stepsSinceGrounded > 1 || stepsSinceJumped <= 2)
    {
      print("definitely off the ground, no snap");
      return false;
    }

    if (!Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, snapProbeDistance, probeMask))
    {
      print("raycast didn't hit any ground, no snap");
      return false;
    }

    if (hit.normal.y < minGroundDotProduct)
    {
      print("ground is too steep, no snap");
      return false;
    }

    groundNormal = hit.normal;
    float dot = Vector3.Dot(velocity, hit.normal);
    if (dot > 0f)
    {
      velocity = (velocity - hit.normal * dot).normalized * speed;
    }
    print("we snappin now boyz!!");
    return true;
  }

  public enum controlType
  {
    none,
    fakePhysx,
    physxRigidbody,
    physxKinematic
  }
}
