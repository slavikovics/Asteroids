using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Serialization;

public class Crossfade : MonoBehaviour
{
    [FormerlySerializedAs("_isShort")] [SerializeField] 
    private bool isShort;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        EventManager.SceneChangeStarted += FadeOut;
    }

    private void FadeOut()
    {
        if (!isShort) _animator.SetTrigger("Start");
        else _animator.SetTrigger("StartShort");
    }

    private void OnDestroy()
    {
        EventManager.SceneChangeStarted -= FadeOut;        
    }
}
