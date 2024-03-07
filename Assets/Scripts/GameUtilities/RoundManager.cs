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
    public static bool draw = false;

    public GameObject Timer { get; private set; }
    private float secondsRemaining = 0;

    private void Start()
    {
        StartCoroutine(GameUtils.live.OpenedScene());
        SceneManager.sceneLoaded += OnSceneLoaded;
        
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
        print("Draw: " + draw);
        if (!draw)
        {
            currentRoundNumber++;
            GameObject.Find("Round").GetComponent<TMP_Text>().text = currentRoundNumber.ToString();
        }

        Timer = GameObject.Find("Time");
        secondsRemaining = (draw)? 15 : 30;  //60 - (currentRoundNumber -1 * 10);
        InvokeRepeating(nameof(UpdateTimer), 1f, 1f);
        
        var activePlayers = FindObjectsByType<PlayerController>(FindObjectsSortMode.None)
            .Where(player => player.Properties.eliminated == false).ToList();
        activePlayers.ForEach(p => p._roundActive = true);
        activePlayers.ForEach(player => player.Properties.RoundReset());
        
        print(currentRoundNumber);
        if ((currentRoundNumber > maxNumberOfRounds || activePlayers.Count <=1) && currentRoundNumber > 1) SceneManager.LoadScene("EndGame");

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
            .Where(player => player.Properties.eliminated == false).ToList();
        var playersByScore = activePlayers.OrderBy(player => player.Properties.Points).ToList();
        activePlayers.ForEach(p => p._roundActive = false);

        var lowestScoring = playersByScore
            .Where(x => x.Properties.Points == playersByScore.First().Properties.Points).ToList();
        
        var NoticePrefab = Resources.Load<GameObject>("Prefabs/EffectAnnouncer");
        print(lowestScoring.Count +" | "+ lowestScoring);
        if (lowestScoring.Count > 1)
        {
            yield return new WaitForSeconds(1f);
            var noticeInstance = Instantiate(NoticePrefab, GameUtils.UICanvas.transform);
            noticeInstance.transform.GetChild(0).GetComponent<TMP_Text>().text = "DRAW!";
            var drawingPlayers = "| ";
            lowestScoring.ForEach(x=> drawingPlayers += "Player " + x.Properties.PlayerNum + " | ");
            noticeInstance.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = drawingPlayers;
            yield return new WaitForSeconds(2f);

            
            lowestScoring.ForEach(player => player.Properties.isDrawPlayer = true);

            draw = true;
            StartCoroutine(GameUtils.live.ChangeScene("DrawTutorial"));
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
            activePlayers.ForEach(player => player.Properties.isDrawPlayer = false);
            
            StartCoroutine(GameUtils.live.ChangeScene("Round"));
        }
    }

    private IEnumerator ZoomToPlayer(PlayerController player)
    {
        var baseLoc = Camera.main.transform;
        var goalLoc = player.Properties.CamAnchor.transform;
        
        for(float i = 0; i < 1.1f; i += 0.17f)
        {
            var newPosition = Vector3.Lerp(baseLoc.position, goalLoc.position, i);
            Camera.main.transform.position = newPosition;
            var newRotation = Quaternion.Lerp(baseLoc.rotation, goalLoc.rotation, i);
            Camera.main.transform.rotation = newRotation;
            
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
        drawnPlayers.ForEach(player => print(player.Properties.PlayerNum + " |  eliminated" ));

        yield return new WaitForSeconds(1f);
        var noticeInstance = Instantiate(NoticePrefab, GameUtils.UICanvas.transform);
        noticeInstance.transform.GetChild(0).GetComponent<TMP_Text>().text = "ROUND END!";
        noticeInstance.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Winner: Player " + winningPlayer;
        yield return new WaitForSeconds(2f);
        drawnPlayers.ForEach(player => player.Eliminate());
        draw = false;
        Destroy(winningPlayer.Properties.heldObject);
        winningPlayer.Properties.heldObject = winningPlayer.Properties.gameObject;
        StartCoroutine(GameUtils.live.ChangeScene("Round"));
    }
}