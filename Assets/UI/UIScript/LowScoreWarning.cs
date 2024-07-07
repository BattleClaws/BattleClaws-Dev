using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowScoreWarning : MonoBehaviour
{
    private bool isLowScore;

    private UIScoreManager _uiData;
    private CornerManager _cornerData;

    private float timer;

    private float _startTimer;
    public float timeToFlash = 10f;

    public float timeScale = 1f;


    void Start()
    {
        isLowScore = false;

        _uiData = GetComponentInParent<UIScoreManager>();
        _cornerData = GetComponentInParent<CornerManager>();
    }

    public void InvokeLowScore()
    {
        
        timer = 0;
        isLowScore = !isLowScore;
    }
    
    void Update()
    {
        _startTimer += Time.deltaTime;
        
        if (_uiData.GetLowestScore() == _uiData.PlayersScores[_cornerData.playerNumber] && _startTimer >= timeToFlash)
        {
            isLowScore = true;
        }
        else
        {
            isLowScore = false;
            timer = 0;
        }
        
        if (isLowScore) UpdateColor();
        else _cornerData.bgColorCurrent = _uiData.bgColorBase;
    }

    private void UpdateColor()
    {
        timer += Time.deltaTime * timeScale;

        float blendAmount = (Mathf.Sin(timer) + 1f) / 2f; 
        _cornerData.bgColorCurrent = _uiData.bgColorBase.gamma * (1 - blendAmount) + _uiData.bgColorWarning * blendAmount;
    }
}
