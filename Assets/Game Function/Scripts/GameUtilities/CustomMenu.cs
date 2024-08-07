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
    public Button action1; // top button
    public Button action2;// middle button
    public Button action3; // bottom button
    public Button returnButton; // the button that should close the menu 
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setCustomHeader(string MenuName)
    {
        customHeaderText.text = MenuName;
    }

    public void AssignAction1(UnityAction function, string buttonContext)
    {
        action1.GetComponentInChildren<TMP_Text>().text = buttonContext;
        action1.onClick.AddListener(function);
    }

    public void AssignAction2(UnityAction function, string buttonContext)
    {
        action2.GetComponentInChildren<TMP_Text>().text = buttonContext;
        action2.onClick.AddListener(function);
    }

    public void AssignAction3(UnityAction function, string buttonContext)
    {
        action3.GetComponentInChildren<TMP_Text>().text = buttonContext;
        action3.onClick.AddListener(function);   
    }

    public void ClearAllAssignedActions() // does not remove the listener from the Return Button (Should It?)
    {
        action1.onClick.RemoveAllListeners();
        action2.onClick.RemoveAllListeners();
        action3.onClick.RemoveAllListeners();
    }
    
}
