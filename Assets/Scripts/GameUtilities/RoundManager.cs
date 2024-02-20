using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Build.Content;

public class RoundManager : MonoBehaviour
{
    // Create a dictionary to store points for each player for the current round
    private Dictionary<int, int> playerPoints = new Dictionary<int, int>();


    // additional dictionary that stores total points and gets added to each round ( see accumulate total points)
    private Dictionary<int, int> playerTotalScores = new Dictionary<int, int>(); 

    private int roundsRemaining; // rounds left in the game
    private int currentRound; // current round Number
    private bool roundEnded; // is the round over
    private bool isDrawRound; // is the current round a draw round
    private bool scoreTied; // is there a tied score 
    //public GameUtils gameUtilityScript; // the script that has a function for getting the active players count
    [SerializeField] private GameObject ResultsPanel;
    public SceneChanger sceneChangingScript;

    void Start()
    {
        sceneChangingScript = new SceneChanger();
        ResultsPanel.SetActive(false);
        //gameUtilityScript = FindObjectOfType<GameUtils>();
    }

    void Update()
    {
    
        if (ResultsPanel.activeSelf) // if the player is looking at the results of the round (player scores) and they press a button 
        {
            
            if (Input.GetKeyDown(KeyCode.A)) // placeholder input (CHANGE ME)
            {
                if (isDrawRound)
                {
                    // if the next round should be a draw round, load the draw round scene
                    sceneChangingScript.loadChosenSceneWithDelay("Draw");
                }

                else
                {
                    //if the next round should be a regular round // load the round scene
                    sceneChangingScript.loadChosenSceneWithDelay("Round");
                }
            }
        }
    }

    public void ManageRounds()
    {
       
        if(PlayerPrefs.GetInt("TotalRounds") !=0)
        {
            roundsRemaining = PlayerPrefs.GetInt("TotalRounds");
        }

        else
        {
           // var numberOfRemainingPlayers = gameUtilityScript.GetActivePlayers().Count;
            //roundsRemaining = numberOfRemainingPlayers - 1;
        }

        if (PlayerPrefs.GetInt("CurrentRoundNumber") != 0)
        {
            currentRound = PlayerPrefs.GetInt("CurrentRoundNumber") + 1;
        }

        if (currentRound > roundsRemaining)
        {
            //Go to end game logic
          //  SceneManager.LoadScene("");
        }
    }



    public int GetPointsForPlayers(int playerID)
    {
        if (playerTotalScores.ContainsKey(playerID))
        {
            return playerTotalScores[playerID];
        }
        else
        {
            return 0;
        }
    }
    private void AccumulateTotalScores()
    {
        // Loop through the current round scores and update the total scores dictionary
        foreach (var kvp in playerPoints)
        {
            int playerID = kvp.Key;
            int roundScore = kvp.Value;

            // If the player is not in the total scores dictionary, add them with the round score
            if (!playerTotalScores.ContainsKey(playerID))
            {
                playerTotalScores[playerID] = roundScore;
            }
            else
            {
                // If the player is already in the total scores dictionary, update their total score
                playerTotalScores[playerID] += roundScore;
            }
        }
    }

    public string CompareScores()
    {
        List<string> activePlayers;
        activePlayers = PlayerPrefs.GetString("RemainingPlayers").Split(',').ToList();

        // initialize variables to track lowest score and respective player
        int lowestScore = int.MaxValue; // Initialize with a value higher than the possible scores.
        string playerWithLowestScore = "";

        //list to store players with identical scores
        List<int> playersWithIdenticalLowestScores = new List<int>();

        foreach (int playerID in playerPoints.Keys)
        {
            int playerScore = GetPointsForPlayers(playerID);

            if (playerScore < lowestScore)
            {
                lowestScore = playerScore;
                playerWithLowestScore = playerID.ToString();

                // Reset the list of players with identical lowest scores.
                playersWithIdenticalLowestScores.Clear();
                playersWithIdenticalLowestScores.Add(playerID);
            }
            else if (playerScore == lowestScore)
            {
                // Add the player to the list of players with identical lowest scores.
                playersWithIdenticalLowestScores.Add(playerID);

                PlayerPrefs.SetString("DrawingPlayers", string.Join(",", playersWithIdenticalLowestScores));
            }
        }

        if (playersWithIdenticalLowestScores.Count >= 2)
        {
            PlayerPrefs.SetString("isDraw", "true");
            
            isDrawRound = true;
            return "Tie among players: " + string.Join(", ", playersWithIdenticalLowestScores);
        }
    
        else
        {
            PlayerPrefs.SetString("isDraw", "false");
            // Return the playerID with the lowest score.
            return playerWithLowestScore;
        }
    }



    public void EndRound()
    {
        AccumulateTotalScores(); // add totals before comparing

        CompareScores(); // check total scores for a loser or a draw

        if (!isDrawRound) 
        {
            // if theres no draw, eliminate the losing player 
            EliminateLowestScorer();
            PushRoundData();
            ManageRounds();
        }

        else
        {
            // add draw logic here to create the list of drawn players and ready up the next round (see void update )

            
        }

      roundEnded = true; // mark that the round is ended 
      ResultsPanel.SetActive(true); // activate the results panel so the players can see their scores
    }


    public void EliminateLowestScorer()
    {
        string playerWithLowestScore = CompareScores(); // Get the player with the lowest score.
        // Check that there is no tied scores and if so, remove the player with the lowest score from the list of remaining players.
        if (playerWithLowestScore != "" && !scoreTied)
        {
            List<string> activePlayers = PlayerPrefs.GetString("RemainingPlayers").Split(',').ToList();
            activePlayers.Remove(playerWithLowestScore);

            PlayerPrefs.SetString("RemainingPlayers", string.Join(",", activePlayers)); // Update PlayerPrefs with the modified active players list.
        }
    }

    private void PushRoundData()
    {
        Debug.Log("Pushing Info!");
        PlayerPrefs.SetInt("TotalRounds", roundsRemaining);
        PlayerPrefs.SetInt("CurrentRoundNumber", currentRound);
    }

}

