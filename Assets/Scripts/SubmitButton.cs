using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SubmitButton : MonoBehaviour
{
    [FormerlySerializedAs("_input")] [SerializeField] 
    private TMP_InputField input;

    private void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (input.text != "")
        {
            AddRecord();
            StartCoroutine(SwitchToMainMenu());
        }
    }

    private void AddRecord()
    {
        LeaderboardLoader.AddToTop(input.text, Counter.AsteroidsCount);
    }

    private IEnumerator SwitchToMainMenu()
    {
        EventManager.SceneChangeStarted?.Invoke();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MainMenu");
    }
}
