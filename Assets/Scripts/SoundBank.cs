using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SoundBank : MonoBehaviour
{
    private static SoundBank _instance;
    private Dictionary<string, AudioClip> _soundCache = new Dictionary<string, AudioClip>();

    public static SoundBank Instance => _instance;

    private void Awake()
    {
        GetSound("/Assets/Sounds/");
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public AudioClip GetSound(string soundPath)
    {
        if (_soundCache.TryGetValue(soundPath, out AudioClip clip))
            return clip;

        AudioClip newClip = Resources.Load<AudioClip>(soundPath);
        if (newClip != null)
        {
            _soundCache.Add(soundPath, newClip);
            return newClip;
        }

        Debug.LogError($"Sound not found: {soundPath}");
        return null;
    }
}
