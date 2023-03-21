using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Saving;

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

      Fader fader = FindObjectOfType<Fader>();
      SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();

      yield return StartCoroutine(fader.FadeOut());
      savingWrapper.Save();
      yield return SceneManager.LoadSceneAsync(sceneIndex);
      savingWrapper.Load();

      Portal otherPortal = GetOtherPortal();
      UpdatePlayer(otherPortal);
      savingWrapper.Save();

      yield return StartCoroutine(fader.FadeIn());

      Destroy(gameObject);
    }

    private void UpdatePlayer(Portal otherPortal)
    {
      GameObject player = GameObject.FindWithTag("Player");
      //might be a bug here? need to test portals w save system
      //if player teleports to wrong position, must disable, then reeneable navmesh agent around below two lines
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
