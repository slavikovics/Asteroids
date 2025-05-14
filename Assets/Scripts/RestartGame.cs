using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RestartGame : MonoBehaviour
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
        SceneManager.LoadScene("BattleScene");
    }
}
