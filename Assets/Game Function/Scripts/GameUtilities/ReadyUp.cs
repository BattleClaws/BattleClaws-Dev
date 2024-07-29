using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReadyUp : MonoBehaviour
{
  
    public static int NumberOfReadyPlayers;
    
    
    // Start is called before the first frame update
    void Start()
    {
        NumberOfReadyPlayers = 0;
    }

    public static void UpdateReadiedPlayersCount()
    {
        NumberOfReadyPlayers++; 
        
        if (NumberOfReadyPlayers > 1 && NumberOfReadyPlayers >= Player.amountOfPlayers) // need at least 2 players to begin
        {
            Debug.Log("Starting Game with " + NumberOfReadyPlayers + " Ready Players");
            SceneManager.LoadScene("Round");
        }
    }



    
    
}
