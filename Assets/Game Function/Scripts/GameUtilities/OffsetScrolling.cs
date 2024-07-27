using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetScrolling : MonoBehaviour
{
    public float scrollSpeedX;
    public float scrollSpeedY;

    private Renderer renderer;
    private Vector2 savedOffset;

    void Start () {
        renderer = GetComponent<Renderer> ();
    }

    void Update () {
        float x = Mathf.Repeat (Time.time * scrollSpeedX, 1);
        float y = Mathf.Repeat (Time.time * scrollSpeedY, 1);
        Vector2 offset = new Vector2 (x, y);
        renderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
    }
}