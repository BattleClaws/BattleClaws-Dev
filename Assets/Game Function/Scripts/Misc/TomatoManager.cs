using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TomatoManager : MonoBehaviour
{
    //Attach this script to a gameobject placed at the point you want the tomatos to spawn. 
    //This script controls instantiating the tomatos (currently you have to press T) 

    public GameObject tomatoPrefab; 
    
  
    // Update is called once per frame
    void Update()
    {
        if(tomatoPrefab != null)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                spawnTomato();
            }
        }
    }



    public void spawnTomato()
    {
        // spawn the tomato
        Instantiate(tomatoPrefab, transform.position, transform.rotation);
    }

   
}
