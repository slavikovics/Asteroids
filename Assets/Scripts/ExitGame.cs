using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ExitGame : MonoBehaviour
{
    private void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        StartCoroutine(QuitApplication());
    }

    private IEnumerator QuitApplication()
    {
        EventManager.SceneChangeStarted?.Invoke();
        yield return new WaitForSeconds(2f);
        Application.Quit();
    }
}
