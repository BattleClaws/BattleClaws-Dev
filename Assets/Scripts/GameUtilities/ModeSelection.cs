using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ModeSelection : MonoBehaviour
{
    // Voting variables
    private int maximumVotesPossible;
    private int votesRequiredToProceed;
    private Dictionary<string, int> votesCount = new Dictionary<string, int>(); // OptionName -> NumberOfVotes
    private string selectedOption;

    // Chosen custom values (based on voting outcomes)
    private int customNumberOfRounds;
    private int customRoundLength;

    //signposting variables
    public Animator playerSelectAnim; // Assigned via the Unity Editor
    public Slider loadingBarSlider;
    public TextMeshProUGUI statusText;

    void Start()
    {
        statusText.text = "Vote for your Game Mode!";
        Debug.Log(votesCount);

    }

    // Record a vote for the specified option
    public void RecordVote(string optionName)
    {
        if (!votesCount.ContainsKey(optionName))
        {
            votesCount[optionName] = 0; // Initialize count if not exists
        }
        votesCount[optionName]++;
        statusText.text = "Votes for " + optionName + ": " + votesCount[optionName];
    }

    // Remove a vote for the specified option
    public void RemoveVote(string optionName)
    {
        if (votesCount.ContainsKey(optionName))
        {
            votesCount[optionName]--;
            if (votesCount[optionName] <= 0)
            {
                votesCount.Remove(optionName);
            }
        }
        
      else  if (!votesCount.ContainsKey(optionName))
      {
          votesCount[optionName] = 0;
      }
        statusText.text = "Votes for " + optionName + ": " + votesCount[optionName];
    }

    // Tally the votes and select the option if it has enough votes
    public void TallyVotes()
    {
        maximumVotesPossible = Player.amountOfPlayers;
        votesRequiredToProceed = 3;
        
            foreach (var option in votesCount)
            {
                if (option.Value >= votesRequiredToProceed)
                {
                    Debug.Log(option.Key + " Selected!");
                    selectedOption = option.Key;
                    BeginStartCountdown();
                }
            }
        
    }

    private void BeginStartCountdown()
    {
        statusText.text = "Starting Game";
        loadingBarSlider.value = 0;
        InvokeRepeating("FillSlider", 0.1f, 0.05f);

    }
    
    private void FillSlider()
    {
        loadingBarSlider.value = Mathf.Clamp(loadingBarSlider.value + 1, loadingBarSlider.minValue, loadingBarSlider.maxValue);

        if (loadingBarSlider.value >= loadingBarSlider.maxValue)
        {
            ApplySelection(selectedOption);
            CancelInvoke("FillSlider");
            loadingBarSlider.value = 0;
        }
    }


    private void ApplySelection(string optionName)
    {
        switch (optionName)
        {
            case "Battle Mode":
                SceneManager.LoadScene("Round");
                
                break;

            case "Custom Mode":
                statusText.text = "CUSTOM MODE! Choose the number of rounds";
                playerSelectAnim.SetTrigger("NumberOptions");
                
                break;

            case "Custom 3 rounds":
                customNumberOfRounds = 3;
                playerSelectAnim.SetTrigger("LengthOptions");
                statusText.text = " 3 ROUNDS! Choose the length of each round";
                break;

            case "Custom 5 rounds":
                customNumberOfRounds = 5;
                playerSelectAnim.SetTrigger("LengthOptions");
                statusText.text = " 5 ROUNDS! Choose the length of each round";
                break;

            case "Custom 8 rounds":
                customNumberOfRounds = 8;
                statusText.text = " 8 ROUNDS! Choose the length of each round";
                playerSelectAnim.SetTrigger("LengthOptions");
                break;

            case "Custom 30 seconds":
                customRoundLength = 30;
                statusText.text = "Starting Game! Rounds: " + customNumberOfRounds + " Length: " + customRoundLength;
                break;

            case "Custom 60 seconds":
                customRoundLength = 60;
                statusText.text = "Starting Game! Rounds: " + customNumberOfRounds + " Length: " + customRoundLength;
                break;

            case "Custom 90 seconds":
                customRoundLength = 90;
                statusText.text = "Starting Game! Rounds: " + customNumberOfRounds + " Length: " + customRoundLength;
                break;
            
           
        }
        
        votesCount.Clear();
    }
}
