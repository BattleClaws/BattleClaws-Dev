using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField]
    private GameObject indicatorLight;
    [SerializeField]
    private ParticleSystem flames;

    [SerializeField] private GameObject collider;

    public void StartFlames(int time)
    {
        StartCoroutine(StartFlamesCoroutine(time));
    }

    private IEnumerator StartFlamesCoroutine(int time)
    {
        // warning period
        for (int i = 0; i <= 7; i++)
        {
            yield return new WaitForSeconds(0.3f);
            indicatorLight.GetComponent<Renderer>().material.color = Color.black;
            yield return new WaitForSeconds(0.3f);
            indicatorLight.GetComponent<Renderer>().material.color = Color.red;
        }
        
        // flames
        flames.Play();
        collider.SetActive(true);
        yield return new WaitForSeconds(time);
        flames.Stop();
        indicatorLight.GetComponent<Renderer>().material.color = Color.black;
        collider.SetActive(false);
    }
}
