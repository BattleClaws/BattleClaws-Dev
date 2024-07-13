using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCam : MonoBehaviour
{
    public GameObject lookAtObj;
    private Camera mainCamera;
    void Start()
    {
        if (lookAtObj == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        if (lookAtObj == null)
        {
            LookTowardsCam();
        }
        else
        {
            LookTowardsObj();
        }
    }

    void LookTowardsCam()
    {
        if (mainCamera != null)
        {
            // Calculate the rotation to make the UI face the camera
            Vector3 lookDirection = mainCamera.transform.forward;
            transform.forward = -lookDirection; // Rotate the UI to face the camera

            // Invert the rotation to face the correct way
            transform.Rotate(0f, 180f, 0f);
        }
    }

    void LookTowardsObj()
    {
        gameObject.transform.LookAt(lookAtObj.transform);
    }
}
