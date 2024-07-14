using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseBgColor : MonoBehaviour
{
    private CornerManager _cornerData;
    private Image _image;
    void Start()
    {
        _cornerData = GetComponentInParent<CornerManager>();

        _image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        _image.color = _cornerData.bgColorCurrent;
    }
}
