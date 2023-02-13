using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

namespace RPG.SceneManagement
{
  public class Portal : MonoBehaviour
  {
    enum DestinationId
    {
      A, B, C, D, E
    }
    [SerializeField] int sceneIndex = -1;
    [SerializeField] Transform spawnPoint;
    [SerializeField] DestinationId destinationId;
    private void OnTriggerEnter(Collider other)
    {
      if (other.tag == "Player")
      {
        StartCoroutine(Transition());
      }
    }

    private IEnumerator Transition()
    {
      DontDestroyOnLoad(gameObject);
      yield return SceneManager.LoadSceneAsync(sceneIndex);

      Portal otherPortal = GetOtherPortal();
      UpdatePlayer(otherPortal);

      Destroy(gameObject);
    }

    private void UpdatePlayer(Portal otherPortal)
    {
      GameObject player = GameObject.FindWithTag("Player");
      player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
      player.transform.rotation = otherPortal.spawnPoint.rotation;
    }

    private Portal GetOtherPortal()
    {
      Portal[] portals = FindObjectsOfType<Portal>();
      foreach (Portal portal in portals)
      {
        if (portal == this) continue;
        if (portal.destinationId != destinationId)
        {
          return portal;
        }
      }
      return null;
    }
  }
}
