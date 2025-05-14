using UnityEngine;

public class HyperAttackAnimation : MonoBehaviour
{
    public string AnimationName;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        EventManager.HyperAttackUsed += Animate;
    }

    private void Animate(float arg)
    {
        if (_animator is null) return;
        _animator.Play(AnimationName, 0, 0f);
    }

    void OnDestroy()
    {
        EventManager.HyperAttackUsed -= Animate;
    }
}
