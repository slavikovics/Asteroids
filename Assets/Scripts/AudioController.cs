using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class AudioController : MonoBehaviour
{
    [FormerlySerializedAs("MasterMixer")] [SerializeField] 
    private AudioMixer masterMixer;
    
    [FormerlySerializedAs("_fadeDuration")] [SerializeField] 
    private float fadeDuration = 5f;

    private void Awake()
    {
        masterMixer.SetFloat("Volume", 0f);
        EventManager.SceneChangeStarted += FadeOutGlobalVolume;
    }

    private void FadeOutGlobalVolume()
    {
        StartCoroutine(FadeMixerVolume());
    }

    private IEnumerator FadeMixerVolume()
    {
        float currentTime = 0;
        masterMixer.GetFloat("Volume", out float startVolume);

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            float newVolume = Mathf.Lerp(startVolume, -80f, currentTime / fadeDuration);
            masterMixer.SetFloat("Volume", newVolume);
            yield return null;
        }
        masterMixer.SetFloat("Volume", -80f);
    }

    private void OnDestroy()
    {
        EventManager.SceneChangeStarted -= FadeOutGlobalVolume;
    }
}
