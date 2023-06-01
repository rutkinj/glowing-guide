using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
  [SerializeField] Material red;

  private void OnJump()
  {
    FindObjectOfType<SphereCollider>().GetComponent<MeshRenderer>().material = red;
  }
}
