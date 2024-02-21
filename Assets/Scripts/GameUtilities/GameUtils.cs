using UnityEngine;

public class GameUtils : MonoBehaviour
{
    public static GameObject UICanvas;

    private void Start()
    {
        UICanvas = GameObject.FindGameObjectWithTag("UI");
    }

    // Update is called once per frame
    private void Update()
    {
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

    public static void PointsAlert(int amount, Transform position)
    {
        // this will show the amount of points added when the ball is dropped into the dropzone
    }
}