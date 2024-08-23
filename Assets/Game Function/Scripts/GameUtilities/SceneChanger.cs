using System;
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

    [Space] [Header("Delay Options")] public bool delayInput = false;
    public float delayTime;
    public GameObject continuePrompt;

    [Space] [Header("Sound Options")] public bool playSound;
    public string clipName;
    
    

    private string currentSceneName;
    private bool isSplash = false;
    private bool isCredits = false;
    private bool isGameOver = false;
    [Space]
    public PlayerController playerScripts;

    public MenuManager menuManagerScript;
    private bool isEndMenuOpen;
    private void Start()
    {
        if (delayInput && continuePrompt != null)
        {
            continuePrompt.SetActive(false);
        }

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
            //print("Killed player " + player.Properties.PlayerNum);
            Destroy(player.gameObject);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        delayTime -= Time.deltaTime;
        if (delayTime <= 0 && continuePrompt != null)
        {
            continuePrompt.SetActive(true);
        }

        if ((delayInput && delayTime <= 0 && Input.GetButtonDown("Submit")) || (!delayInput && Input.GetButtonDown("Submit")))
        {
            if (!isEndMenuOpen && currentSceneName == "EndGame")
            {
                OpenEndGameMenu();
            }

            if (currentSceneName != "EndGame")
            {
                print("Changing to: " + destinationSceneName);
                if(playSound)GameUtils.instance.audioPlayer.PlayChosenClip(clipName);
                StartCoroutine(loadChosenSceneWithDelay(destinationSceneName));
            }

        }


    }

    public IEnumerator loadChosenSceneWithDelay(string SceneName)
    {
        if (!String.IsNullOrEmpty(SceneName))
        {
            yield return new WaitForSeconds(0.3f);
            destinationSceneName = SceneName;
            SceneManager.LoadScene(destinationSceneName);
            print("Changing to: " + destinationSceneName);
            destinationSceneName = null;
        }
    }

    public void changeSceneViaUI(string sceneString)
    {
        if (GameUtils.isMenuOpen && sceneString != "Credits") return;
        
        destinationSceneName = sceneString;
        StartCoroutine(loadChosenSceneWithDelay(destinationSceneName));
    }

    public void OpenEndGameMenu()
    {
        menuManagerScript = FindObjectOfType<MenuManager>();
        if (menuManagerScript != null)
        {
            menuManagerScript.LaunchCustomMenu("GameComplete");
            isEndMenuOpen = true;
        }
    }

   
}
