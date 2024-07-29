using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class UIBehaviour : MonoBehaviour
{

    // this script allows for different fade in / flash / swapping sprites depending on what's needed. 
    // add this to a TMP or a UI Image and adjust values in the inspector to toggle different features / speeds

    // Text Variables
    private TextMeshProUGUI textObject;
    [SerializeField] private bool ShouldFadeText;

    //UI Image Variables
    private Image buttonPromptImage;
    private Sprite startingSprite;
    [SerializeField] private Sprite promptSprite;
    [SerializeField] private bool ShouldSwapSprite;

    // universal variables (speed etc)
    [SerializeField] private float desiredSpeed;


    public void Start()
    {
        textObject = GetComponent<TextMeshProUGUI>();
        buttonPromptImage = GetComponent<Image>();
      
   
     
        if(buttonPromptImage != null)
        {
            startingSprite = buttonPromptImage.sprite;
            if (!ShouldSwapSprite)
            {
                StartCoroutine(flashImage());
            }

            else
            {
                StartCoroutine(SwapImageSprite());
            }
        }
    }

    private void Update()
    {
        if (textObject != null && ShouldFadeText)
        {
            FadeInText();
        }

    
            
    }

    public void FadeInText()
    {
        {
            textObject.alpha = Mathf.Sin(Time.time * desiredSpeed) * 0.5f + 0.5f;
        }
    }

    public IEnumerator flashImage()
    {
        yield return new WaitForSeconds(0.5f);

        buttonPromptImage.color -= new Color(0f, 0f, 0f, 1f);

        yield return new WaitForSeconds(0.5f);

        buttonPromptImage.color += new Color(0f, 0f, 0f, 1f);

        StartCoroutine(flashImage());
    }

    public IEnumerator SwapImageSprite()
    {
      yield return new WaitForSeconds(desiredSpeed);

      buttonPromptImage.sprite = promptSprite;
        
      yield return new WaitForSeconds(desiredSpeed);

      buttonPromptImage.sprite = startingSprite;

      StartCoroutine(SwapImageSprite());
    }


 
}