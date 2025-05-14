using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class UfoSoundController : MonoBehaviour
{
    [FormerlySerializedAs("_player")]
    [Header("References")]
    
    [SerializeField] private Transform player;
    
    private AudioSource _ufoAudio;
    
    [FormerlySerializedAs("_targetMixerGroup")] [SerializeField] 
    private AudioMixerGroup targetMixerGroup;

    [FormerlySerializedAs("_maxDistance")]
    [Header("Sound Settings")]
    
    [SerializeField] private float maxDistance;
    
    [FormerlySerializedAs("_minDistance")] [SerializeField] 
    private float minDistance = 2f;
    
    [FormerlySerializedAs("_maxVolume")] [SerializeField] 
    private float maxVolume = 1f;
    
    [FormerlySerializedAs("_minVolume")] [SerializeField] 
    private float minVolume = 0.1f;

    private void Awake()
    {
        _ufoAudio = GetComponent<AudioSource>();
        EventManager.SceneChangeStarted += AddToMixerGroup;
    }

    private void AddToMixerGroup()
    {
        _ufoAudio.outputAudioMixerGroup = targetMixerGroup;
    }

    private void OnDestroy()
    {
        EventManager.SceneChangeStarted -= AddToMixerGroup;
    }

    private void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        float volume = Mathf.InverseLerp(maxDistance, minDistance, distance);
        volume = Mathf.Clamp(volume, minVolume, maxVolume);

        _ufoAudio.volume = volume;

        if (!_ufoAudio.isPlaying)
        {
            _ufoAudio.Play();
        }
    }
}
