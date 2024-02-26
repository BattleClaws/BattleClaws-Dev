using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    private static int currentRoundNumber;
    private static int maxNumberOfRounds;
    private static bool roundEnded;
    private GameObject endRoundPanel;

    public GameObject Timer { get; private set; }
    private float secondsRemaining = 0;

    private void Start()
    {
        DontDestroyOnLoad(gameObject); // preserve this object across scenes
        
        currentRoundNumber = 1;
        maxNumberOfRounds = 4 - 1;
        print($"current: {currentRoundNumber} | max: {maxNumberOfRounds}");
        SceneReload();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneReload();
        print("Scene Loaded");
    }

    private void SceneReload()
    {
        if (currentRoundNumber > maxNumberOfRounds) SceneManager.LoadScene("EndGame");
        Timer = GameObject.Find("Time");
        secondsRemaining = 10;  //60 - (currentRoundNumber -1 * 10);
        InvokeRepeating(nameof(UpdateTimer), 1f, 1f);
        
        var activePlayers = FindObjectsByType<PlayerController>(FindObjectsSortMode.None)
            .Where(player => player.eliminated == false).ToList();
        activePlayers.ForEach(p => p._roundActive = true);
    }

    private void UpdateTimer()
    {
        secondsRemaining--;
        var formatTime = $"{Mathf.Floor(secondsRemaining / 60):0}:{secondsRemaining % 60:00}";
        Timer.GetComponent<TMP_Text>().text = formatTime;

        if (secondsRemaining <= 0)
        {
            StartCoroutine(EndRound());
            CancelInvoke(nameof(UpdateTimer));
        }
    }


    private IEnumerator EndRound()
    {
        var activePlayers = FindObjectsByType<PlayerController>(FindObjectsSortMode.None)
            .Where(player => player.eliminated == false).ToList();
        var playersByScore = activePlayers.OrderBy(player => player.Properties.Points).ToList();
        activePlayers.ForEach(p => p._roundActive = false);

        var playerToElim = playersByScore.Last();

        yield return new WaitForSeconds(1f);
        yield return ZoomToPlayer(playerToElim);

        var NoticePrefab = Resources.Load<GameObject>("Prefabs/EffectAnnouncer");
        var noticeInstance = Instantiate(NoticePrefab, GameUtils.UICanvas.transform);
        noticeInstance.transform.GetChild(0).GetComponent<TMP_Text>().text = "Eliminated";
        noticeInstance.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Player " + playerToElim.Properties.PlayerNum;
        
        yield return new WaitForSeconds(2f);
        
        if (playerToElim.Properties.Points == playersByScore[-2].Properties.Points)
        {
            // do draw logic
        }
        
        activePlayers.ForEach(player => player.Properties.RoundReset());
        playerToElim.Eliminate();

        SceneManager.LoadScene("Round");
    }

    private IEnumerator ZoomToPlayer(PlayerController player)
    {
        var baseLoc = Camera.main.transform.position;
        var goalLoc = player.Position + new Vector3(0, 1, -4);
        
        for(float i = 0; i < 1.1f; i += 0.17f)
        {
            var newPosition = Vector3.Lerp(baseLoc, goalLoc, i);
            Camera.main.transform.position = newPosition;
            yield return new WaitForSeconds(0.02f);
        }
    }
}