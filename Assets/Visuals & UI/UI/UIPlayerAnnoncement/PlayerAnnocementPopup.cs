using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerAnnocementPopup : MonoBehaviour
{
    public GameObject bannerPrefab;
    
    public GameObject targetPosMarker;
    public GameObject startPosMarker;

    public float timeToMoveIn;
    public float timeToMoveOut;

    public float holdTime;

    public LeanTweenType moveInType;
    public LeanTweenType moveOutType;
    

    private GameObject _banner;
    public void TriggerAnnouncement(string topText, string bottomText)
    {
        GameObject tempBanner = Instantiate(bannerPrefab, startPosMarker.transform.position, Quaternion.identity, gameObject.transform);

        if (tempBanner != _banner)
        {
            Destroy(_banner);
            _banner = tempBanner;
        }
        
        _banner.GetComponent<PlayerBannerSetup>().UpdateText(topText, bottomText);
        
        LeanTween.move(_banner, targetPosMarker.transform.position, timeToMoveIn).setEase(moveInType).setOnComplete(x =>
            {
                StartCoroutine(DelayTime(HideAnnouncement, holdTime));
            }
        );
    }
    
    
    IEnumerator DelayTime(Action method, float delay)
    {
        float elapsedTime = 0f;
        

        while (elapsedTime < delay)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        method();
    }

    void HideAnnouncement()
    {
        LeanTween.move(_banner, startPosMarker.transform.position, timeToMoveOut).setEase(moveOutType)
            .setOnComplete(x =>
            {
                Destroy(_banner);
            });
    }
}
