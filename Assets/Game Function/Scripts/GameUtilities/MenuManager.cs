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
        SetCurrentScreen(customizableMenu);
        customMenuScript = customizableMenu.GetComponent<CustomMenu>();
        
       

        switch (Context)
        {
            case "Quit": // initial quit button on the main settings menu 
                
                customMenuScript.setCustomHeaderAndSubHeader("Quit the Game?", "You will lose any current progress");
                customMenuScript.AssignAction(() => QuitGameToMainMenu(), "Quit to Main Menu"); // Quits to Splash
                customMenuScript.AssignAction(() => QuitGame(), "Quit the Game"); // Closes the Application
                customMenuScript.AssignAction(()=> OnBackPressed(), "Back");
              
                break;
        }
    }
    
    
    public void QuitGame()
    {
        Application.Quit();
    }

    public void QuitGameToMainMenu()
    {
        OnBackPressed();
        SetVisibility(false);
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
        var settingsScreen = currentScreen.GetComponent<SettingsScreen>(); // get the right settings script for the currently active screen 
    
        if (settingsScreen != null && settingsScreen.previousPage != null)
        {
            settingsScreen.previousPage.SetActive(true); // activate the root settings menu screen 
            currentScreen.SetActive(false); // set the custom menu object to false 
            settingsScreen.previousPage = currentScreen; // root settings menu becomes current screen 
           

        }
        else
        {
           // no previous page - close the menu 
            SetVisibility(false);
           
        }
        
        customMenuScript.ClearAllAssignedActions(); // deletes the spawned custom buttons and their assigned actions
  
        
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
