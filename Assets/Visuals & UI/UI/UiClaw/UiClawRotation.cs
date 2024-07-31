using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiClawRotation : MonoBehaviour
{
    public float rotationSpeed;

    
    void Start()
    {
        
    }

    
    void FixedUpdate()
    {
        gameObject.transform.Rotate(new Vector3(0, rotationSpeed, 0));
    }
}
