using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentShakerManager : MonoBehaviour
{
    [Header("Cannon Control")]
    [SerializeField]
    private List<Cannon> cannons;
    [SerializeField] private int cannonFrequency;
    [SerializeField] private int cannonLength;

    void Start()
    {
        InvokeRepeating(nameof(StartCanons), 0, cannonFrequency);
    }

    private void StartCanons()
    {
        cannons.ForEach(c => c.StartFlames(cannonLength));
    }
    
}
