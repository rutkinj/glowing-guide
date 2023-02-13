using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.SceneManagement
{
  public class Fader : MonoBehaviour
  {
    CanvasGroup canvasGroup;
    [SerializeField] float fadeTime = 3f;

    private void Start() {
      canvasGroup = GetComponent<CanvasGroup>();

      StartCoroutine(FadeOutIn());
    }

    IEnumerator FadeOutIn(){
      yield return FadeOut(fadeTime);
      yield return FadeIn(fadeTime);
    }
    public IEnumerator FadeOut(float time)
    {
      while(canvasGroup.alpha < 1){
        canvasGroup.alpha += Time.deltaTime / time;
        yield return null;
      }
    }

    public IEnumerator FadeIn(float time)
    {
      while (canvasGroup.alpha > 0)
      {
        canvasGroup.alpha -= Time.deltaTime / time;
        yield return null;
      }
    }
  }
}