using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Build.Content;
using System.Linq;

public class RoundManager : MonoBehaviour
{
    private static  int currentRoundNumber;
    private static int maxNumberOfRounds;
    private static bool roundEnded;
    
    // Create a dictionary to store points for each player for the current round
    private static Dictionary<int, int> playerPointsThisRound = new Dictionary<int, int>();

    // additional dictionary that stores total points and gets added to each round ( see accumulate total points)
    private static Dictionary<int, int> playerTotalScores = new Dictionary<int, int>();

    private GameObject endRoundPanel;
    

    

    void Start()
    {
        DontDestroyOnLoad(gameObject); // preserve this object across scenes and all static variables 
        endRoundPanel.SetActive(false);
        

        // get the round number from PP
        currentRoundNumber = PlayerPrefs.GetInt("RoundNumber");

        //if there's nothing in the PP for RoundNumber (it returns 0)
        if(currentRoundNumber == 0)
        {
            // initialize player scores to 0 by getting the active players
            List<PlayerController> activePlayers = FindObjectsByType<PlayerController>(FindObjectsSortMode.None).ToList();
            foreach(PlayerController player in activePlayers)
            {
                playerTotalScores[player.Properties.PlayerNum] = 0;
            }

            currentRoundNumber = 1; // note that its the first round
            PlayerPrefs.SetInt("RoundNumber", 1); // update the PP again 
        }

        maxNumberOfRounds = Player.amountOfPlayers - 1;
    }

    void Update()
    {
        if (roundEnded) // Have the timer logic set roundEnded to true to end the round
        {
            EndRound();
        }
       
    }

    public void EndRound()
    {

        ManagePoints(); // tally up round and total points
        IncreaseRoundNumber();


    }



    public void ManagePoints()
    {
        //get the list of active players at the end of the round
        List<PlayerController> activePlayers = FindObjectsByType<PlayerController>(FindObjectsSortMode.None).ToList();
        // for each active player, add up their points this round, and then add that to their total points across the game so far
        foreach (PlayerController player in activePlayers)
        {
            playerPointsThisRound.Add(player.Properties.PlayerNum, player.Properties.Points);

            playerTotalScores[player.Properties.PlayerNum] += player.Properties.Points;
        }
    }

    public void DisplayScores()
    {
        endRoundPanel.SetActive(true);
        
        
    }

    public void IncreaseRoundNumber()
    {
        currentRoundNumber++;
        PlayerPrefs.SetInt("RoundNumber", currentRoundNumber); 
    }

    public void ReadyNextRound()
    {
        if(currentRoundNumber > maxNumberOfRounds)
        {
            SceneManager.LoadScene("EndGame");
        }

        else
        {
            // reload the scene and other stuff for next round
        }
    }






    private void PushRoundData()
    {
        Debug.Log("Pushing Info!");
       // PlayerPrefs.SetInt("TotalRounds", roundsRemaining);
       // PlayerPrefs.SetInt("CurrentRoundNumber", currentRound);
    }

}

