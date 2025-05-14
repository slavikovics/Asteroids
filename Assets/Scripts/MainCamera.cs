using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCamera : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        EventManager.SceneChangeStarted += GameOver;
    }

    private void GameOver()
    {
        _animator.SetBool("Death", true);
        StartCoroutine(LoadGameOverScene());
    }

    private IEnumerator LoadGameOverScene()
    {
        EventManager.SceneChangeStarted -= GameOver;
        yield return null;
    }
}
