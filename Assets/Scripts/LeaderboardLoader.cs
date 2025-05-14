using UnityEngine;
using TMPro;
using System.IO;
using System;

public class LeaderboardLoader : MonoBehaviour
{
    public class Leaderboard
    {
        public string FirstName;
    
        public int FirstCount;
    
        public string SecondName;
    
        public int SecondCount;
    
        public string ThirdName;
    
        public int ThirdCount;
    
        public Leaderboard()
        {
            if (PlayerPrefs.HasKey("FirstName")) Load();
            else LoadDefaults();
        }
    
        public void LoadDefaults()
        {
            PlayerPrefs.SetString("FirstName", "_");
            PlayerPrefs.SetInt("FirstCount", 0);
            PlayerPrefs.SetString("SecondName", "_");
            PlayerPrefs.SetInt("SecondCount", 0);
            PlayerPrefs.SetString("ThirdName", "_");
            PlayerPrefs.SetInt("ThirdCount", 0);
            PlayerPrefs.Save();
        }
    
        public void Load()
        {
            FirstName = PlayerPrefs.GetString("FirstName");
            FirstCount = PlayerPrefs.GetInt("FirstCount");
            SecondName = PlayerPrefs.GetString("SecondName");
            SecondCount = PlayerPrefs.GetInt("SecondCount");
            ThirdName = PlayerPrefs.GetString("ThirdName");
            ThirdCount = PlayerPrefs.GetInt("ThirdCount");
        }
    
        public void Save() 
        {
            PlayerPrefs.SetString("FirstName", FirstName);
            PlayerPrefs.SetInt("FirstCount", FirstCount);
            PlayerPrefs.SetString("SecondName", SecondName);
            PlayerPrefs.SetInt("SecondCount", SecondCount);
            PlayerPrefs.SetString("ThirdName", ThirdName);
            PlayerPrefs.SetInt("ThirdCount", ThirdCount);
            PlayerPrefs.Save();
        }
    }
    
    public static Leaderboard CurrentLeaderboard { get; set; }

    private static bool _wasLoaded = false;
    
    private void Awake()
    {
        if (!_wasLoaded)
        {
            CurrentLeaderboard = new Leaderboard();
            _wasLoaded = true;
        }
    }

    public static bool CheckTop(int asteroidsCount)
    {
        return asteroidsCount > CurrentLeaderboard.ThirdCount;
    }

    public static void AddToTop(string name, int asteroidsCount) 
    {
        if (asteroidsCount > CurrentLeaderboard.FirstCount) 
        {
            CurrentLeaderboard.ThirdName = CurrentLeaderboard.SecondName;
            CurrentLeaderboard.ThirdCount = CurrentLeaderboard.SecondCount;
            CurrentLeaderboard.SecondName = CurrentLeaderboard.FirstName;
            CurrentLeaderboard.SecondCount = CurrentLeaderboard.FirstCount;
            CurrentLeaderboard.FirstName = name;
            CurrentLeaderboard.FirstCount = asteroidsCount;
        } 
        else if (asteroidsCount > CurrentLeaderboard.SecondCount) 
        {
            CurrentLeaderboard.ThirdName = CurrentLeaderboard.SecondName;
            CurrentLeaderboard.ThirdCount = CurrentLeaderboard.SecondCount;
            CurrentLeaderboard.SecondName = name;
            CurrentLeaderboard.SecondCount = asteroidsCount;
        } 
        else if (asteroidsCount > CurrentLeaderboard.ThirdCount) 
        {
            CurrentLeaderboard.ThirdName = name;
            CurrentLeaderboard.ThirdCount = asteroidsCount;
        } 
        else return;
        
        CurrentLeaderboard.Save();
    }
}
