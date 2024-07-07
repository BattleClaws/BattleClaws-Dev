using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UseBgOutlineColor : MonoBehaviour
{
    private CornerManager _cornerData;
    private UIScoreManager _uiData;
    private TextMeshProUGUI _text;

    private Material _uniqueMat;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _cornerData = GetComponentInParent<CornerManager>();
        _uiData = GetComponentInParent<UIScoreManager>();
        
        _uniqueMat = new Material(_text.material);

        _text.material = _uniqueMat;
    }

    private void Update()
    {
        _text.outlineColor = _cornerData.bgColorCurrent;
    }
}
