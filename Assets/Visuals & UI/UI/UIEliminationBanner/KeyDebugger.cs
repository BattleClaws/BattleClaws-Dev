using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KeyDebugger : MonoBehaviour
{
    public PlayerAnnocementPopup poop4;
    public PlayerAnnocementPopup poop3;
    public PlayerAnnocementPopup poop2;
    public PlayerAnnocementPopup poop1;

    public string line1;
    public string line2;

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            poop4.TriggerAnnouncement(line1, line2);
        }
        if (Input.GetKey(KeyCode.E))
        {
            poop3.TriggerAnnouncement(line1, line2);
        }
        if (Input.GetKey(KeyCode.W))
        {
            poop2.TriggerAnnouncement(line1, line2);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            poop1.TriggerAnnouncement(line1, line2);
        }
       
    }
}
