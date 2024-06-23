using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class MenuLogic : MonoBehaviour
{
    private int customTimerLength;
    private int customNumberOfRounds;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    public void StoreChosenSetting(int buttonValue)
    {
        if (buttonValue <= 10)
        {
            customNumberOfRounds = buttonValue;
        }

        if (buttonValue >= 10)
        {
            customTimerLength = buttonValue;
        }
        
        Debug.Log(customTimerLength + " " + customNumberOfRounds.ToString());
    }
}
