using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickupVisualRandomiser : MonoBehaviour
{
    //list of gameobjects to be randomly selected from
    public GameObject[] pickups;
    GameObject pickupToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        //select a random gameobject from the list
        pickupToSpawn = pickups[Random.Range(0, pickups.Length)];
        //instantiate the selected gameobject as a child of the current gameobject
        Instantiate(pickupToSpawn, transform.position, transform.rotation, transform);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
