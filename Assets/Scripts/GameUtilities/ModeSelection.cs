using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;



public class ModeSelection : MonoBehaviour
{
    private int votesForThisMode; 
    private int possibleMaxVotes;
    private int NumReqForMajority;
    private Player playerCountScript; // attach logic later to get the active player count for determining max votes


    [SerializeField] private TextMeshProUGUI VoteDisplay; // text over the Mode Chute to indicate votes applied
    [SerializeField] private TextMeshProUGUI StatusText; // the text at the top of the screen
    [SerializeField] private GameObject SliderHolder; // the slider that controls the image fill for the logo
    [SerializeField] private bool ModeSelected; // flag for if the majority have players have selected a mode or not 
    [SerializeField] private GameObject particleObject;

    
    


    private void Start()
    {
        SliderHolder.SetActive(false); // turn the slider off on start
        particleObject.SetActive(false);
    }


    void Update()
    {
        if(ModeSelected) 
        {
            SliderHolder.SetActive(true); 
            ManageSlider();

            //TO DO: fix this
            ModeSelected = false; // DO NOT REMOVE! Or  update will trigger an invoke repeating every frame and you will explode. 
          
        }
    }

    public void OnTriggerEnter(Collider other) 
        
        // when a collision occurs with the chute for the chosen mode, increase votes and check for majority
    {
        if(other.CompareTag("Collectable"))
        {
            votesForThisMode++;
            VoteDisplay.text = ("Votes: " + votesForThisMode).ToString();
            checkVotes();
        }

    }

    public void OnTriggerExit(Collider other) // if a player removes their vote, minus the vote and check to see if there is still a majority 
    {
        if (other.CompareTag("Collectable"))
        {
            votesForThisMode--;
            VoteDisplay.text = ("Votes: " + votesForThisMode).ToString();
            checkVotes();
        }
    }

    public void checkVotes()
    {
        possibleMaxVotes = 1; // this should be _amountOfPlayers but that variable is set to private (CHANGE ME)
        NumReqForMajority = possibleMaxVotes / 2 + 1;

        if(votesForThisMode >= NumReqForMajority) // if any given mode has enough votes for a majority 
        {
            
            ModeSelected = true;
            SliderHolder.SetActive(true); 
            ManageSlider(); // update the slider value
        } 
        else
        {
            ModeSelected=false; 
            ManageSlider(); // reset the slider value 
        }

       
    }

    public void ManageSlider()
    {
        Slider BeginningSlider = SliderHolder.GetComponent<Slider>();
        if(ModeSelected)
        {
            StatusText.text = "Starting Game...";
            InvokeRepeating("FillSlider", 0.1f, 0.05f);
            particleObject.SetActive(true);
            
        }

        else if (!ModeSelected) // if a mode is not selected any more = stop filling the slider
        {
            CancelInvoke("FillSlider"); // stop filling the slider 
            particleObject.SetActive(false);
            BeginningSlider.value = 0;
            SliderHolder.SetActive(false); // turn the slider off again
            StatusText.text = "Choose your Game Mode";
        }
    }

    public void FillSlider()
    {
        Slider BeginningSlider = SliderHolder.GetComponent<Slider>();
        // Increase the slider value by 1 
        float currentValue = BeginningSlider.value + 1;

        // Clamp the value between the min and max values of the slider
        currentValue = Mathf.Clamp(currentValue, BeginningSlider.minValue, BeginningSlider.maxValue);

        // Update the slider value
        BeginningSlider.value = currentValue;

        if (BeginningSlider.value >= BeginningSlider.maxValue) // start the game with the selected mode when the slider is full 
        {
            SceneManager.LoadScene("Round");
        }


    }

}
