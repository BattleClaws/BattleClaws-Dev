using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cosmetic : MonoBehaviour
{
    public Vector3 _defaultScale;
    public Vector3 _defaultPosition;
    public Vector3 GachaScale;
    public Vector3 GachaLocation;
    public GameObject mesh;
    
    void Awake()
    {
        if(_defaultScale == Vector3.zero) {
            _defaultScale = mesh.transform.localScale;
            _defaultPosition = mesh.transform.localPosition;
        }
        
        //print(mesh.transform.localPosition + " | " + _defaultPosition);
    }

    public void SetWearableView(bool OnOff)
    {
        //print("Wearable View");

        if (OnOff)
        {
            if(GetComponentInChildren<Animator>() != null)
                GetComponentInChildren<Animator>().enabled = true;
            mesh.transform.localScale = _defaultScale;
            //print("Default Position: " + _defaultPosition);
            mesh.transform.localPosition = _defaultPosition;
        }
        else
        {
            if(GetComponentInChildren<Animator>() != null)
                GetComponentInChildren<Animator>().enabled = false;
            mesh.transform.localScale = GachaScale;
            mesh.transform.localPosition = GachaLocation;
        }
    }
}
