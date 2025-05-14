using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Counter : MonoBehaviour
{
    private static int _livesCount;

    public static int AsteroidsCount { get; private set; }

    [FormerlySerializedAs("AsteroidsText")] [SerializeField]
    private TextMeshProUGUI asteroidsText;

    [FormerlySerializedAs("LivesText")] [SerializeField]
    private TextMeshProUGUI livesText;

    public void Start()
    {
        _livesCount = 5;
        AsteroidsCount = 0;

        EventManager.AsteroidDestroyedByLaser += AsteroidsUp;
        EventManager.AsteroidCollidedWithSpaceShip += LivesDown;
    }

    private void AsteroidsUp()
    {
        AsteroidsCount++;
        asteroidsText.text = AsteroidsCount.ToString();
    }

    private void LivesDown()
    {
        _livesCount--;
        livesText.text = _livesCount.ToString();

        if (_livesCount == 0)
        {
            EventManager.AsteroidCollidedWithSpaceShip -= LivesDown;
            EventManager.SceneChangeStarted?.Invoke();
            EventManager.AsteroidDestroyedByLaser -= AsteroidsUp;

            StartCoroutine(SwitchScene());
        } 
    }

    private IEnumerator SwitchScene()
    {
        bool shouldEnterName = LeaderboardLoader.CheckTop(AsteroidsCount);
        yield return new WaitForSeconds(5.3f);

        if (!shouldEnterName) SceneManager.LoadScene("GameOverScene");
        else SceneManager.LoadScene("NameInputScene");
    }
}
