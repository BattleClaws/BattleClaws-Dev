using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BannerHandler : MonoBehaviour
{
    public float annoucementShowingTime = 5f;
    
    public GameObject endMarkerObject;
    public float moveToTime;
    public float moveOutTime;
    public float dotLength;

    public float annoucementLength;
    public TMP_Text announcementType;

    public GameObject annoucementEndPos;

    private Vector3 _annoucementStartPos;
    private Vector3 _annoucementStartScale;
    
    //public AnimationCurve shakeCurve;
    public LeanTweenType easeMoveTypeIn;
    public LeanTweenType easeMoveTypeOut;

    public LeanTweenType easeMoveTypeAn;

    public Color[] playerColor = new Color[4];

    private Vector3 _startPosition;

    public TextMeshProUGUI playerTMP;

    public TextMeshProUGUI dotTMP;
    public GameObject annoucementTextObject;

    private void Start()
    {
        _startPosition = gameObject.transform.position;
        _annoucementStartPos = annoucementTextObject.transform.localPosition;
        _annoucementStartScale = annoucementTextObject.transform.localScale;
    }

    public void EliminationAnnounce(int playerId = 0)
    {
        MoveToActive(playerId);
        announcementType.text = (RoundManager.gameStyle == GameType.Basic) ? "ELIMINATED" : "WINNER";
    }

    private void MoveToActive(int playerId = 0)
    {
        playerTMP.text = "P" + playerId.ToString();
        
        int tempId = 0;

        if (playerId < 1 || playerId > 4)
        {
            tempId = 0;
        }
        else
        {
            tempId = playerId - 1;
        }
        playerTMP.color = playerColor[tempId];

        dotTMP.text = "is";
        
        LeanTween.move(gameObject, endMarkerObject.transform, moveToTime).setEase(easeMoveTypeIn).setOnComplete(() => { ShowText(); });
    }

    private void MoveOutDisable()
    {
        LeanTween.move(gameObject, _startPosition, moveOutTime).setEase(easeMoveTypeOut).setOnComplete(() => { ResetAssets(); });
    }

    private void ResetAssets()
    {
        annoucementTextObject.transform.localPosition = _annoucementStartPos;
        annoucementTextObject.transform.localScale = _annoucementStartScale;
    }

    public void ShowText()
    {
        StartCoroutine(dotReveal());
    }

    IEnumerator dotReveal()
    {
        float elapsedTime = 0f;
        
        //print("awog?");

        while (elapsedTime < dotLength)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > dotLength / 3)
            {
                dotTMP.text = "is.";
            }
            if (elapsedTime > (dotLength / 3) * 2)
            {
                dotTMP.text = "is..";
            }
            
            yield return null;
        }

        dotTMP.text = "is...";
        ShowAnnoucement();
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

    void ShowAnnoucement()
    {
        LeanTween.move(annoucementTextObject, annoucementEndPos.transform.position, annoucementLength).setEase(easeMoveTypeAn).setOnComplete(
            () => { StartCoroutine(waitToStop()); });
        LeanTween.scale(annoucementTextObject, new Vector3(1, 1, 1), annoucementLength).setEase(easeMoveTypeAn);
    }
    
    
}
