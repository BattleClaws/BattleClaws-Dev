using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneUtils : MonoBehaviour
{
    public PlayerController player;

    public void SetTrigger(string trigger)
    {
        player.Properties.Animator.SetTrigger(trigger);
    }

    public void EndScene(string scene)
    {
        StartCoroutine(ChangeScene(scene));
    }
    
    public IEnumerator ChangeScene(string scene)
    {
        var pane = GameObject.Find("Darkening").gameObject;

        for (float i = 0; i <= 1.0f; i += 0.05f)
        {
            pane.GetComponent<UnityEngine.UI.Image>().color = new Color(0, 0, 0, i);
            yield return new WaitForFixedUpdate();
        }
        pane.GetComponent<UnityEngine.UI.Image>().color = new Color(0, 0, 0, 1);

        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);
    }

}
