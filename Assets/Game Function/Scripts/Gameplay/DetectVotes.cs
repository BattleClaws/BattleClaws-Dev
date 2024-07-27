using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectVotes : MonoBehaviour
{
    // Option name associated with this basket
    public string optionName;
    public ModeSelection modeTallyScript;
    private string lastCollectableName = "";
    void Start()
    {
        modeTallyScript = FindObjectOfType<ModeSelection>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable") )
        {
           
            Debug.Log("Collided");
            // Record a vote for the associated option when a collectible enters the trigger
            modeTallyScript.RecordVote(optionName);
            modeTallyScript.TallyVotes();
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        string collectableName = other.gameObject.name.ToString();
        if (other.CompareTag("Collectable"))
        {
            // Remove a vote for the associated option when a collectible exits the trigger
            modeTallyScript.RemoveVote(optionName);
            modeTallyScript.TallyVotes();
            lastCollectableName = null;
        }
    }
}