using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReadyUp : MonoBehaviour
{
  
    public static int NumberOfReadyPlayers;
    public TextMeshProUGUI readyStatusText;
    public GameObject MorePlayersBanner;

    // ray to hold ref to light status scripts (see game controller object)
    public lightStatus[] playerLightScripts;
    
    // Singleton yeehaw
    public static ReadyUp Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        NumberOfReadyPlayers = 0;
        MorePlayersBanner.SetActive(true);
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

    public void ShowPlayerReadyStatus(int playerNumber, bool isReady) // Controls the status of the Ready Up lights yippee
    {
        if (playerLightScripts != null && playerNumber > 0 && playerNumber <= playerLightScripts.Length)
        {
            // Set the light status based on the player's ready up state
            playerLightScripts[playerNumber - 1].status = isReady ? 2 : 0; // Light status int is set to 2 if they're ready, and 0 if not
           
        }
    }

    void Update()
    {
        if (Player.amountOfPlayers >= 2)
        {
            MorePlayersBanner.SetActive(false);
            readyStatusText.text = NumberOfReadyPlayers.ToString() + "/" + Player.amountOfPlayers.ToString();
        }

      
        
    }



    
    
}
