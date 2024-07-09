using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreUI : MonoBehaviour
{
    [SerializeField] private GameObject gameCompleteHeader, finalScoresSubHeader, scoresText,  winnerHeader, winnerText;

    void Start()
    {
        // the game complete header
        LeanTween.moveY( gameCompleteHeader, 1050f, 1f).setEase( LeanTweenType.easeInQuad ).setDelay(0.5f).setEase( LeanTweenType.easeInOutBack );
        
        //the winning player Label Header
        LeanTween.moveX( winnerHeader, 400, 2f).setEase( LeanTweenType.easeInQuad ).setDelay(4.5f).setEase( LeanTweenType.easeInOutBack );
        
        // the winning player text 
        LeanTween.moveX( winnerText, 560, 1.5f).setEase( LeanTweenType.easeInQuad ).setDelay(5f).setEase( LeanTweenType.easeInOutBack);
        LeanTween.scaleY(winnerText,3f,1.4f).setEase( LeanTweenType.easeInQuad ).setDelay(6).setEase( LeanTweenType.easeInOutBack );
        LeanTween.scaleX(winnerText,3f,1.2f).setEase( LeanTweenType.easeInQuad ).setDelay(6).setEase( LeanTweenType.easeInOutBack );
        LeanTween.scaleZ(winnerText,3f,1.2f).setEase( LeanTweenType.easeInQuad ).setDelay(6).setEase( LeanTweenType.easeInOutBack );
        
        //final scores subheader
        LeanTween.moveX(finalScoresSubHeader, 1415, 1.3f).setEase( LeanTweenType.easeInQuad ).setDelay(2f).setEase( LeanTweenType.easeInOutBack );
        
        //finalScores Text
        LeanTween.moveY(scoresText, 300, 1.5f).setEase( LeanTweenType.easeInQuad ).setDelay(3f).setEase( LeanTweenType.easeInOutBack );
        LeanTween.scaleY(scoresText,1.2f,1f).setEase( LeanTweenType.easeInQuad ).setDelay(4f).setEase( LeanTweenType.easeInOutBack );
        LeanTween.scaleX(scoresText,1.2f,1f).setEase( LeanTweenType.easeInQuad ).setDelay(4f).setEase( LeanTweenType.easeInOutBack );
        LeanTween.scaleZ(scoresText,1.2f,1f).setEase( LeanTweenType.easeInQuad ).setDelay(4f).setEase( LeanTweenType.easeInOutBack );
        
      
    }
    
    
    
}
