using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
  public class DestroyPaticleOnFinish : MonoBehaviour
  {
    [SerializeField] GameObject toDestroy = null;
    void Update()
    {
      if (!GetComponent<ParticleSystem>().IsAlive())
      {
        if (toDestroy != null)
        {
          Destroy(toDestroy);
        }
        Destroy(gameObject);
      }

    }
  }
}
