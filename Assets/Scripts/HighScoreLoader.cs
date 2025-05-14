using TMPro;
using UnityEngine;

public class HighScoreLoader : MonoBehaviour
{
    [SerializeField] private TMP_Text _first;

    [SerializeField] private TMP_Text _second;

    [SerializeField] private TMP_Text _third;

    void Awake()
    {
        _first.text = $"1. {LeaderboardLoader.CurrentLeaderboard.FirstName} - {LeaderboardLoader.CurrentLeaderboard.FirstCount}";
        _second.text = $"2. {LeaderboardLoader.CurrentLeaderboard.SecondName} - {LeaderboardLoader.CurrentLeaderboard.SecondCount}";
        _third.text = $"3. {LeaderboardLoader.CurrentLeaderboard.ThirdName} - {LeaderboardLoader.CurrentLeaderboard.ThirdCount}";
    }
}
