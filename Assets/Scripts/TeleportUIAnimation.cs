using UnityEngine;

public class TeleportUIAnimation : MonoBehaviour
{
    public string AnimationName;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        EventManager.TeleportApplied += Animate;
    }

    private void Animate(float param)
    {
        if (_animator is null) return;
        _animator.Play(AnimationName, 0, 0f);
    }

    void OnDestroy()
    {
        EventManager.TeleportApplied -= Animate;
    }
}
