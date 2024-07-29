using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBGToScore : MonoBehaviour
{
    private Vector2 currentPos;
    private Vector3 currentScale;

    private Vector2 actualCurrentPos;
    private Vector3 actualCurrentScale;

    public float lerpSpeed = 0.5f;

    private UIScoreManager _uiScore;
    private CornerManager _corner;

    public Vector2 maxPos;
    public Vector2 minPos;

    public Vector3 maxScale;
    public Vector3 minScale;

    private RectTransform _rectTransform;

    private float scale;

    public bool doScale = true;
    private void Start()
    {
        _uiScore = GetComponentInParent<UIScoreManager>();
        _corner = GetComponentInParent<CornerManager>();

        _rectTransform = gameObject.GetComponent<RectTransform>();
    }

    

    private void Update()
    {
        //print(_corner.GetScale());
        currentPos = Vector2.Lerp(minPos, maxPos, _corner.GetScale());
        

        actualCurrentPos = _rectTransform.localPosition;
        

        _rectTransform.localPosition = Vector2.Lerp(actualCurrentPos, currentPos, lerpSpeed);
        

        if (doScale)
        {
            currentScale = Vector3.Lerp(minScale, maxScale, _corner.GetScale());
            actualCurrentScale = _rectTransform.localScale;
            _rectTransform.localScale = Vector3.Lerp(actualCurrentScale, currentScale, lerpSpeed);
            
        }
    }
}
