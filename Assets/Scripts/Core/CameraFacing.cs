using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
  public class CameraFacing : MonoBehaviour
  {
    Camera camToFace;

    void Start()
    {
        camToFace = FindObjectOfType<Camera>();
    }

    void LateUpdate()
    {
        transform.SetPositionAndRotation(transform.position, camToFace.transform.rotation);
    }
  }
}
