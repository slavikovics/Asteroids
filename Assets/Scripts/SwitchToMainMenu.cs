using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwitchToMainMenu : MonoBehaviour
{
    private void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        StartCoroutine(SwitchToBattleScene());
    }

    private IEnumerator SwitchToBattleScene()
    {
        EventManager.SceneChangeStarted?.Invoke();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MainMenu");
    }
}
