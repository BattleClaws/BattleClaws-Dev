using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SettingsScreen : MonoBehaviour
{
    public GameObject previousPage;
    private MenuManager menuManager;

    private void Start()
    {
        menuManager = GameObject.FindObjectOfType<MenuManager>();
    }

    public void Back()
    { 
       menuManager.SetCurrentScreen(previousPage);
       previousPage.SetActive(true);
       gameObject.SetActive(false);
       
    }
}
