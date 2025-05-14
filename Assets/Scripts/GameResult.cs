using Unity.VisualScripting;
using UnityEngine;

public class GameResult : MonoBehaviour
{
    public static int AsteroidsCount { get; set; }

    public GameResult()
    {
        EventManager.SceneChangeStarted += UpdateCount;
    }

    private void Awake()
    {
        Debug.Log($"Asteroids count: {AsteroidsCount}");
    }

    private static void UpdateCount()
    {
        AsteroidsCount = Counter.AsteroidsCount;
    }
}
