using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum CornerStates
{
    defaut,
    freeze,
    speed,
    doublePoints
}

public class CornerManager : MonoBehaviour
{
    [DoNotSerialize]public Color bgColorCurrent;
    [DoNotSerialize]public Color bgColorBase;

    public CornerStates CurrentState
    {
        get
        {
            return CurrentState;
        }
        set
        {
            UpdateState(value);
            CurrentState = value;
        }
    }
    
    public Color playerColor;
    
    public int playerNumber;

    private UIScoreManager _uiScore;

    public Animator bgAnimator; 

    public void Start()
    {
        _uiScore = GameUtils.instance.uIScoreManager;
        bgAnimator = gameObject.GetComponentInChildren<Animator>();

        CurrentState = CornerStates.defaut;
    }

    private void UpdateState(CornerStates state)
    {
        switch(state)
        {
            case CornerStates.freeze:
                bgColorBase = _uiScore.freezeColor;
                bgAnimator.speed = _uiScore.slowSpeed;
                break;
            case CornerStates.doublePoints:
                bgColorBase = _uiScore.doubleColor;
                bgAnimator.speed = _uiScore.fastSpeed;
                break;
            case CornerStates.speed:
                bgColorBase = _uiScore.speedColor;
                bgAnimator.speed = _uiScore.fastSpeed;
                break;
            default:
                bgColorBase = _uiScore.baseColor;
                bgAnimator.speed = _uiScore.baseSpeed;
                break;
        }
        
            
                
        
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
