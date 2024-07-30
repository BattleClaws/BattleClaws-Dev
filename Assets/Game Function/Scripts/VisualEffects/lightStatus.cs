using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightStatus : MonoBehaviour
{
    public int status = 0;

    private Renderer materialStatus;
    public Light light;

    public float maxIntensity = 60;
    public float minIntensity = 0;
    public Color orange = new Color(1.0f, 0.3f, 0.0f, 1.0f);

    //private float timer;

    void Start()
    {
        materialStatus = GetComponent<Renderer>();
        //light = GetComponentsInChildren<Light>();
        //can't get this to work rn so you have to manually assign it sorry :(
    }

    void Update()
    {
        if (status == 0)
        {
            //materialStatus.material.SetFloat("_Status", 0);
            materialStatus.material.SetColor("_EmissionColor", Color.grey);
            light.intensity = 0;
            //timer = 0;
        }
        else if (status == 1)
        {
            //materialStatus.material.SetFloat("_Status", 1);
            materialStatus.material.SetColor("_EmissionColor", orange);
            light.color = orange;

            light.intensity = maxIntensity;

            //timer += Time.deltaTime;
            //light.intensity = Mathf.Lerp(maxIntensity, minIntensity, Mathf.Round(timer % 1)) * 1;
        }
        else if (status == 2)
        {
            //materialStatus.material.SetFloat("_Status", 2);
            materialStatus.material.SetColor("_EmissionColor", Color.green);

            light.color = Color.green;
            light.intensity = maxIntensity;
            //timer = 0;
        }
    }
}