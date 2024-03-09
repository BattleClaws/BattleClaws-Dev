using System.Collections;
using System.Collections.Generic;
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
       
    }

    public void OnDrop(InputAction.CallbackContext ctx)
    {
        StartCoroutine(loadChosenSceneWithDelay(destinationSceneName));
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
