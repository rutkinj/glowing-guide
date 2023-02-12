using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
  public class Portal : MonoBehaviour
  {
    [SerializeField] int sceneIndex = -1;
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player"){
            SceneManager.LoadScene(sceneIndex);
        }
        print("TRIGGERED!!");
    }
  }
}
