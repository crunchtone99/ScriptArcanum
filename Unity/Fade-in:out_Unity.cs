using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ForestAmb : MonoBehaviour
{
    public AudioSource Forest;
    float targetVolume;
    public float duration;



    void Start()
    {
        Forest = gameObject.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag =="Player")
        {
            Debug.Log("New Location: Yard!");
            targetVolume = 0f;
            StartCoroutine(FadeAudioSource.StartFade(Forest, duration, targetVolume));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag =="Player")
        {
            Debug.Log("New Location: Forest!");
            targetVolume = 0.07f;
            Forest.volume = 0f;
            Forest.Play();
            StartCoroutine(FadeAudioSource.StartFade(Forest, duration, targetVolume));
        }
        
    }

    public static class FadeAudioSource
    {
        public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
        {
            float currentTime = 0;
            float start = audioSource.volume;
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
                yield return null;
            }
            yield break;
        }
    }
}
