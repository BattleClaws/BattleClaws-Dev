using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int PlayerNum { get; private set; }
    public Color PlayerColor { get; private set; }
    public int Speed { get; private set; }
    public int Multiplier { get; private set; }

    public GameObject heldObject = null;
    [SerializeField] private int _baseSpeed = 5;

    public GameObject Model
    {
        get => transform.Find("PlayerModelPlaceHolders").gameObject;
    }
    
    private void Awake()
    {
        Speed = _baseSpeed;
        Multiplier = 1;
        heldObject = this.gameObject;
    }

    public IEnumerator SpeedEffect(int amount, float length)
    {
        Speed = amount;
        yield return new WaitForSeconds(length);
        Speed = _baseSpeed;
    }

    public IEnumerator MultiplierEffect(int amount, float length)
    {
        Multiplier *= amount;
        yield return new WaitForSeconds(length);
        Multiplier = 1;
    }
}
