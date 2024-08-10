using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CustomMenu : MonoBehaviour
{
    public TextMeshProUGUI customHeaderText; // header label
    public TextMeshProUGUI customSubHeaderText;
    public Button customButtonPrefab; // top button
    public Button returnButton; // the button that should close the menu 

    public GameObject buttonContainer;
    
    // List to keep track of all instantiated buttons
    private List<Button> spawnedButtons = new List<Button>();

    public void setCustomHeaderAndSubHeader(string Header, string SubHeader)
    {
        customHeaderText.text = Header;
        customSubHeaderText.text = SubHeader;
    }

    // Unified method to assign actions to buttons
    public void AssignAction(UnityAction function, string buttonContext)
    {
        Debug.Log("AssignAction called");
        // Instantiate the button from the prefab and add it to the container
        Button newButton = Instantiate(customButtonPrefab, buttonContainer.transform);
        
        // Set the button text
        newButton.GetComponentInChildren<TMP_Text>().text = buttonContext;
        
        // Add the action to the button's onClick event
        newButton.onClick.AddListener(function);
        
        // Add the new button to the spawned buttons list
        spawnedButtons.Add(newButton);

        Debug.Log("Button instantiated and ready");
        SetFirstSelectedButton();
        SetButtonNavigation();
    }
    
    public void SetFirstSelectedButton()
    {
        if (spawnedButtons.Count > 0)
        {
            // Set the first button as selected
            EventSystem.current.SetSelectedGameObject(spawnedButtons[0].gameObject);
        }
    }
    
    private void SetButtonNavigation()
    {
        // Update navigation for all buttons
        for (int i = 0; i < spawnedButtons.Count; i++)
        {
            Button button = spawnedButtons[i];
            Navigation nav = new Navigation
            {
                mode = Navigation.Mode.Explicit
            };

            // Set navigation for the button
            nav.selectOnUp = i > 0 ? spawnedButtons[i - 1] : spawnedButtons[spawnedButtons.Count - 1];
            nav.selectOnDown = i < spawnedButtons.Count - 1 ? spawnedButtons[i + 1] : spawnedButtons[0];

            button.navigation = nav;
        }
    }

    public void ClearAllAssignedActions() // remove all currently assigned actions 
    {
        for (int i = spawnedButtons.Count - 1; i >= 0; i--)
        {
            Button customButton = spawnedButtons[i];
            customButton.onClick.RemoveAllListeners();
            spawnedButtons.RemoveAt(i);
            Destroy(customButton.gameObject);
        }
        print("All buttons cleared and destroyed!");
    }
}
