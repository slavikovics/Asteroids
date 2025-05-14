using UnityEngine;
using UnityEngine.Serialization;

public class LaserAnimation : MonoBehaviour
{
    [FormerlySerializedAs("AnimationName")] public string animationName;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        EventManager.LaserShoot += Animate;
    }

    private void Animate()
    {
        if (_animator is null) return;
        _animator.Play(animationName, 0, 0f);
    }

    void OnDestroy()
    {
        EventManager.LaserShoot -= Animate;
    }
}
