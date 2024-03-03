using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Allows newly spawned claws to know their id
    public static int amountOfPlayers = 0;
    
    
    // Properties
    public int PlayerNum { get; private set; }
    public Color PlayerColor { get; private set; }
    public int Speed { get; private set; }
    public int Points { get; set; }
    public int LegacyPoints { get; private set; }
    public int Multiplier { get; private set; }
    public GameObject ScorePanel { get; private set; }

    public TMP_Text ScoreDisplay { get; set; }

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
    private Material charge;
    private bool effectActive = false;
    
    public GameObject heldObject = null;
    private List<Color> playerColours = new List<Color>() { Color.red, Color.blue, Color.magenta, Color.green };

    private void Awake()
    {
        print("Spawn");
        Speed = _baseSpeed;
        Multiplier = 1;
        heldObject = this.gameObject;
        
        amountOfPlayers++;
        PlayerNum = amountOfPlayers;
        PlayerColor = playerColours[PlayerNum - 1];
        PlayerSetup();

        charge = Resources.Load<Material>("Materials/SuperCharge");


    }

    private void PlayerSetup()
    {
        transform.position = GameUtils.RequestSpawnLocation(PlayerNum).position;

        // Assign model claw tips colour
        List<Renderer> ChildrenRenderer = GetComponentsInChildren<Renderer>(true).Where(ren => ren.material.name.Contains("Tips")).ToList();
        ChildrenRenderer.ForEach(ren => ren.material.color = playerColours[PlayerNum - 1]);
        
        SpawnPointstracker();
        
    }

    private IEnumerator MultiplierGlow(Renderer renderer, Color color, float length)
    {
        effectActive = true;
        var originalMaterial = renderer.material;
        renderer.material = charge;
        renderer.material.color = color;

        yield return new WaitForSeconds(length);

        renderer.material = originalMaterial;
        effectActive = false;
    }

    private void SpawnPointstracker()
    {
        var scorePrefab = Resources.Load<GameObject>("Prefabs/Score");
        ScorePanel = Instantiate(scorePrefab, GameUtils.UICanvas.transform);
        ScoreDisplay = ScorePanel.transform.Find("Score").GetComponent<TMP_Text>();

        ScorePanel.GetComponent<RectTransform>().anchoredPosition = GameUtils.ScorePosition(PlayerNum);

        var _PlayerNumDisplay = ScorePanel.transform.Find("Player").GetComponent<TMP_Text>();
        _PlayerNumDisplay.text = PlayerNum.ToString();
        _PlayerNumDisplay.color = playerColours[PlayerNum - 1];
    }

    public IEnumerator SpeedEffect(int amount, float length)
    {
        Speed = amount;
        if(!effectActive)
            GetComponentsInChildren<Renderer>(true).ToList().ForEach(x=> StartCoroutine(MultiplierGlow(x, Color.blue, length)));
        yield return new WaitForSeconds(length);
        Speed = _baseSpeed;
    }

    public void ApplySpeed(int amount, float length)
    {
        StartCoroutine(SpeedEffect(amount, length));
    }
    
    public void ApplyMultiplier(int amount, float length)
    {
        StartCoroutine(MultiplierEffect(amount, length));
    }

    public IEnumerator MultiplierEffect(int amount, float length)
    {
        Multiplier *= amount;
        if(!effectActive)
            GetComponentsInChildren<Renderer>(true).ToList().ForEach(x=> StartCoroutine(MultiplierGlow(x, Color.yellow, length)));
        yield return new WaitForSeconds(length);
        Multiplier = 1;
    }

    public int AddPoints(int amount)
    {
        Points += amount * Multiplier;
        UpdateScoreDisplay();
        return amount * Multiplier;
    }

    private void UpdateScoreDisplay()
    {
        ScoreDisplay.text = Points.ToString().PadLeft(6, '0');
    }

    public void RoundReset()
    {
        LegacyPoints += Points;
        Points = 0;
        heldObject = null;
        PlayerSetup();
    }

    private void OnDestroy()
    {
        print("Destroy");
    }
}
