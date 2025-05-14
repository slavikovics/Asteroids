using UnityEngine;
using UnityEngine.Serialization;

public class TimeSlowdownUIAnimation : MonoBehaviour
{
    [FormerlySerializedAs("AnimationName")] public string animationName;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        EventManager.SlowdownEffectApplied += Animate;
    }

    private void Animate(float param)
    {
        if (_animator is null) return;
        _animator.Play(animationName, 0, 0f);
    }

    void OnDestroy()
    {
        EventManager.SlowdownEffectApplied -= Animate;
    }
}
