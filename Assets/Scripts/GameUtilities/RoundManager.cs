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
    public bool draw = false;

    


    public GameObject Timer { get; private set; }
    private float secondsRemaining = 0;

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
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
        if (!draw)
            currentRoundNumber++;
        if (currentRoundNumber > maxNumberOfRounds) SceneManager.LoadScene("EndGame");
        Timer = GameObject.Find("Time");
        secondsRemaining = (draw)? 15 : 10;  //60 - (currentRoundNumber -1 * 10);
        InvokeRepeating(nameof(UpdateTimer), 1f, 1f);
        
        var activePlayers = FindObjectsByType<PlayerController>(FindObjectsSortMode.None)
            .Where(player => player.eliminated == false).ToList();
        activePlayers.ForEach(p => p._roundActive = true);

        if (draw) 
        {
            activePlayers.Where(p => !p.Properties.isDrawPlayer).ToList().ForEach(p => p._roundActive = false);
        }
    }

    private void UpdateTimer()
    {
        secondsRemaining--;
        var formatTime = $"{Mathf.Floor(secondsRemaining / 60):0}:{secondsRemaining % 60:00}";
        Timer.GetComponent<TMP_Text>().text = formatTime;

        if (secondsRemaining <= 0)
        {
            if (draw)
            {
                StartCoroutine(EndRoundDraw());
                CancelInvoke(nameof(UpdateTimer));
            }

            else
            {
                StartCoroutine(EndRound());
                CancelInvoke(nameof(UpdateTimer));
            }
        }
    }


    private IEnumerator EndRound()
    {
        var activePlayers = FindObjectsByType<PlayerController>(FindObjectsSortMode.None)
            .Where(player => player.eliminated == false).ToList();
        var playersByScore = activePlayers.OrderBy(player => player.Properties.Points).ToList();
        activePlayers.ForEach(p => p._roundActive = false);

        var lowestScoring = playersByScore
            .Where(x => x.Properties.Points == playersByScore.First().Properties.Points).ToList();
        
        var NoticePrefab = Resources.Load<GameObject>("Prefabs/EffectAnnouncer");
        if (lowestScoring.Count > 1)
        {
            yield return new WaitForSeconds(1f);
            var noticeInstance = Instantiate(NoticePrefab, GameUtils.UICanvas.transform);
            noticeInstance.transform.GetChild(0).GetComponent<TMP_Text>().text = "DRAW!";
            var drawingPlayers = "| ";
            lowestScoring.ForEach(x=> drawingPlayers += "Player " + x.Properties.PlayerNum + " | ");
            noticeInstance.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = drawingPlayers;
            yield return new WaitForSeconds(2f);

            activePlayers.ForEach(player => player.Properties.RoundReset());
            lowestScoring.ForEach(player => player.Properties.isDrawPlayer = true);

            draw = true;
            SceneManager.LoadScene("Draw");
            yield return null;
        }
        else
        {
            var playerToElim = lowestScoring[0];
            yield return new WaitForSeconds(1f);
            yield return ZoomToPlayer(playerToElim);
            
            var noticeInstance = Instantiate(NoticePrefab, GameUtils.UICanvas.transform);
            noticeInstance.transform.GetChild(0).GetComponent<TMP_Text>().text = "Eliminated";
            noticeInstance.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Player " + playerToElim.Properties.PlayerNum;
        
            yield return new WaitForSeconds(2f);
            playerToElim.Eliminate();
            draw = false;
        }

        activePlayers.ForEach(player => player.Properties.RoundReset());
        activePlayers.ForEach(player => player.Properties.isDrawPlayer = false);


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

    private IEnumerator EndRoundDraw()
    {
        var NoticePrefab = Resources.Load<GameObject>("Prefabs/EffectAnnouncer");

        var drawnPlayers = FindObjectsByType<PlayerController>(FindObjectsSortMode.None).Where(player => player.Properties.isDrawPlayer).ToList();
        var winningPlayer = drawnPlayers.First(player => player.isWinningPlayer);

        drawnPlayers.ForEach(player => player.Properties.isDrawPlayer = false);
        drawnPlayers.Remove(winningPlayer);

        yield return new WaitForSeconds(1f);
        var noticeInstance = Instantiate(NoticePrefab, GameUtils.UICanvas.transform);
        noticeInstance.transform.GetChild(0).GetComponent<TMP_Text>().text = "ROUND END!";
        noticeInstance.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Winner: Player " + winningPlayer;
        yield return new WaitForSeconds(2f);
        drawnPlayers.ForEach(player => player.Eliminate());
        draw = false;
        SceneManager.LoadScene("Round");
    }
}