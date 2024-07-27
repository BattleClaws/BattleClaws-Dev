using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupRejector : MonoBehaviour
{
    //set of tags to reject
    public string[] tagsToReject;
    public float rejectForce;
    public float upwardsRejectForce;
    Vector3 rejectDirection;
    public Transform rejectTarget;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
//check if the tag of the object entering the trigger is in the list of tags to reject
        if (Array.Exists(tagsToReject, tag => tag == other.tag))
        {
            //set the velocity of the object to zero
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
            //get vector between this object and the rejectDirection object
            Vector3 rejectDirection = rejectTarget.position - transform.position;
            //launch the object toward the reject direction with reject force
            other.GetComponent<Rigidbody>().AddForce(rejectDirection * rejectForce, ForceMode.Impulse);
            other.GetComponent<Rigidbody>().AddForce(Vector3.up * upwardsRejectForce, ForceMode.Impulse);
            
        }
    }
}
