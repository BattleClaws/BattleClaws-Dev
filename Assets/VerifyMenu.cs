using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VerifyMenu : MonoBehaviour
{
    void Start()
    {
        if (!SceneManager.GetSceneByName("InitNonPerishables").isLoaded && !GameObject.FindGameObjectWithTag("Menu"))
        {
            SceneManager.LoadScene("InitNonPerishables", LoadSceneMode.Additive);
        }
    }
}
