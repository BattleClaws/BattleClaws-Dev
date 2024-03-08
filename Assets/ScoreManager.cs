using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        GetFinalScores();
    }
    
    void GetFinalScores()
    {
        var FinalScores = FindObjectsOfType<PlayerController>().OrderBy(p => p.Properties.LegacyPoints).Reverse();

        try
        {
            var Winner = FindObjectsOfType<PlayerController>().First(p => !p.Properties.eliminated);
            GameObject.Find("Winner").GetComponent<TMP_Text>().text = "PLAYER" + Winner.Properties.PlayerNum;
        }
        catch (Exception e)
        {
            GameObject.Find("Winner").GetComponent<TMP_Text>().text = "NO WINNER!";
        }
        

        string playerScores = "";
        FinalScores.ToList().ForEach(sc => playerScores += "PLAYER" + sc.Properties.PlayerNum + "  " 
                                                           + sc.Properties.LegacyPoints.ToString().PadLeft(6, '0') + "\n");

        
        GameObject.Find("Scores").GetComponent<TMP_Text>().text = playerScores;
    }
}
