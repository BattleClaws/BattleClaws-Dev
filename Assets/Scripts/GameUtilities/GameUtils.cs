using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        
        InitDropZones();
    }

    public static void InitCollectables()
    {
        
    }

    public static void InitDropZones()
    {
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
        print(_enteredColors[1]);
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