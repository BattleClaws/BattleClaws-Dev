using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUpdate : MonoBehaviour
{
    
    private UIScoreManager _uiScore;
    private CornerManager _cornerManager;
    private TextMeshProUGUI _text;

    private void Start()
    {
        _uiScore = GetComponentInParent<UIScoreManager>();
        _cornerManager = GetComponentInParent<CornerManager>();
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        _text.text = _uiScore.PlayersScores[_cornerManager.playerNumber].ToString().PadLeft(6, '0');;
    }
}
