using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreUI : MonoBehaviour
{
    [SerializeField] private GameObject gameCompleteHeader, finalScoresSubHeader, scoresText,  winnerHeader, winnerText;

    private int screenWidth;
    private int screenHeight;
    
    void Start()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        // the game complete header
        LeanTween.moveY( gameCompleteHeader,  screenHeight * (1050f/1080f), 1f).setEase( LeanTweenType.easeInQuad ).setDelay(0.5f).setEase( LeanTweenType.easeInOutBack );
        
        //the winning player Label Header
        LeanTween.moveX( winnerHeader, screenWidth * (400f/1920f), 2f).setEase( LeanTweenType.easeInQuad ).setDelay(4.5f).setEase( LeanTweenType.easeInOutBack );
        
        // the winning player text 
        LeanTween.moveX( winnerText, screenWidth * (560f/1920f), 1.5f).setEase( LeanTweenType.easeInQuad ).setDelay(5f).setEase( LeanTweenType.easeInOutBack);
        LeanTween.scaleY(winnerText,1.5f,1.4f).setEase( LeanTweenType.easeInQuad ).setDelay(6).setEase( LeanTweenType.easeInOutBack );
        LeanTween.scaleX(winnerText,1.5f,1.2f).setEase( LeanTweenType.easeInQuad ).setDelay(6).setEase( LeanTweenType.easeInOutBack );
        LeanTween.scaleZ(winnerText,1.5f,1.2f).setEase( LeanTweenType.easeInQuad ).setDelay(6).setEase( LeanTweenType.easeInOutBack );
        
        //final scores subheader
        LeanTween.moveX(finalScoresSubHeader, screenWidth * (1415f/1920f), 1.3f).setEase( LeanTweenType.easeInQuad ).setDelay(2f).setEase( LeanTweenType.easeInOutBack );
        
        //finalScores Text
        LeanTween.moveY(scoresText, screenHeight * (300f/1080f), 1.5f).setEase( LeanTweenType.easeInQuad ).setDelay(3f).setEase( LeanTweenType.easeInOutBack );
        LeanTween.scaleY(scoresText,1.2f,1f).setEase( LeanTweenType.easeInQuad ).setDelay(4f).setEase( LeanTweenType.easeInOutBack );
        LeanTween.scaleX(scoresText,1.2f,1f).setEase( LeanTweenType.easeInQuad ).setDelay(4f).setEase( LeanTweenType.easeInOutBack );
        LeanTween.scaleZ(scoresText,1.2f,1f).setEase( LeanTweenType.easeInQuad ).setDelay(4f).setEase( LeanTweenType.easeInOutBack );
        
      
    }
    
    
    
}
