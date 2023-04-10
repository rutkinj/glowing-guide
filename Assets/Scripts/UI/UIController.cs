using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
  [SerializeField] GameObject pauseScreen;
  [SerializeField] GameObject characterScreen;
  [SerializeField] GameObject debugScreen;

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.P))
    {
      ToggleScreen(pauseScreen);
      if (characterScreen.activeSelf) ToggleScreen(characterScreen);
    }
    if (Input.GetKeyDown(KeyCode.I))
    {
      ToggleScreen(characterScreen);
      if (pauseScreen.activeSelf) ToggleScreen(pauseScreen);
    }
    if (Input.GetKeyDown(KeyCode.O))
    {
      ToggleScreen(debugScreen);
    }
  }

  private void ToggleScreen(GameObject screen)
  {
    screen.SetActive(!screen.activeSelf);

  }
}
