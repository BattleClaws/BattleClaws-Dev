using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameUtils : MonoBehaviour
{
    public static GameObject UICanvas;
    
    [SerializeField] private List<Color> colors = new List<Color>();
    [SerializeField] private List<Transform> PlayerSpawns = new List<Transform>();
    private static List<Color> _enteredColors;
    private static List<Transform> _playerSpawns;
    private static GameObject ScoreIndicator;

    [Space] [Header("DropZone Properties")]
    public Vector3 zoneScale;
    private static Vector3 _zoneScale;
    private static List<Transform> _dropZoneSpawns;
    [SerializeField] private List<Transform> dropZoneSpawns = new List<Transform>();

    private static List<GameObject> _dropZones = new List<GameObject>();

    private static List<string> _effects = new List<string>() { "LockDown", "DoublePoints", "SpeedBoost", "ShuffleZones", "FreezeMetal" }; 

    private void Awake()
    {
        _zoneScale = zoneScale;
        _enteredColors = colors;
        _playerSpawns = PlayerSpawns;
        _dropZoneSpawns = dropZoneSpawns;
        Collectable.SetValue(100);
    }
    
    private void Start()
    {
        ScoreIndicator = Resources.Load<GameObject>("Prefabs/Score Indicator");
        UICanvas = GameObject.FindGameObjectWithTag("UI");
        
        InitDropZones(true);
        Repeat(200, InitCollectables);
        
    }
    
    public static void Repeat(int count, Action action)
    {
        for (int i = 0; i < count; i++)
        {
            action();
        }
    }

    public static void InitCollectables()
    {
        var collectable = Resources.Load<GameObject>("Prefabs/Collectable");
        Vector3 randomLoc = Random.insideUnitCircle;
        randomLoc = new Vector3(randomLoc.x, 0.3f, randomLoc.y) * 7;
        Instantiate(collectable, randomLoc, Quaternion.identity);
    }

    public static void SpecialAction(PlayerController user)
    {
        var effect = _effects[Random.Range(0, _effects.Count)];

        print(effect);
        switch (effect)
        {
            case "DoublePoints":
                user.Properties.ApplyMultiplier(2, 10);
                break;
            case "SpeedBoost":
                user.Properties.ApplySpeed(8, 10);
                break;
            case "ShuffleZones":
                InitDropZones(false);
                break;
            case "FreezeMetal":
                FindObjectsOfType<PlayerController>().Where(p => p!=user).ToList()
                    .ForEach(p=> p.Properties.ApplySpeed(1, 2));
                break;
            case "LockDown":
                var Zones = GameObject.FindGameObjectsWithTag("DropZone").ToList();
                var ZoneToLock = Zones[Random.Range(0, Zones.Count)];
                
                FindObjectOfType<GameUtils>().LockZone(ZoneToLock);
                break;
            default:
                break;
        }
    }

    public void LockZone(GameObject zone)
    {
        StartCoroutine(LockZoneInstance(zone));
    }

    private IEnumerator LockZoneInstance(GameObject zone)
    {
        Color originalColor = zone.GetComponent<Renderer>().material.color;
        zone.GetComponent<Renderer>().material.color = Color.gray;
        yield return new WaitForSeconds(10f);
        zone.GetComponent<Renderer>().material.color = originalColor;
    }

    public static void InitDropZones(bool isStart)
    {
        if (!isStart)
        {
            GameObject.FindGameObjectsWithTag("DropZone").ToList().ForEach(Destroy);
            //Randomise the colours so the dropzones actually shuffle :3
            _enteredColors = _enteredColors.OrderBy(_ => Guid.NewGuid()).ToList();
        }

        for(int i = 0; i < _dropZoneSpawns.Count; i++){
            var newZone = GameObject.CreatePrimitive(PrimitiveType.Cube);
            newZone.transform.localScale = _zoneScale;
            newZone.transform.position = _dropZoneSpawns[i].position;
            newZone.GetComponent<Renderer>().material.color = _enteredColors[i];
            newZone.tag = "DropZone";
            
            _dropZones.Add(newZone);
        }
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
    
    public static Color RequestColor()
    {
        return _enteredColors[Random.Range(0, _enteredColors.Count)];
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
}