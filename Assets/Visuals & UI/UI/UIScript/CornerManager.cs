using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CornerManager : MonoBehaviour
{
    [DoNotSerialize]public Color bgColorCurrent;
    
    
    public Color playerColor;
    
    public int playerNumber;

    private UIScoreManager _uiScore;

    public void Start()
    {
        _uiScore = GameUtils.instance.uIScoreManager;
    }

    public float GetScale()
    {
        //print("UI SCORE OBJ: " + _uiScore.name);
        var lowest = _uiScore.GetLowestScore();
        var max = _uiScore.GetHighestScore();

        float currentVal = _uiScore.PlayersScores[playerNumber];

        float output = Mathf.InverseLerp(lowest, max, currentVal);

        if (output > 1)
        {
            output = 1;
        }

        return output;
    }
}
