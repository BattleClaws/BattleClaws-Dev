using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameType
{
    Basic,
    BestOf
}

public class RoundManager : MonoBehaviour
{
    public static int currentRoundNumber;
    private static bool roundEnded;
    private GameObject endRoundPanel;
    public static bool draw = false;

    public static GameType gameStyle;
    public static int roundTime;
    public static int roundAmount;

    [Space] public GameObject CirclePlatform;
    
    [Header("Round Number Text Assets")]
    public TextMeshProUGUI roundNumberTMP;
    public TextMeshProUGUI roundMaxTMP;
   

    public GameObject Timer { get; private set; }
    private float secondsRemaining = 0;

    private void Start()
    {
        StartCoroutine(GameUtils.live.OpenedScene());
        //SceneManager.sceneLoaded += OnSceneLoaded;

        roundTime = PlayerPrefs.GetInt("RTime");
        roundAmount = PlayerPrefs.GetInt("RAmount");

        // Accounts for SceneManager.sceneLoaded event not being set on first run
        if (currentRoundNumber == 0 || (!draw && SceneManager.GetActiveScene().name == "Round"))
        {
            currentRoundNumber++;
        }
        
        GameObject.Find("Round").GetComponent<TMP_Text>().text = currentRoundNumber.ToString();
        
        GameUtils.instance.audioPlayer.PlayChosenClip("Gameplay/Sequencing/GameStart");

        SceneReload();
        UpdateRound();
    }

    private void SceneReload()
    {
        print("Executing Scene Reload Sequence");

        Timer = GameObject.Find("Time");
        secondsRemaining = (draw)? 360 : (gameStyle == GameType.Basic)? 30:roundTime;  //60 - (currentRoundNumber -1 * 10);
        
        
        var activePlayers = FindObjectsByType<PlayerController>(FindObjectsSortMode.None)
            .Where(player => player.Properties.eliminated == false).ToList();
        foreach (var playerController in activePlayers)
        {
            playerController.Invisible(false);
        }

        StartCoroutine(SpawnBuffer(activePlayers));

        if (!draw)
        {
            switch (gameStyle)
            {
                case GameType.Basic:
                    if ((activePlayers.Count <= 1) && currentRoundNumber > 1) SceneManager.LoadScene("EndGame");

                    if (draw)
                    {
                        activePlayers.Where(p => !p.Properties.isDrawPlayer).ToList()
                            .ForEach(p => p._roundActive = false);
                        activePlayers.Where(p => !p.Properties.isDrawPlayer).ToList().ForEach(p => p.Invisible(true));
                    }

                    break;
                case GameType.BestOf:
                    if (currentRoundNumber > roundAmount) SceneManager.LoadScene("EndGame");
                    break;
            }
        }
        else
        {
            //StartCoroutine(PlatformReduction());
        }
    }

    public void UpdateRound()
    {
        roundNumberTMP.text = RoundManager.currentRoundNumber.ToString();

        roundMaxTMP.text = "/" + RoundManager.roundAmount;

        if (RoundManager.gameStyle == GameType.Basic) roundMaxTMP.text = "";
    }


    private IEnumerator SpawnBuffer(List<PlayerController> active)
    {
        yield return new WaitForSeconds(0.5f);

        foreach (var playerController in active)
        {
            playerController.StopCoroutines();
            print("Resetting player position: " + playerController.Properties.PlayerNum + " at position " + playerController.Position);
            playerController.Properties.RoundReset();
            print("Changed player position: " + playerController.Properties.PlayerNum + " to " + playerController.Position);
            playerController._roundActive = true;
        }
        InvokeRepeating(nameof(UpdateTimer), 0f, 1f);
    }

    private IEnumerator PlatformReduction()
    {
        Vector3 scale = CirclePlatform.transform.localScale;
        while (scale.x > 0.5f)
        {
            scale -= new Vector3(0.001f, 0, 0.001f);
            yield return new WaitForSeconds(0.01f);
            CirclePlatform.transform.localScale = scale;
        }
    }

    private void UpdateTimer()
    {
        if (GameUtils.isMenuOpen)
        {
            return;
        }

        secondsRemaining--;
        var formatTime = $"{Mathf.Floor(secondsRemaining / 60):0}:{secondsRemaining % 60:00}";
        Timer.GetComponent<TMP_Text>().text = formatTime;

        if (draw)
        {
            var drawnPlayers = FindObjectsByType<PlayerController>(FindObjectsSortMode.None).Where(player => player.Properties.isDrawPlayer).Where(player => !player.Properties.eliminated).ToList();
            //print(drawnPlayers.Count + "  Found!");
            if (drawnPlayers.OrderBy(p => p.Properties.Points).Last().Properties.Points >= 300)
            {
                StartCoroutine(EndRoundDraw());
                CancelInvoke(nameof(UpdateTimer));
            }
            
            
        }

        if (secondsRemaining <= 0)
        {
            GameUtils.instance.audioPlayer.PlayChosenClip("Gameplay/Sequencing/TimerEnd");
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
        activePlayers.ForEach(p => p._roundActive = false);

        yield return new WaitForSeconds(1.5f);
        var NoticePrefab = Resources.Load<GameObject>("Prefabs/EffectAnnouncer");
        
        var playersByScore = activePlayers.OrderBy(player => player.Properties.Points).ToList();
        var lowestScoring = playersByScore
            .Where(x => x.Properties.Points == playersByScore.First().Properties.Points).ToList();
        var highestScoring = playersByScore
            .Where(x => x.Properties.Points == playersByScore.Last().Properties.Points).ToList();

        switch (gameStyle)
        {
            case GameType.Basic:
                if (lowestScoring.Count > 1)
                {
                    yield return new WaitForSeconds(1f);
                    var drawingPlayers = "| ";
                    lowestScoring.ForEach(x=> drawingPlayers += "Player " + x.Properties.PlayerNum + " | ");

                    GameUtils.instance.GenericAnnouncement(drawingPlayers, "DRAW!");
                    yield return new WaitForSeconds(2f);

            
                    lowestScoring.ForEach(player => player.Properties.isDrawPlayer = true);

                    draw = true;
                    StartCoroutine(GameUtils.live.ChangeScene("DrawTutorial"));
                    yield return null;
                }
                else
                {
                    var playerToElim = lowestScoring[0];
                    yield return new WaitForSeconds(0.2f);
                    yield return ZoomToPlayer(playerToElim);
            
                    GameUtils.instance.AnnounceEliminatedPlayer(playerToElim.Properties.PlayerNum);
                    /*var noticeInstance = Instantiate(NoticePrefab, GameUtils.UICanvas.transform);
                    noticeInstance.transform.GetChild(0).GetComponent<TMP_Text>().text = "Eliminated";
                    noticeInstance.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Player " + playerToElim.Properties.PlayerNum;*/
        
                    yield return new WaitForSeconds(5f);
                    playerToElim.Eliminate();
                    draw = false;
                    activePlayers.ForEach(player => player.Properties.isDrawPlayer = false);
            
                    StartCoroutine(GameUtils.live.ChangeScene("Round"));
                }

                break;
            
            case GameType.BestOf:
                if (highestScoring.Count == 1)
                {
                    yield return new WaitForSeconds(1f);
                    yield return ZoomToPlayer(highestScoring[0]);

                    GameUtils.instance.AnnounceEliminatedPlayer(highestScoring[0].Properties.PlayerNum);
                    /*var noticeInstance = Instantiate(NoticePrefab, GameUtils.UICanvas.transform);
                    noticeInstance.transform.GetChild(0).GetComponent<TMP_Text>().text = "Round Winner!";
                    noticeInstance.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text =
                        "Player " + highestScoring[0].Properties.PlayerNum;*/
                    highestScoring[0].Properties.sessionWins++;
                    yield return new WaitForSeconds(5f);

                    draw = false;
                    StartCoroutine(GameUtils.live.ChangeScene("Round"));
                }
                else
                {
                    highestScoring.ForEach(x => x.Properties.sessionWins++);
                    var drawingPlayers = "| ";
                    lowestScoring.ForEach(x=> drawingPlayers += "Player " + x.Properties.PlayerNum + " | ");

                    GameUtils.instance.GenericAnnouncement(drawingPlayers, "DRAW!");
                    yield return new WaitForSeconds(2f);
                    
                    draw = false;
                    StartCoroutine(GameUtils.live.ChangeScene("Round"));
                }
                break;
        }
        
        
        
        foreach (var playerController in activePlayers)
        {
            if (playerController.Properties.heldObject != playerController.gameObject)
            {
                Destroy(playerController.Properties.heldObject.gameObject);
                playerController.Properties.heldObject = playerController.gameObject;
            }
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
        drawnPlayers.ForEach(p => p._roundActive = false);
        yield return new WaitForSeconds(0.7f);
        PlayerController winningPlayer = null;
        try
        {
            winningPlayer = drawnPlayers.OrderBy(p => p.Properties.Points).Last();
        }
        catch {
            // Empty
        }

        drawnPlayers.ForEach(player => player.Properties.isDrawPlayer = false);
        drawnPlayers.Remove(winningPlayer);
        drawnPlayers.ForEach(player => print(player.Properties.PlayerNum + " |  eliminated" ));

        yield return new WaitForSeconds(1f);
        
        GameUtils.instance.AnnounceEliminatedPlayer(winningPlayer.Properties.PlayerNum);
        yield return new WaitForSeconds(2f);
        drawnPlayers.ForEach(player => player.Eliminate());
        draw = false;

        StartCoroutine(GameUtils.live.ChangeScene("Round"));
    }
}