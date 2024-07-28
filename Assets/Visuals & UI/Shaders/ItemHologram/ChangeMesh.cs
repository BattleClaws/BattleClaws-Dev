using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMesh : MonoBehaviour
{
    public GameObject[] holograms;

    public float changeSpeed = 0.1f;

    private int currentHologramId;
    private GameObject currentHologramObject;

    public Coroutine switchHologramCoroutine;

    public Material hologramMat;
    void Start()
    {
        

        switchHologramCoroutine = StartCoroutine(SwitchHologram());
    }

    IEnumerator SwitchHologram()
    {
        while(true){
        currentHologramId++;

        Destroy(currentHologramObject);
        currentHologramObject = Instantiate(holograms[currentHologramId % holograms.Length],gameObject.transform.position, gameObject.transform.rotation, gameObject.transform);
        currentHologramObject.GetComponent<MeshRenderer>().material = hologramMat;

        yield return new WaitForSeconds(changeSpeed);
        }
    }

}
