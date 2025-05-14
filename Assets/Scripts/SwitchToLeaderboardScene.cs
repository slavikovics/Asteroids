using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwitchToLeaderboardScene : MonoBehaviour
{
    private void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        StartCoroutine(SwitchToLeaderboard());
    }

    private IEnumerator SwitchToLeaderboard()
    {
        EventManager.SceneChangeStarted?.Invoke();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Leaderboard");
    }
}
