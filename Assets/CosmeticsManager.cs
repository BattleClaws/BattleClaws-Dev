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
        var poofRef = Resources.Load<GameObject>("Prefabs/Poof");
        Instantiate(poofRef, GetComponent<Player>().Model.transform);
        GameUtils.instance.audioPlayer.PlayChosenClip("Gameplay/Claw/ClawCosmetic");
        if (_currentCosmetic != null)
        {
            Destroy(_currentCosmetic);
            _currentCosmetic = null;
        }

        var newCosmetic = Instantiate(cosmetic, cosmeticsHolder.transform);
        print("Instantiated " + newCosmetic.name);
        newCosmetic.GetComponent<Cosmetic>().SetWearableView(true);
        _currentCosmetic = newCosmetic;
    }
}
