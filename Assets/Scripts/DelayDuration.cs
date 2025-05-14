using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public enum UIItems
{
    HyperAttack,
    Shield,
    Slowdown,
    Teleport
}

public class DelayDuration : MonoBehaviour
{
    [FormerlySerializedAs("AnimationName")] [SerializeField]
    private string animationName;

    [FormerlySerializedAs("EffectName")] [SerializeField]
    private UIItems effectName;

    private Animator _animator;

    private bool _wasControllerSpeedAdjusted = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        switch (effectName)
        {
            case UIItems.HyperAttack: EventManager.HyperAttackUsed += BeginCountdownAnimation;
                break;
            
            case UIItems.Shield: EventManager.ShieldUsed += BeginCountdownAnimation;
                break;

            case UIItems.Slowdown: EventManager.SlowdownEffectApplied += BeginCountdownAnimation;
                break;

            case UIItems.Teleport: EventManager.TeleportApplied += BeginCountdownAnimation;
                break;
        }
    }

    private void AdjustControllerSpeed(float time)
    {
        if (_animator.runtimeAnimatorController == null) return;

        var clips = _animator.runtimeAnimatorController.animationClips;
        float clipLength = -1f;

        foreach (var clip in clips)
        {
            if (clip.name == animationName)
            {
                clipLength = clip.length;
                break;
            }
        }

        if (clipLength <= 0f)
        {
            Debug.LogWarning($"Animation '{animationName}' not found or has invalid length.");
            return;
        }

        float animationSpeed = clipLength / time;
        _animator.speed = animationSpeed;

        _wasControllerSpeedAdjusted = true;
    }

    private void BeginCountdownAnimation(float time)
    {
        if (!_wasControllerSpeedAdjusted) AdjustControllerSpeed(time);

        StartCoroutine(ResetAnimateAfter(time));
        _animator.SetBool("Animate", true);
    }

    private IEnumerator ResetAnimateAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        _animator.SetBool("Animate", false);
    }

    private void OnDestroy()
    {
        switch (effectName)
        {
            case UIItems.HyperAttack: EventManager.HyperAttackUsed -= BeginCountdownAnimation;
                break;
            
            case UIItems.Shield: EventManager.ShieldUsed -= BeginCountdownAnimation;
                break;

            case UIItems.Slowdown: EventManager.SlowdownEffectApplied -= BeginCountdownAnimation;
                break;

            case UIItems.Teleport: EventManager.TeleportApplied -= BeginCountdownAnimation;
                break;
        }
    }
}


