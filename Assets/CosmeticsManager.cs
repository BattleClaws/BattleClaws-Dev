using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmeticsManager : MonoBehaviour
{
    [SerializeField] private GameObject cosmeticsHolder;
    private GameObject _currentCosmetic;
    
    void Start()
    {
        _currentCosmetic = null;
    }


    public void SetCosmetic(GameObject cosmetic)
    {
        if (_currentCosmetic != null)
        {
            Destroy(_currentCosmetic);
            _currentCosmetic = null;
        }

        var newCosmetic = Instantiate(cosmetic, cosmeticsHolder.transform);
        _currentCosmetic = newCosmetic;
    }
}
