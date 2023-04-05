using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXRandomizer : MonoBehaviour
{
  [SerializeField] bool fromPool;
  [SerializeField] bool randomPitch;
  [SerializeField] AudioClip[] clipsPool;
  AudioSource audioSource = null;
  private void Awake()
  {
    audioSource = GetComponent<AudioSource>();
  }

  public void ClipRandomizer()
  {
    AudioClip clipToPlay = audioSource.clip;
    if (fromPool)
    {
      int index = Random.Range(0, clipsPool.Length);
      clipToPlay = clipsPool[index];
    }

    if (randomPitch)
    {
      audioSource.pitch = Random.Range(0.8f, 1.2f);
    }
    else audioSource.pitch = 1;
    audioSource.PlayOneShot(clipToPlay);
  }
}
