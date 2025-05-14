using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class CenralSoundEffects : MonoBehaviour
{
    [FormerlySerializedAs("HyperAttack")] [SerializeField]
    private AudioClip hyperAttack;

    [FormerlySerializedAs("AsteroidExploaded")] [SerializeField]
    private AudioClip asteroidExploaded;

    [FormerlySerializedAs("AsteroidCollidedWithShip")] [SerializeField]
    private AudioClip asteroidCollidedWithShip;

    [FormerlySerializedAs("TeleportApplied")] [SerializeField]
    private AudioClip teleportApplied;

    [FormerlySerializedAs("AudioMixer")] [SerializeField]
    private AudioMixer audioMixer;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        EventManager.HyperAttackUsed += PlayHyperAttackSound;
        EventManager.AsteroidDestroyedByLaser += PlayAsteroidExploadedSound;
        EventManager.AsteroidCollidedWithSpaceShip += PlayAsteroidCollidedSound;
        EventManager.TeleportApplied += TeleportAppliedSound;
        audioMixer.SetFloat("Volume", 0f);
    }

    private void PlayHyperAttackSound(float f)
    {
        _audioSource.PlayOneShot(hyperAttack);
    }

    private void PlayAsteroidExploadedSound()
    {
        _audioSource.PlayOneShot(asteroidExploaded);
    }

    private void PlayAsteroidCollidedSound()
    {
        _audioSource.PlayOneShot(asteroidCollidedWithShip);
    }

    private void TeleportAppliedSound(float f)
    {
        _audioSource.PlayOneShot(teleportApplied);
    }

    private void OnDestroy()
    {
        EventManager.HyperAttackUsed -= PlayHyperAttackSound;
        EventManager.AsteroidDestroyedByLaser -= PlayAsteroidExploadedSound;
        EventManager.AsteroidCollidedWithSpaceShip -= PlayAsteroidCollidedSound;
        EventManager.TeleportApplied -= TeleportAppliedSound;
    }
}
