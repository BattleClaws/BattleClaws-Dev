using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuLogic : MonoBehaviour
{
    // This script is tracking the player's selection of custom mode options.
    // It's handling both the number of rounds UI buttons and the timer length buttons in two different functions
    // so it can keep the relevant selection highlighted.

    private int customTimerLength;
    private int customNumberOfRounds;
    public Button[] roundButtons; // Array of buttons for number of rounds
    public Button[] timerButtons; // Array of buttons for timer length

    public Color defaultColor;
    public Color selectedColor;
    private Button selectedRoundButton;
    private Button selectedTimerButton;

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
}