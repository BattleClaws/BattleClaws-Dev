using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    private static int currentRoundNumber;
    private static int maxNumberOfRounds;
    private static bool roundEnded;
    private GameObject endRoundPanel;


    private void Start()
    {
        DontDestroyOnLoad(gameObject); // preserve this object across scenes and all static variables 
        endRoundPanel.SetActive(false);


        // get the round number from PP
        currentRoundNumber = 0;

        //if there's nothing in the PP for RoundNumber (it returns 0)
        if (currentRoundNumber == 0)
        {
            currentRoundNumber = 1; // note that its the first round
            PlayerPrefs.SetInt("RoundNumber", 1); // update the PP again 
        }

        maxNumberOfRounds = Player.amountOfPlayers - 1;
    }

    private void Update()
    {
        if (roundEnded) // Have the timer logic set roundEnded to true to end the round
            EndRound();
    }

    public void EndRound()
    {
        EndPlayers();
        currentRoundNumber++;
    }

    public void EndPlayers()
    {
        // Get all active players and order them by score.
        var activePlayers = FindObjectsByType<PlayerController>(FindObjectsSortMode.None)
            .Where(player => player.eliminated == false).ToList();
        var playersByScore = activePlayers.OrderBy(player => player.Properties.Points).ToList();

        var playerToElim = playersByScore.Last();

        if (playerToElim.Properties.Points == playersByScore[-2].Properties.Points)
        {
            // do draw logic
        }

        activePlayers.ForEach(player => player.Properties.RoundReset());
        Destroy(playerToElim.gameObject);
    }


    public void DisplayScores()
    {
        endRoundPanel.SetActive(true);
    }

    public void ReadyNextRound()
    {
        if (currentRoundNumber > maxNumberOfRounds) SceneManager.LoadScene("EndGame");
        // reload the scene and other stuff for next round
    }
}