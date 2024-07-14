using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    private PlayerController _controller;

    private void Awake()
    {
        _controller = transform.parent.GetComponent<PlayerController>();
    }

    // This script is on the model, and passes the fact theres been a collision to the controller script
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _controller.KnockBack(other.transform.parent.transform, true);
        }

        if (other.CompareTag("Constraint"))
        {
            _controller.hold();
        }

        if (other.CompareTag("Return"))
        {
            _controller.ResetPosition();
        }
        
        

    }

    private void OnTriggerExit(Collider other)
    {
        
        //print("Exit Trigger: " + other.name);
        if (other.CompareTag("SafeZone") && RoundManager.draw)
        {
            _controller.Explode();
        }
    }
}
