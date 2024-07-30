using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialBlink : MonoBehaviour
{
    public int status = 0;

    public Renderer objectRenderer;
    public Material materialStatus;
    public Color redColor = Color.red;
    public Color greenColor = Color.green;
    public float maxIntensity = 1;
    public float minIntensity = 0;

    public bool isOn = true;
    public float timer = 0;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
       // materialStatus = objectRenderer.material;
    }

    void Update()
    {
        if (status == 0)
        {
            // Set material emission intensity to zero
            materialStatus.SetColor("_EmissionColor", Color.black);
        }
        else if (status == 1)
        {
            materialStatus.color = redColor;
            timer += Time.deltaTime;
            if (timer < 1)
            {
                materialStatus.SetColor("_EmissionColor", redColor * maxIntensity);
            }
            else if (timer < 2)
            {
                materialStatus.SetColor("_EmissionColor", redColor * minIntensity);
            }
            else
            {
                timer = 0;
            }
        }
        else if (status == 2)
        {
            materialStatus.color = greenColor;
            materialStatus.SetColor("_EmissionColor", greenColor * maxIntensity);
        }

        // Ensure the material's emission color is updated in real-time
        objectRenderer.material.SetColor("_EmissionColor", materialStatus.GetColor("_EmissionColor"));
    }
}