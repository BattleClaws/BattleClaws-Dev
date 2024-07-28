using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnnocementHandler : MonoBehaviour
{
    public PlayerAnnocementPopup[] playerAnnocementPopups;

    public void StatusPopup(int i, string topText, string bottomText)
    {
        playerAnnocementPopups[i].TriggerAnnouncement(topText, bottomText);
    }
}
