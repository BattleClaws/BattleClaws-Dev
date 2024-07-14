using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class SettingsScreen : MonoBehaviour
{
    public GameObject previousPage;
    private MenuManager menuManager;
    public GameObject firstSelected;

    private void Start()
    {
        menuManager = GameObject.FindObjectOfType<MenuManager>();
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    public void Back()
    { 
       menuManager.SetCurrentScreen(previousPage);
       previousPage.SetActive(true);
       gameObject.SetActive(false);
       
    }
}
