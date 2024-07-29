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

    private CornerStates currentState;

    public CornerStates CurrentState
    {
        get
        {
            return currentState;
        }
        set
        {
            //UpdateState(value);
            currentState = value;
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

    private float animationSpeed;
    private void UpdateState(CornerStates state)
    {
        _uiScore = GameUtils.instance.uIScoreManager;
        bgAnimator = gameObject.GetComponentInChildren<Animator>();

        //print(state);
        switch(state)
        {
            case CornerStates.freeze:
                //print("frozen!");
                bgColorBase = _uiScore.freezeColor;
                animationSpeed = _uiScore.slowSpeed;
                break;
            case CornerStates.doublePoints:
                bgColorBase = _uiScore.doubleColor;
                animationSpeed = _uiScore.fastSpeed;
                break;
            case CornerStates.speed:
                bgColorBase = _uiScore.speedColor;
                animationSpeed = _uiScore.fastSpeed;
                break;
            default:
                bgColorBase = _uiScore.baseColor;
                animationSpeed = _uiScore.baseSpeed;
                break;
        }

        //print(animationSpeed);

        //print(bgAnimator.gameObject.activeSelf);

        bgAnimator.SetFloat("Speed", animationSpeed);

        //bgColorCurrent = bgColorBase;
    }

    public void Update()
    {
        UpdateState(CurrentState);
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
