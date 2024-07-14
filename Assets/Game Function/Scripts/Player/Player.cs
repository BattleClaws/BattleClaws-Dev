using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public bool isDrawPlayer;
    // Allows newly spawned claws to know their id
    public static int amountOfPlayers = 0;
    public int sessionWins;
    
    
    // Properties
    public int PlayerNum { get; private set; }
    public Color PlayerColor { get; private set; }
    public int Speed { get; private set; }
    public int Points { get; set; }
    public int LegacyPoints { get; set; }
    public int Multiplier { get; private set; }
    //public GameObject ScorePanel { get; private set; }

    //public TMP_Text ScoreDisplay { get; set; }

    public GameObject CamAnchor { get; set; }

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
    public bool eliminated = false;
    private PlayerController _controller;
    
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
        _controller = GetComponent<PlayerController>();
        PlayerSetup();
        
        charge = Resources.Load<Material>("Materials/SuperCharge");
        CamAnchor = _controller._handle.transform.Find("Cam Anchor").gameObject;
        print("awake!");


    }

    private void PlayerSetup()
    {
        _controller.Position = GameUtils.RequestSpawnLocation(PlayerNum).position;
        //print("Position: " + GameUtils.RequestSpawnLocation(PlayerNum).position);
        //StartCoroutine(DoubleSetPosition());

        TMP_Text playerNum = transform.GetComponentInChildren<TMP_Text>(true);
        playerNum.text = "P" + PlayerNum;
        playerNum.color = PlayerColor;
        
        // Assign model claw tips colour
        List<Renderer> ChildrenRenderer = GetComponentsInChildren<Renderer>(true).Where(ren => ren.material.name.Contains("Tips")).ToList();
        ChildrenRenderer.ForEach(ren => ren.material.color = playerColours[PlayerNum - 1]);
        
        if (!RoundManager.draw && SceneManager.GetActiveScene().name == "Round" && !eliminated)
            SpawnPointstracker();

        /*if (heldObject != gameObject)
        {
            Destroy(heldObject);
            heldObject = gameObject;
        }*/
    }
    
    // TEMP FIX
    /*public IEnumerator DoubleSetPosition()
    {
        yield return new WaitForSeconds(0.5f);
        _controller.Position = GameUtils.RequestSpawnLocation(PlayerNum).position;
    }*/

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
        GameUtils.instance.SendScoreToUI(PlayerNum, Points);
        
        /*var scorePrefab = Resources.Load<GameObject>("Prefabs/Score");
        ScorePanel = Instantiate(scorePrefab, GameUtils.UICanvas.transform);
        ScoreDisplay = ScorePanel.transform.Find("Score").GetComponent<TMP_Text>();

        ScorePanel.GetComponent<RectTransform>().anchoredPosition = GameUtils.ScorePosition(PlayerNum);

        var _PlayerNumDisplay = ScorePanel.transform.Find("Player").GetComponent<TMP_Text>();
        _PlayerNumDisplay.text = PlayerNum.ToString();
        _PlayerNumDisplay.color = playerColours[PlayerNum - 1];*/
    }

    public IEnumerator SpeedEffect(int amount, float length, bool colorEffect)
    {
        Speed = amount;
        if(!effectActive && colorEffect)
            GetComponentsInChildren<Renderer>(true).ToList().ForEach(x=> StartCoroutine(MultiplierGlow(x, Color.blue, length)));
        yield return new WaitForSeconds(length);
        Speed = _baseSpeed;
    }

    public void ApplySpeed(int amount, float length)
    {
        StartCoroutine(SpeedEffect(amount, length, true));
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
        GameUtils.instance.SendScoreToUI(PlayerNum, Points);
    }

    public void RoundReset()
    {
        LegacyPoints += Points;
        Points = 0;
        heldObject = gameObject;
        PlayerSetup();
    }

    private void OnDestroy()
    {
        print("Destroy");
    }
}
