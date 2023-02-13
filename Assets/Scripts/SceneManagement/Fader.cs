using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.SceneManagement
{
  public class Fader : MonoBehaviour
  {
    CanvasGroup canvasGroup;
    [SerializeField] float fadeTime = 0.5f;

    private void Start() {
      canvasGroup = GetComponent<CanvasGroup>();
    }

    IEnumerator FadeOutIn(){
      yield return FadeOut();
      yield return FadeIn();
    }
    public IEnumerator FadeOut()
    {
      while(canvasGroup.alpha < 1){
        canvasGroup.alpha += Time.deltaTime / fadeTime;
        yield return null;
      }
    }

    public IEnumerator FadeIn()
    {
      while (canvasGroup.alpha > 0)
      {
        canvasGroup.alpha -= Time.deltaTime / fadeTime;
        yield return null;
      }
    }
  }
}