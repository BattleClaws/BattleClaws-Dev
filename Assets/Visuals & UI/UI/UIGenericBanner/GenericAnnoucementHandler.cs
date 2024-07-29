using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GenericAnnoucementHandler : MonoBehaviour
{
    public float annoucementShowingTime = 5f;
    
    public GameObject endMarkerObject;
    public float moveToTime;
    public float moveOutTime;

    public float lineRevealLength;

    public GameObject line1EndPos;
    public GameObject line2EndPos;

    public float line2Delay = 0.1f;

    private Vector3 _line1StartPos;
    private Vector3 _line1StartScale;
    private Vector3 _line2StartPos;
    private Vector3 _line2StartScale;
    
    //public AnimationCurve shakeCurve;
    public LeanTweenType easeMoveTypeIn;
    public LeanTweenType easeMoveTypeOut;

    public LeanTweenType easeMoveTypeText;

    public Color[] playerColor = new Color[4];

    private Vector3 _startPosition;

    public GameObject line1TextObject;
    public GameObject line2TextObject;

    private void Start()
    {
        _startPosition = gameObject.transform.position;
        
        _line1StartPos = line1TextObject.transform.localPosition;
        _line1StartScale = line1TextObject.transform.localScale;
        
        _line2StartPos = line2TextObject.transform.localPosition;
        _line2StartScale = line2TextObject.transform.localScale;
    }

    public void StartAnnounce(string line1, string line2)
    {
        line1TextObject.GetComponent<TextMeshProUGUI>().text = line1;
        line2TextObject.GetComponent<TextMeshProUGUI>().text = line2;
        
        MoveToActive();
    }

    private void MoveToActive()
    {
        LeanTween.move(gameObject, endMarkerObject.transform, moveToTime).setEase(easeMoveTypeIn).setOnComplete(() => { ShowText(); });
    }

    private void MoveOutDisable()
    {
        LeanTween.move(gameObject, _startPosition, moveOutTime).setEase(easeMoveTypeOut).setOnComplete(() => { ResetAssets(); });
    }

    private void ResetAssets()
    {
        line1TextObject.transform.localPosition = _line1StartPos;
        line1TextObject.transform.localScale = _line1StartScale;
        
        line2TextObject.transform.localPosition = _line2StartPos;
        line2TextObject.transform.localScale = _line2StartScale;
    }

    public void ShowText()
    {
        ShowLine1();
        StartCoroutine(DelayTime(ShowLine2));
    }

    IEnumerator DelayTime(Action method)
    {
        float elapsedTime = 0f;
        

        while (elapsedTime < line2Delay)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        method();
    }

    IEnumerator waitToStop()
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < annoucementShowingTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        MoveOutDisable();
        
    }

    void ShowLine1()
    {
        LeanTween.move(line1TextObject, line1EndPos.transform.position, lineRevealLength).setEase(easeMoveTypeText);
        LeanTween.scale(line1TextObject, new Vector3(1, 1, 1), lineRevealLength).setEase(easeMoveTypeText);
    }
    void ShowLine2()
    {
        LeanTween.scale(line2TextObject, new Vector3(1, 1, 1), lineRevealLength).setEase(easeMoveTypeText);
        LeanTween.move(line2TextObject, line2EndPos.transform.position, lineRevealLength).setEase(easeMoveTypeText).setOnComplete(
            () => { StartCoroutine(waitToStop()); });
        
    }
    
    
}
