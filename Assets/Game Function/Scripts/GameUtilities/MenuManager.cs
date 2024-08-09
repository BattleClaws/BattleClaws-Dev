using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
   
    [Header("Menu Fields")] public GameObject Base;
    public GameObject Root;
    public GameObject audioSettings;
    public GameObject graphicsSettings;
    public GameObject controlsSettings;
    public GameObject customizableMenu;
    public CustomMenu customMenuScript; // contains the logic for custom menu 

    [Header("Settings Sliders")] public Slider Volume;
    public Slider Contrast;
    public Slider Brightness;

    [Space] [Header("Requirements")] public AudioMixer masterVolumeMixer;
    public AudioMixer EffectsVolumeMixer;
    
    public PostProcessProfile postProcessingProfile;
    private AutoExposure exposure;
    private ColorGrading colorGrading;
    public TMP_Text textSliderLabel;
    public TMP_Text contrastSliderLabel;

    private GameObject previousHighlightedItem;

    [Header("Misc.")] public GameObject currentScreen;


 
    private void OnApplicationFocus(bool hasFocus)
    {
        //print("Focused");
        if (!hasFocus)
        {
            SetVisibility(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            //print("Reenter");

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            EventSystem.current.SetSelectedGameObject(currentScreen.GetComponent<SettingsScreen>().firstSelected);
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        //print("Paused");
        if (pauseStatus)
        {
            SetVisibility(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            //print("Reenter");
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            EventSystem.current.SetSelectedGameObject(currentScreen.GetComponent<SettingsScreen>().firstSelected);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(currentScreen.GetComponent<SettingsScreen>().firstSelected);
        }


    }

    public void LaunchCustomMenu(string Context)
    {
        customizableMenu.SetActive(true);
        customMenuScript = customizableMenu.GetComponent<CustomMenu>();

        switch (Context)
        {
            case "EndGame": // After Pressing A when viewing the Final Scores in the End Game Scene

            // hide any  unneeded button(s)
            customMenuScript.action3.gameObject.SetActive(false);   // hide action 3
            customMenuScript.returnButton.gameObject.SetActive(false); // hide the return button
            // assign the menu header
            customMenuScript.setCustomHeaderAndSubHeader("Game Complete!", "");

            // assign the primary button
            customMenuScript.AssignAction(customMenuScript.action1,() => Rematch(), "REMATCH");

            // assign the secondary button
            customMenuScript.AssignAction(customMenuScript.action2,() => MainMenu(), "Main Menu");
            
            
            break;
        
            case "MainMenu": // After selecting the "Main Menu" option on the Settings / Pause Menu
                
            SetCurrentScreen(customizableMenu);
            // hide any  unneeded button(s)
            customMenuScript.action3.gameObject.SetActive(false);   // hide action 3
            customMenuScript.returnButton.gameObject.SetActive(false); // hide the return button
            
            customMenuScript.setCustomHeaderAndSubHeader("Quit to Main Menu?", "You will lose all current progress");
            
            // assign the Primary Button
            customMenuScript.AssignAction(customMenuScript.action1,()=> OnBackPressed(),"NO");
            
            // assign the Secondary button 
            customMenuScript.AssignAction(customMenuScript.action2,() => MainMenu(), "YES");
            
            break;
            
            case "QuitApp": // when the player chooses the Quit Button on the Game Selection Screen
                
                SetCurrentScreen(customizableMenu);
                // hide any  unneeded button(s)
                customMenuScript.action3.gameObject.SetActive(false);   // hide action 3
                customMenuScript.returnButton.gameObject.SetActive(false); // hide the return button
            
                customMenuScript.setCustomHeaderAndSubHeader("Exit the Game?", "The Claws will miss you");
            
                // assign the Primary Button
                customMenuScript.AssignAction(customMenuScript.action1,()=> OnBackPressed(),"NO");
            
                // assign the Secondary button 
                customMenuScript.AssignAction(customMenuScript.action2,() => QuitGame(), "YES");
                break;
            
        }

    }

    public void Rematch()
    {
        // this function should begin a new game with the same gamemode,
        // number of players, customization options and round settings. 
        SceneManager.LoadScene("Round");
    }

  

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu() // mode selection
    {
        OnBackPressed();
        SceneManager.LoadScene("Splash");
    }

    private void Awake()
    {
        if(GameUtils.instance)
            GameUtils.instance.audioPlayer = FindObjectOfType<AudioManager>();
    }

    void Start()
    {
     
        // This is hacky. Remove later
        RoundManager.draw = false;
        
        
        DontDestroyOnLoad(gameObject);

        postProcessingProfile.TryGetSettings(out exposure);
        postProcessingProfile.TryGetSettings(out colorGrading);

        float volume;
        masterVolumeMixer.GetFloat("MasterVol", out volume);
        Volume.value = volume;
        
        Contrast.value = colorGrading.contrast.value;
        Brightness.value = exposure.keyValue.value;

        currentScreen = Root;
        
        SetVisibility(false);
    }

    public void SetCurrentScreen(GameObject newScreen)
    {
        currentScreen = newScreen;
    }

    public void SetVisibility(bool value)
    {
        GameUtils.isMenuOpen = value;
        if (Base.activeSelf == value)
            return;

        if (value)
        {
            Time.timeScale = 0;
            previousHighlightedItem = EventSystem.current.currentSelectedGameObject;
        }
        else
        {
            Time.timeScale = 1;
            EventSystem.current.SetSelectedGameObject(previousHighlightedItem);
        }

        if (Base.activeSelf)
        {
            audioSettings.SetActive(false);
            controlsSettings.SetActive(false);
            graphicsSettings.SetActive(false);
        }

        Base.SetActive(value);
        Root.SetActive(value);
        currentScreen = Root;
    }

    public void OnBackPressed()
    {
        if (currentScreen == customizableMenu)
        {
            customMenuScript.ClearAllAssignedActions();
            currentScreen.SetActive(false);
            currentScreen = FindObjectOfType<SettingsScreen>().gameObject;
           
            
        }
        
        if(Base.activeSelf && currentScreen.GetComponent<SettingsScreen>().previousPage != null)
            currentScreen.GetComponent<SettingsScreen>().Back();
        else if (currentScreen.GetComponent<SettingsScreen>().previousPage == null)
        {
            SetVisibility(false);
        }
    }

    public void OnVolumeChange(System.Single value) 
    {
        masterVolumeMixer.SetFloat("MasterVol", value);
        textSliderLabel.text = value.ToString();
    }
    
    public void OnEffectsChange(System.Single value) 
    {
        EffectsVolumeMixer.SetFloat("SoundEffectsVol", value);
    }

    public void OnBrightnessChange(System.Single value)
    {
        exposure.keyValue.value = value;
    }
    
    public void OnContrastChange(System.Single value)
    {
        colorGrading.contrast.value = value;
    }

    public void ToggleFullScreen(bool isOn)
    {
        Screen.fullScreen = isOn;
        
    }
    
    
    
}
