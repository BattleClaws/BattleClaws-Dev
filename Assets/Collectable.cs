using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public static int Points { get; private set; }
    public Color Color { get; private set; }
    public GameObject Mesh { get; private set; }
    public PlayerController Holder { get; set; }

    private void Start()
    {
        Mesh = transform.GetChild(0).gameObject;
        CollectableSetup();
    }

    private void CollectableSetup()
    {
        Color = GameUtils.RequestColor();
        Mesh.GetComponent<Renderer>().material.color = Color;
    }

    public static void SetValue(int value)
    {
        Points = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DropZone"))
        {
            if (other.GetComponent<Renderer>().material.color == Color)
            {
                Holder.Properties.AddPoints(Points);
                print("Added points to " + Holder.Properties.PlayerNum);
                GameUtils.ScoreNotication(Points, transform);
                Destroy(gameObject);
            }   
        }
    }
}