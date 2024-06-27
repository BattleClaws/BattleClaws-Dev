using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuLogic : MonoBehaviour
{
    // This script is tracking the player's selection of custom mode options.
    // It's handling both the number of rounds UI buttons and the timer length buttons in two different functions
    // so it can keep the relevant selection highlighted.
    
    //this script also contains the full screen option manager 
    // fullScreenButtons[0]: This is the button for turning FullScreen ON.
    //fullScreenButtons[1]: This is the button for turning FullScreen OFF.

    // custom mode setting variables 
    private int customTimerLength;
    private int customNumberOfRounds;
    
    // button arrays (this is related to keeping the selected option highlighted pink 
    public Button[] roundButtons; // Array of buttons for number of rounds
    public Button[] timerButtons; // Array of buttons for timer length
    public Button[] fullScreenButtons; // Array of buttons for FullScreen (0: ON, 1: OFF)
    private bool fullScreenActive;
    public Color defaultColor;
    public Color selectedColor;
    private Button selectedRoundButton;
    private Button selectedTimerButton;
    
    // volume setting variables
    public Slider volumeSlider;
    private TextMeshProUGUI volumeText;
    

    // Start is called before the first frame update
    void Start()
    {
        // Initialize all buttons to default color and add listeners
        foreach (Button button in roundButtons)
        {
            button.onClick.AddListener(() => OnRoundButtonClick(button));
            button.GetComponent<Image>().color = defaultColor;
        }

        foreach (Button button in timerButtons)
        {
            button.onClick.AddListener(() => OnTimerButtonClick(button));
            button.GetComponent<Image>().color = defaultColor;
        }
        
            fullScreenButtons[0].onClick.AddListener(() => SetFullScreen(true)); // ON button
            fullScreenButtons[1].onClick.AddListener(() => SetFullScreen(false)); // OFF button
            UpdateFullScreenButtons();
            
            // Initialize volume slider
            volumeSlider.onValueChanged.AddListener(OnVolumeSliderChange);
            volumeSlider.value = AudioListener.volume; // Set initial slider value to current audio volume
        

        DefaultCustomOptions();
    }

    public void ApplySelectionsAndProceed()
    {
        // Jess! use this function to apply the player's selections to the Round Management logic. 
        // This function gets called when the player clicks "Start" Button on the Custom Mode UI menu. 
        // apply settings
        // transition into player customization scene
    }

    public void DefaultCustomOptions() // called on Start, sets Custom Mode Options to default
    {
        // reference the default selected buttons
        Button defaultSelectedRoundButton = roundButtons[0];
        Button defaultSelectedTimerButton = timerButtons[1];

        selectedRoundButton = defaultSelectedRoundButton;
        selectedTimerButton = defaultSelectedTimerButton;
        
        selectedRoundButton.GetComponent<Image>().color = selectedColor;
        selectedTimerButton.GetComponent<Image>().color = selectedColor;
        
        // store the default custom mode settings
        //  3 rounds 
        StoreChosenSetting(3);
        // 60 second timer
        StoreChosenSetting(60);
    }

    public void StoreChosenSetting(int buttonValue)
    {
        if (buttonValue <= 10) // Maximum round option is 8
        {
            customNumberOfRounds = buttonValue;
        }

        if (buttonValue >= 10) // Minimum timer option is 30
        {
            customTimerLength = buttonValue;
        }

        Debug.Log("Rounds: " + customNumberOfRounds + " Timer Length: " + customTimerLength); // For testing
    }

    void OnRoundButtonClick(Button clickedButton)
    {
        if (selectedRoundButton != null)
        {
            // Reset the color of the previously selected button
            selectedRoundButton.GetComponent<Image>().color = defaultColor;
        }

        // Highlight the clicked button
        clickedButton.GetComponent<Image>().color = selectedColor;

        // Update the selected button reference
        selectedRoundButton = clickedButton;
    }

    void OnTimerButtonClick(Button clickedButton)
    {
        if (selectedTimerButton != null)
        {
            // Reset the color of the previously selected button
            selectedTimerButton.GetComponent<Image>().color = defaultColor;
        }

        // Highlight the clicked button
        clickedButton.GetComponent<Image>().color = selectedColor;

        // Update the selected button reference
        selectedTimerButton = clickedButton;
    }

   public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        fullScreenActive = isFullScreen;
        UpdateFullScreenButtons();
    }

    void UpdateFullScreenButtons()
    {
        if (fullScreenActive)
        {
            fullScreenButtons[0].GetComponent<Image>().color = selectedColor; // ON button is highlighted pink
            fullScreenButtons[1].GetComponent<Image>().color = defaultColor; // OFF button is defaulted to grey
        }
        else
        {
            fullScreenButtons[0].GetComponent<Image>().color = defaultColor; // ON button // is defaulted to grey
            fullScreenButtons[1].GetComponent<Image>().color = selectedColor; // OFF button // is highlighted pink
        }
        
        Debug.Log("FullScreen Status " + fullScreenActive );
    }
    
    public void OnVolumeSliderChange(float value)
    {
        AudioListener.volume = value;
        volumeText = volumeSlider.GetComponentInChildren<TextMeshProUGUI>();
        volumeText.text = value.ToString();
        PlayerPrefs.SetFloat("Volume", value); // save the value to the PlayerPrefs;
    }
    
}
