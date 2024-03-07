using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = System.Random;

public class Collectable : MonoBehaviour
{
    public static int Points { get; private set; }
    public Color Color { get; private set; }
    public GameObject Mesh { get; private set; }
    public PlayerController Holder { get; set; }

    public bool Special { get; private set; }
    [Tooltip("Percentage of special collectables: 0-100")]
    public static int specialPercent = 5;

    private void Start()
    {
        Mesh = transform.GetChild(0).gameObject;
        CollectableSetup();
    }

    private void CollectableSetup()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity) && hit.transform.CompareTag("DropZone"))
        {
            GameUtils.InitCollectables();
            Destroy(gameObject);
            return;
        }

        if (UnityEngine.Random.Range(0, 100) < specialPercent && !RoundManager.draw)
        {
            Special = true;
            Mesh.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/Collectableglint");
        }

        if (!RoundManager.draw)
        {
            Color = GameUtils.RequestColor();
            Mesh.GetComponent<Renderer>().material.color = Color;
        }
    }

    public static void SetValue(int value)
    {
        Points = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DropZone"))
        {
            if (!Holder || Holder == null)
            {
                GameUtils.InitCollectables();
                Destroy(gameObject);
                return;
            }
            if (other.GetComponent<Renderer>().material.color == Color)
            {
                if (Special)
                {
                    GameUtils.SpecialAction(Holder);
                }
                else
                {
                    var value = Holder.Properties.AddPoints(Points);
                    GameUtils.ScoreNotication(value, transform);
                }
                Destroy(gameObject);
            }   
        }
    }
}