using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillzoneManager : MonoBehaviour
{

    public GameObject collectablePrefab;
    public Vector3 instantiationPos;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            Destroy(other.gameObject);
            Instantiate(collectablePrefab, instantiationPos, Quaternion.identity);
        }
    }
}
