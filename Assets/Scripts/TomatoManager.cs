using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TomatoManager : MonoBehaviour
{

    public GameObject tomatoPrefab;
    public GameObject tomatoSpawn;
 

    // Start is called before the first frame update
    void Start()
    {
        
    }

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
