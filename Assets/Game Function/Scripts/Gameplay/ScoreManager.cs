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
        var allPlayers = FindObjectsOfType<PlayerController>();
        allPlayers.ToList().ForEach(player=> player.Properties.LegacyPoints +=  player.Properties.Points);
        var FinalScores = FindObjectsOfType<PlayerController>().OrderBy(p => p.Properties.LegacyPoints).Reverse();

        try
        {
            string Winners = "";
            if (RoundManager.gameStyle == GameType.BestOf)
            {
                //Winner = FindObjectsOfType<PlayerController>().OrderBy(x => x.Properties.sessionWins).Last();
                var highestWins = FindObjectsOfType<PlayerController>().OrderBy(x => x.Properties.sessionWins).Last().Properties
                    .sessionWins;
                FindObjectsOfType<PlayerController>().Where(x => x.Properties.sessionWins == highestWins).ToList()
                    .ForEach(x => Winners += "Player " + x.Properties.PlayerNum + "\n");
            }
            else
            {
                Winners += "Player "+ FindObjectsOfType<PlayerController>().First(p => !p.Properties.eliminated).Properties.PlayerNum;
            }
            
            GameObject.Find("Winner").GetComponent<TMP_Text>().text = Winners;
        }
        catch (Exception e)
        {
            GameObject.Find("Winner").GetComponent<TMP_Text>().text = "NO WINNER!";
        }
        

        string playerScores = "";

        if (RoundManager.gameStyle == GameType.BestOf)
        {
            FinalScores.ToList().ForEach(sc => playerScores += "PLAYER" + sc.Properties.PlayerNum + ":  "
                                                               + sc.Properties.sessionWins.ToString() + "/" + RoundManager.roundAmount + " Wins" + "\n");
        }
        else {
            FinalScores.ToList().ForEach(sc => playerScores += "PLAYER" + sc.Properties.PlayerNum + "  "
                                                               + sc.Properties.LegacyPoints.ToString().PadLeft(6, '0') + "\n");
        }
            

        
        GameObject.Find("Scores").GetComponent<TMP_Text>().text = playerScores;
    }
}
