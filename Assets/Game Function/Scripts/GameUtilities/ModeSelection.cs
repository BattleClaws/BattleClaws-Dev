using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ModeSelection : MonoBehaviour
{

    public GameObject votingCollectable;
    public Vector3 collectablesSpawnPoint; // respawning collectables also happens in the Killzone manager script 
    
    // Voting variables
    private int maximumVotesPossible;
    private int votesRequiredToProceed;

    private int requiredReadyPlayers;
    private Dictionary<string, int> votesCount = new Dictionary<string, int>(); // OptionName -> NumberOfVotes
    private string selectedOption;

    // Chosen custom values (based on voting outcomes)
    private int customNumberOfRounds;
    private int customRoundLength;

    //signposting variables
    public Animator playerSelectAnim; // Assigned via the Unity Editor
    public Slider loadingBarSlider;
    public TextMeshProUGUI statusText;
    

    // Record a vote for the specified option
    public void RecordVote(string optionName)
    {
        if (!votesCount.ContainsKey(optionName))
        {
            votesCount[optionName] = 0; // Initialize count if not exists
        }
        votesCount[optionName]++;
      //  statusText.text = "Votes for " + optionName + ": " + votesCount[optionName]; Used for Testing Purposes
        TallyVotes();
    }

    // Remove a vote for the specified option
    public void RemoveVote(string optionName)
    {
        if (votesCount.ContainsKey(optionName))
        {
            votesCount[optionName]--;
            
        }
   
    }



    // Tally the votes and select the option if it has enough votes
    public void TallyVotes()
    {
        //maximumVotesPossible = Player.amountOfPlayers;
        requiredReadyPlayers = Player.amountOfPlayers;
        
            foreach (var option in votesCount)
            {
                if (option.Value >= votesRequiredToProceed)
                {
                    Debug.Log(option.Key + " Selected!");
                    selectedOption = option.Key;
                    StartCoroutine(ApplySelection(selectedOption));
                }
            }
        
    }

  
    // Method to delete all objects with the tag "Collectable" in between selections
    public IEnumerator DeleteCollectables()
    {
        GameObject[] collectables = GameObject.FindGameObjectsWithTag("Collectable");
        foreach (GameObject votingObject in collectables)
        {
            Destroy(votingObject);
        }

        yield return new WaitForSeconds(1);

        // Instantiate the deleted collectables with 0.5 seconds delay between each
        for (int i = 0; i < 2; i++)
        {
            Instantiate(votingCollectable, collectablesSpawnPoint, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }
    }
    


    private IEnumerator ApplySelection(string optionName)
    {
        //statusText.text = "Option Selected:  " + optionName; Used for Testing Purposes
         yield return new WaitForSeconds(1);
         
         
        switch (optionName)
        {
            case "Battle Mode":
                SceneManager.LoadScene("Round");
                
                break;

            case "Custom Mode":
                statusText.text = "Choose the number of rounds";
                playerSelectAnim.SetTrigger("NumberOptions");
                StartCoroutine(DeleteCollectables());
                
                break;

            case "Custom 3 rounds":
                customNumberOfRounds = 3;
                playerSelectAnim.SetTrigger("LengthOptions");
                statusText.text = "Choose the length of each round";
                StartCoroutine(DeleteCollectables());
                break;

            case "Custom 5 rounds":
                customNumberOfRounds = 5;
                playerSelectAnim.SetTrigger("LengthOptions");
                statusText.text = "Choose the length of each round";
                StartCoroutine(DeleteCollectables());
                break;

            case "Custom 8 rounds":
                customNumberOfRounds = 8;
                statusText.text = "Choose the length of each round";
                playerSelectAnim.SetTrigger("LengthOptions");
                StartCoroutine(DeleteCollectables());
                break;

            case "Custom 30 seconds":
                customRoundLength = 30;
                statusText.text = "Starting Custom Match!";
                break;

            case "Custom 60 seconds":
                customRoundLength = 60;
                statusText.text = "Starting Custom Match!";
                break;

            case "Custom 90 seconds":
                customRoundLength = 90;
                statusText.text = "Starting Custom Match!"; 
                break;
            
           
        }
        
        votesCount.Clear();
    }
}
