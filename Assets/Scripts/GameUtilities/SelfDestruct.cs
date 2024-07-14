using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public int countDown = -1;

    private void Awake()
    {
        if (countDown > 0)
        {
            StartCoroutine(CountDown(countDown));
        }
    }

    private IEnumerator CountDown(int time)
    {
        yield return new WaitForSeconds(time);
        Destruct();
    }

    public void Destruct()
    {
        Destroy(gameObject);
    }
}
