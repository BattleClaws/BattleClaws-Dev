using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICornerColorHandler : MonoBehaviour
{
    private CornerManager _cornerData;
    private UIScoreManager _mainManager;

    
    private void Start()
    {
        _cornerData = GetComponentInParent<CornerManager>();
        _mainManager = GetComponentInParent<UIScoreManager>();
    }
}
