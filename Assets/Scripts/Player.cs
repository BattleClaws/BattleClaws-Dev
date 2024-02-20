using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Allows newly spawned claws to know their id
    public static int amountOfPlayers = 0;
    
    
    // Properties
    public int PlayerNum { get; private set; }
    public Color PlayerColor { get; private set; }
    public int Speed { get; private set; }
    public int Points { get; private set; }
    public int Multiplier { get; private set; }
    public GameObject Model
    {
        get => transform.Find("Arcade Grabber").gameObject;
    }
    public Animator Animator
    {
        get => Model.GetComponent<Animator>();
    }

    // Properties End
    
    
    // Base speed, not the active speed, but the one the claw will return to after an effect
    [SerializeField] private int _baseSpeed = 5;
    
    
    public GameObject heldObject = null;
    private List<Color> playerColours = new List<Color>() { Color.red, Color.blue, Color.magenta, Color.green };
    
    private void Awake()
    {
        Speed = _baseSpeed;
        Multiplier = 1;
        heldObject = this.gameObject;
        
        PlayerSetup();

       
    }

    private void PlayerSetup()
    {
        amountOfPlayers++;
        PlayerNum = amountOfPlayers;
        PlayerColor = playerColours[PlayerNum - 1];
        
        // Assign model claw tips colour
        List<Renderer> ChildrenRenderer = GetComponentsInChildren<Renderer>(true).Where(ren => ren.material.name.Contains("Tips")).ToList();
        ChildrenRenderer.ForEach(ren => ren.material.color = playerColours[PlayerNum - 1]);
        
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

    public void AddPoints(int amount)
    {
        Points += amount * Multiplier;
    }
}
