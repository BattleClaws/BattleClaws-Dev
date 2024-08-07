using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDebug : MonoBehaviour
{
    public UIScoreManager manager;
    public PlayerAnnocementHandler ann;

    public void Start()
    {
        manager = GameUtils.instance.uIScoreManager;
    }

    public void SetFreeze (int i)
    {
        manager.SetCornerState(i, CornerStates.freeze);
    }

    public void SetDefault (int i)
    {
        manager.SetCornerState(i, CornerStates.defaut);
    }

    public void SetSpeed (int i)
    {
        manager.SetCornerState(i, CornerStates.speed);
    }

    public void SetDouble (int i)
    {
        manager.SetCornerState(i, CornerStates.doublePoints);
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            //print("frozen");
            SetFreeze(0);

            ann.StatusPopup(0, "Frozen!");
        }
        if (Input.GetKey(KeyCode.G))
        {
            //print("def");
            SetDefault(0);
        }
        if (Input.GetKey(KeyCode.H))
        {
            //print("2x");
            SetDouble(1);
        }
        if (Input.GetKey(KeyCode.J))
        {
            //print("J");
            SetSpeed(1);
        }
    }
}
