using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioObject;

    private void Start()
    {
        audioObject = GetComponent<AudioSource>();
    }

    public void PlayChosenClip(string clipName)
    {
        var clip = Resources.Load<AudioClip>("Audio/" + clipName);
        print("playing: " + clip.name);
        audioObject.PlayOneShot(clip);

    }
}
