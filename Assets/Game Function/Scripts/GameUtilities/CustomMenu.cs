using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.Events;

public class CustomMenu : MonoBehaviour
{
    public TextMeshProUGUI customHeaderText; // header label
    public TextMeshProUGUI customSubHeaderText;
    public Button action1; // top button
    public Button action2;// middle button
    public Button action3; // bottom button
    public Button returnButton; // the button that should close the menu 

    public void setCustomHeaderAndSubHeader(string Header, string SubHeader)
    {
        customHeaderText.text = Header;
        customSubHeaderText.text = SubHeader;
    }

    // Unified method to assign actions to buttons
    public void AssignAction(Button button, UnityAction function, string buttonContext)
    {
        button.GetComponentInChildren<TMP_Text>().text = buttonContext;
        button.onClick.AddListener(function);
    }

    public void ClearAllAssignedActions() // remove all currently assigned actions 
    {
        action1.onClick.RemoveAllListeners();
        action2.onClick.RemoveAllListeners();
        action3.onClick.RemoveAllListeners();
        returnButton.onClick.RemoveAllListeners();
    }
}