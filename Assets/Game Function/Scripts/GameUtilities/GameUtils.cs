using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class GameUtils : MonoBehaviour
{
    public static GameObject UICanvas;
    
    [SerializeField] private List<Color> colors;
    [SerializeField] private List<Transform> PlayerSpawns;
    private static List<Color> _enteredColors;
    private static List<Transform> _playerSpawns;
    private static GameObject ScoreIndicator;
    private static GameObject EffectIndicator;
    private static GameObject DropParticles;
    public static MenuManager menuManager;
    public BannerHandler eliminationAnnouncer;
    public GenericAnnoucementHandler genericAnnouncement;
    public UIScoreManager uIScoreManager;
    public PlayerAnnocementHandler playerAnnocementHandler;
    public AudioManager audioPlayer;

    public static GameUtils instance;
    public static bool isMenuOpen;
    public static GameUtils live;

    public float spawnOffset;
    public int spawnAmount;
    public int spawnRadius;
    

    [Space] [Header("DropZone Properties")]
    public Vector3 zoneScale;
    private static Vector3 _zoneScale;
    private static List<Transform> _dropZoneSpawns;
    private static List<GameObject> _dropZoneHolograms;
    private static Vector3 _hologramOffset;
    [SerializeField] private List<Transform> dropZoneSpawns = new List<Transform>();
    [SerializeField] private List<GameObject> dropZoneHolograms = new List<GameObject>();
    [SerializeField] private Vector3 hologramsOffset;
    private static List<GameObject> _dropZones = new List<GameObject>();


    private static List<string> _effects = new List<string>() { "LockDown", "DoublePoints", "SpeedBoost", "ShuffleZones", "FreezeMetal" };

    private bool isShakeActive;

    private void Awake()
    {
        //Application.targetFrameRate = 60;
        _zoneScale = zoneScale;
        _enteredColors = colors;
        _playerSpawns = PlayerSpawns;
        _dropZoneSpawns = dropZoneSpawns;
        _hologramOffset = hologramsOffset;
        _dropZoneHolograms = dropZoneHolograms;
        UICanvas = GameObject.FindGameObjectWithTag("UI");
        Collectable.SetValue(100);
        uIScoreManager = GameObject.FindObjectOfType<UIScoreManager>();
        eliminationAnnouncer = FindObjectOfType<BannerHandler>();
        genericAnnouncement = FindObjectOfType<GenericAnnoucementHandler>();
        menuManager = GameObject.FindObjectOfType<MenuManager>();
        playerAnnocementHandler = FindObjectOfType<PlayerAnnocementHandler>();
        audioPlayer = FindObjectOfType<AudioManager>();

        live = this;
    }

    public void AnnounceEliminatedPlayer(int playerNum)
    {
        eliminationAnnouncer.EliminationAnnounce(playerNum);
    }

    public void GenericAnnouncement(string line1, string line2)
    {
        genericAnnouncement.StartAnnounce(line1, line2);
    }

    public void SetMenuVisibility(bool isOn)
    {
        menuManager.SetVisibility(isOn);
    }

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        ScoreIndicator = Resources.Load<GameObject>("Prefabs/Score Indicator");
        EffectIndicator = Resources.Load<GameObject>("Prefabs/EffectAnnouncer");
        DropParticles = Resources.Load<GameObject>("Prefabs/DropZone Particles");
        UICanvas = GameObject.FindGameObjectWithTag("UI");

        InitDropZones(true);
        Repeat(spawnAmount, InitCollectables);
        
        /*if (!RoundManager.draw && SceneManager.GetActiveScene().name == "Round")
        {
            
        }*/

    }
    
    public static void Repeat(int count, Action action)
    {
        for (int i = 0; i < count; i++)
        {
            action();
        }
    }

    public void InitCollectables()
    {
        var collectable = Resources.Load<GameObject>("Prefabs/Collectable");
        Vector3 randomLoc = Random.insideUnitCircle;
        randomLoc = new Vector3(randomLoc.x, spawnOffset, randomLoc.y) * spawnRadius;
        Instantiate(collectable, randomLoc, Quaternion.identity);
    }

    public void SendScoreToUI(int playerNum, int score)
    {
        uIScoreManager.InjectScore(playerNum, score);
    }

    public static void EffectNotification(string effect, int player)
    {
        var newNotificationObj = Instantiate(EffectIndicator, UICanvas.transform);
        var EffectText = newNotificationObj.transform.GetChild(0).GetComponent<TMP_Text>();
        EffectText.text = effect.Prettify();

        EffectText.transform.GetChild(0).GetComponent<TMP_Text>().text = "Player " + player.ToString() + " Activated";
    }

    public void LockZone(GameObject zone)
    {
        StartCoroutine(LockZoneInstance(zone));
    }
    
    public IEnumerator CamShake(float shakeDuration)
    {
        if (!isShakeActive)
        {
            isShakeActive = true;
            var originalPos = Camera.main.transform.localPosition;
            while (shakeDuration > 0)
            {
                Camera.main.transform.localPosition = originalPos + UnityEngine.Random.insideUnitSphere * 0.23f;

                shakeDuration -= Time.deltaTime * 0.7f;
                yield return new WaitForFixedUpdate();
            }

            shakeDuration = 0f;
            Camera.main.transform.localPosition = originalPos;
            isShakeActive = false;
        }
    }

    private static void ZoneParticles(GameObject zone)
    {
        var pSystem = Instantiate(DropParticles, zone.transform);
        var systemMain = pSystem.GetComponent<ParticleSystem>().main;
        systemMain.startColor = zone.GetComponent<Renderer>().material.color + new Color(0,0,0,1);
        pSystem.GetComponent<ParticleSystem>().Play();
    }

    private IEnumerator LockZoneInstance(GameObject zone)
    {
        Color originalColor = zone.GetComponent<Renderer>().material.color;
        zone.GetComponent<Renderer>().material.color = Color.gray;
        ZoneParticles(zone);
        yield return new WaitForSeconds(8f);
        zone.GetComponent<Renderer>().material.color = originalColor;
        ZoneParticles(zone);
    }

    public static void InitDropZones(bool isStart)
    {
        if (!isStart)
        {
            GameObject.FindGameObjectsWithTag("DropZone").ToList().ForEach(Destroy);
            //Randomise the colours so the dropzones actually shuffle :3
            _enteredColors = _enteredColors.OrderBy(_ => Guid.NewGuid()).ToList();
            _dropZones = new List<GameObject>();
        }

        for(int i = 0; i < _dropZoneSpawns.Count; i++){
            var newZone = GameObject.CreatePrimitive(PrimitiveType.Cube);
            newZone.transform.localScale = _zoneScale;
            newZone.transform.position = _dropZoneSpawns[i].position;
            newZone.GetComponent<Renderer>().material.color = _enteredColors[i];
            newZone.tag = "DropZone";
            newZone.layer = LayerMask.NameToLayer("Collectables");
            ZoneParticles(newZone);

            if (RoundManager.draw)
            {
                newZone.GetComponent<Renderer>().enabled = false;
            }

            _dropZones.Add(newZone);
            
            InitDropZoneHologram(_enteredColors[i], _dropZoneSpawns[i].position);
        }
    }

    private static void InitDropZoneHologram(Color inputColor, Vector3 pos)
    {
        pos = pos + _hologramOffset;
        
        int hologramId = 0;

        Color[] validColors =
        {
            new Color(1f, 0.75686276f, 0.02745098f), new Color(0f, 0.3019608f, 0.2509804f),
            new Color(0.84705883f, 0.105882354f, 0.3764706f), new Color(0.11764706f, 0.53333336f, 0.8980392f)
        };

        if (inputColor == validColors[0])
        {
            hologramId = 0;
        }
        else if (inputColor == validColors[1])
        {
            hologramId = 1;
        }
        else if (inputColor == validColors[2])
        {
            hologramId = 2;
        }
        else
        {
            hologramId = 3;
        }
        
        
        GameObject newHologram = Instantiate(_dropZoneHolograms[hologramId], pos, Quaternion.identity);
        print(newHologram);
    }

    public static IEnumerator LerpToLocalPosition(GameObject obj, Vector3 goal, float delay)
    {
        Vector3 startPos = obj.transform.localPosition;

        for (float i = 0; i < 1; i += 0.3f)
        {
            obj.transform.localPosition = Vector3.Lerp(startPos, goal, i);
            yield return new WaitForSeconds(delay);
        }

        obj.transform.localPosition = goal;
    }

    public static T FindResource<T>(string path) where T : class
    {
        return Resources.Load(path, typeof(T)) as T;
    }

    public static Vector2 ScorePosition(int playerNum)
    {
        return playerNum switch
        {
            1 => new Vector2(100, -30),
            2 => new Vector2(1600, -30),
            3 => new Vector2(100, -1000),
            4 => new Vector2(1600, -1000),
            _ => Vector2.zero
        };
    }

    public IEnumerator ChangeScene(string scene)
    {
        var pane = GameObject.Find("Darkening").gameObject;

        for (float i = 0; i <= 1.0f; i += 0.05f)
        {
            pane.GetComponent<UnityEngine.UI.Image>().color = new Color(0, 0, 0, i);
            yield return new WaitForFixedUpdate();
        }
        pane.GetComponent<UnityEngine.UI.Image>().color = new Color(0, 0, 0, 1);

        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);
    }
    
    public IEnumerator OpenedScene()
    {
        var pane = GameObject.Find("Darkening").gameObject;
        pane.GetComponent<UnityEngine.UI.Image>().color = new Color(0, 0, 0, 1);
        yield return new WaitForSeconds(0.5f);
        for (float i = 1; i > 0.0f; i -= 0.05f)
        {
            pane.GetComponent<UnityEngine.UI.Image>().color = new Color(0, 0, 0, i);
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(0.1f);
    }
    

    public static Transform RequestSpawnLocation(int playerNum)
    {
        return _playerSpawns[playerNum -1];
    }

    public static void ScoreNotication(int score, Transform position)
    {
        var newScoreObj = Instantiate(ScoreIndicator, position.position + new Vector3(0,1,0), Quaternion.identity);
        newScoreObj.GetComponentInChildren<TMP_Text>().text = "+" + score;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Splash");
        }
    }
}