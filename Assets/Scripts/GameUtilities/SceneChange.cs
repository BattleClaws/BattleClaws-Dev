using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{

    public string destinationScene;
    public int timeUntilChange;
    public bool startOnAwake;
    
    
    void Start()
    {
        if (startOnAwake)
        {
            StartCoroutine(StartSceneChange());
        }
    }

    private IEnumerator StartSceneChange()
    {
        yield return new WaitForSeconds(timeUntilChange);
        SceneManager.LoadScene(destinationScene);
    }
}
