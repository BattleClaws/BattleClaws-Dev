using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScoreManager : MonoBehaviour
{
    [SerializeField] private GameObject[] uiCorners;

    //public int[] scores = new int[4];
    public Dictionary<int, int> PlayersScores
    {
        get
        {
            return scores;
        }
        set
        {
            scores = value;
            UpdatePlayerUI();
        }
    }

    private Dictionary<int, int> scores;

    public int numberOfPlayers;
    
    
    public Color bgColorBase;
    public Color bgColorWarning;

    public int DEBUGVAL;

    private void Start()
    {
        PlayersScores = new Dictionary<int, int>();
        UpdatePlayerUI();
    }

    public void InjectScore(int playerNum, int score)
    {
        //print("Score Injected!");
        var tempscores = PlayersScores;

        if (tempscores.ContainsKey(playerNum))
        {
            tempscores[playerNum] = score;
        }
        else
        {
            tempscores.Add(playerNum,0);
        }

        
        PlayersScores = tempscores;
    }

    public int GetLowestScore()
    {
        int low = 999999999;

        foreach (var entry in PlayersScores)
        {
            if (entry.Value < low && entry.Value >= 0)
            {
                low = entry.Value;
            }
        }

        //print("Lowest Score " + low);
        return low;
    }
    
    public int GetHighestScore()
    {
        int high = -9999;
        
        foreach (var entry in PlayersScores)
        {
            if (entry.Value > high && entry.Value >= 0)
            {
                high = entry.Value;
            }
        }
        //print("Highest Score: " + high);
        return high;
    }

    private void UpdatePlayerUI()
    {
        //print("Updating player UI!");
        for (int i = 0; i < uiCorners.Length + 1; i++)
        {
            
            if (PlayersScores.ContainsKey(i+1))
            {
                //print("Setting player Active: " + i);
                uiCorners[i].SetActive(true);
            }
        }
    }

    public void DebugAddScore( int player)
    {
        scores[player] += DEBUGVAL;
    }

    public void DebugRemoveScore(int player)
    {
        scores[player] -= DEBUGVAL;
    }
}
