using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.WSA;



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
    [SerializeField] private string ModeNameString;
    [SerializeField] private string ModeSceneToLoad;
    [SerializeField] private int requiredPlayers;
  
   

    
    


    private void Start()
    {
        UpdateTextDisplay();
        SliderHolder.SetActive(false); // turn the slider off on start
     
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
            if (ModeNameString != "Coming Soon")
            {
                votesForThisMode++;
                
                checkVotes();
            }

            UpdateTextDisplay();
        }
    }

    public void OnTriggerExit(Collider other) // if a player removes their vote, minus the vote and check to see if there is still a majority 
    {
        if (other.CompareTag("Collectable"))
        {
            votesForThisMode--;
          
          
           checkVotes();
           UpdateTextDisplay();
        }
    }

    public void checkVotes()
    {
        possibleMaxVotes = Player.amountOfPlayers; 
        NumReqForMajority = possibleMaxVotes / 2 + 1;

        if(votesForThisMode >= NumReqForMajority) // if any given mode has enough votes for a majority 
        {
            
            ModeSelected = true;
            SliderHolder.SetActive(true); 
            ManageSlider(); // update the slider value
            UpdateTextDisplay();
        } 
     
        else
        {
            ModeSelected=false; 
            ManageSlider(); // reset the slider value 
            UpdateTextDisplay();
        }
    }

    public void ManageSlider()
    {
        Slider BeginningSlider = SliderHolder.GetComponent<Slider>();
        if(ModeSelected && Player.amountOfPlayers >= requiredPlayers)
        {
            InvokeRepeating("FillSlider", 0.1f, 0.05f);
        }

        else if (!ModeSelected) // if a mode is not selected any more = stop filling the slider
        {
            UpdateTextDisplay();
            CancelInvoke("FillSlider"); // stop filling the slider 
            BeginningSlider.value = 0;
            SliderHolder.SetActive(false); // turn the slider off again
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
            SceneManager.LoadScene(ModeSceneToLoad);
        }
    }

    public void UpdateTextDisplay()
    {
        if (ModeSelected && Player.amountOfPlayers < requiredPlayers && ModeNameString != "Coming Soon" )
        {
            StatusText.text = requiredPlayers + " or more Players required";
        }
        else if (ModeSelected && ModeNameString != "Coming Soon")
        {
            StatusText.text = "Starting " + ModeNameString + " ...";
        }
        else if (!ModeSelected && ModeNameString != "Coming Soon")
        {
            StatusText.text = "Choose your Game Mode";
        }

        else if (ModeNameString == "Coming Soon")
        {
            StatusText.text = "More Game Modes Coming Soon!";
           // ComingSoonInfo();
        }
    }

    //public void ComingSoonInfo()
   // {
       // Animator panelAnim = FutureModesInfo.GetComponent<Animator>();
       // FutureModesInfo.SetActive(true);
           // if (panelAnim == null)
           // {
             //   panelAnim.SetTrigger("Activate");
          //  }
          //  else
         //   {
//panelAnim.SetTrigger("DeActivate");
         //   }
   // }
}
