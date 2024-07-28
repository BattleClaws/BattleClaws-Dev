using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightBlink : MonoBehaviour
{
    public int status = 0;

    public Light lightStatus;
    public float maxIntensity = 1;
    public float minIntensity = 0;

    public bool isOn = true;
    public float timer = 1;
    void Start()
    {
        lightStatus = GetComponent<Light>();
    }

    void Update()
    {
        if (status == 0)
        {
            lightStatus.intensity = 0;
        }
        else if (status == 1)
        {
            lightStatus.color = Color.red;
            timer += Time.deltaTime;
            if (timer > 1)
            {
                lightStatus.intensity = maxIntensity;
            }
            else if (timer > 2)
            {
                lightStatus.intensity = minIntensity;
                timer = 0;
            }
        }
        else if (status == 2)
        {
            lightStatus.color = Color.green;
            lightStatus.intensity = 1;
        }
    }
}
