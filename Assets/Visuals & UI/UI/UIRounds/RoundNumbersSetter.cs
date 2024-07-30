using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundNumbersSetter : MonoBehaviour
{
    public TextMeshProUGUI roundNumberTMP;
    public TextMeshProUGUI roundMaxTMP;

    public int currentRound
    {

        get { return currentRound;}
        set
        {
            currentRound = value; 
            updateRound();
        }
    }
    public int maxRound;

    public void updateRound()
    {
        roundNumberTMP.text = currentRound.ToString();

        roundMaxTMP.text = "/" + maxRound.ToString();
    }

    private void Start()
    {
        updateRound();
    }
}
