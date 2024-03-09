using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string destinationSceneName;
    [SerializeField] public GameObject audioHolder;
    private AudioManager audioScript;

    private string currentSceneName;
    private bool isSplash = false;
    private bool isCredits = false;
    private bool isGameOver = false;

    public PlayerController playerScripts;
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Splash")
        {
            ResetGame();
        }

        currentSceneName = SceneManager.GetActiveScene().name;
        if(currentSceneName == "Splash")
        {
            audioScript = audioHolder.GetComponent<AudioManager>();
        }

    }

    public void OnDrop(InputAction.CallbackContext ctx)
    {
        if(isGameOver)
        {
            ResetGame();
        }
        StartCoroutine(loadChosenSceneWithDelay(destinationSceneName));
    }

    public void ResetGame()
    {
        RoundManager.currentRoundNumber = 0;
        Player.amountOfPlayers = 0;
        PlayerController[] playersToDestroy = FindObjectsOfType<PlayerController>();
       foreach(PlayerController player in playersToDestroy)
        {
            print("Killed player " + player.Properties.PlayerNum);
            Destroy(player.gameObject);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            StartCoroutine(loadChosenSceneWithDelay(destinationSceneName));
        }


    }

    public IEnumerator loadChosenSceneWithDelay(string SceneName) 
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(destinationSceneName);
    }

   
}
