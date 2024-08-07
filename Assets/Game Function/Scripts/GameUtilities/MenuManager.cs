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
        print("Focused");
        if (!hasFocus)
        {
            SetVisibility(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            print("Reenter");

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            EventSystem.current.SetSelectedGameObject(currentScreen.GetComponent<SettingsScreen>().firstSelected);
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        print("Paused");
        if (pauseStatus)
        {
            SetVisibility(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            print("Reenter");
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

    public void LaunchCustomMenu()
    {
        // call me during EndGame after viewing the final scores
        
        customizableMenu.SetActive(true);
        customMenuScript = customizableMenu.GetComponent<CustomMenu>();
        // assign the menu header
        customMenuScript.setCustomHeader("Game Complete!");
        
        // assign the primary button
        // Wrap Rematch in a lambda expression to match UnityAction
        customMenuScript.AssignAction1(() => Rematch(), "REMATCH");
        
        // assign the secondary button
        customMenuScript.AssignAction2(()=>MainMenu(), "Main Menu");
        
        // assign the tertiary button
        customMenuScript.AssignAction3(() => QuitGame(), "Quit Game");
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
        SceneManager.LoadScene("GameSelect");
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
