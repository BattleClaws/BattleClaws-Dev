using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupRejector : MonoBehaviour
{
    public float rejectForce;
    public float upwardsRejectForce;
    Vector3 rejectDirection;
    public Vector3 rejectTarget;
    
    // Start is called before the first frame update
    void Start()
    {
        rejectTarget = Vector3.zero;
        
    }

    // Update is called once per frame
    
}
