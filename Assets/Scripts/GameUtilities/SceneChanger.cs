using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.XR;
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
       
        currentSceneName = SceneManager.GetActiveScene().name;
        if(currentSceneName == "Splash")
        {
            audioScript = audioHolder.GetComponent<AudioManager>();
            isSplash = true;
           
        }
        if(currentSceneName == "Credits")
        {
            isCredits = true;
        }

        if(currentSceneName == "EndGame")
        {
            isGameOver = true;
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
        PlayerController[] playersToDestroy = FindObjectsOfType<PlayerController>();
       foreach(PlayerController player in playersToDestroy)
        {
            Destroy(player);
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
