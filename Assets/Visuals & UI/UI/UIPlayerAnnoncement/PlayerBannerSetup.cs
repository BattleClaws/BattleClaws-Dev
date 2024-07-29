using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerBannerSetup : MonoBehaviour
{
    public TextMeshProUGUI topText;
    public TextMeshProUGUI bottomText;

    public void UpdateText(string line1, string line2)
    {
        topText.text = line1;
        bottomText.text = line2;
    }
    
    
}
