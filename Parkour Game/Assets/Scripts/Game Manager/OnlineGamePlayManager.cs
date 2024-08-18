using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnlineGamePlayManager : MonoBehaviour
{
    #region Variables

    //Player Identification
    [Header("Player Identification")]

    [HideInInspector] public GameObject player1ObjectOnline = null;
    [HideInInspector] public GameObject player2ObjectOnline = null;

    public GameObject onlinePlayerPrefab;

    public string player1Name;
    public string player2Name;

    // Gamestates
    public enum GameState { Intro, Running, Pause, End };

    [Header("Gamestate Identification")]
    public GameState state = GameState.Intro;

    // Player Scores
    [Header("Score Settings")]
    public int player1Score; 
    public int player2Score;

    public int scorePerKill;

    public int maxScore;

    [Header("Spawnpoints")]
    public Transform player1Spawnpoint;
    public Transform player2Spawnpoint;

    [Header("Respawn Settings")]
    // Respawn Y Level Threshold
    public int respawnThreshold = -40;

    [Header("Player Kill / Death")]
    // Player Kill / Death
    public string playerDied;
    public string playerScored;

    // Timer
    [Header("Timer")]
    public float timer;
    public float gameDuration;

    public float currentTime;
    public float previousTime;

    public string displayTime;

    // UI Setting
    [Header("UI Settings")]
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public TMP_Text alertText;

    public PhotonView view;
    #endregion

    #region singleton
    public static OnlineGamePlayManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    // Start Game Setup
    public void Start()
    {
        view = GetComponent<PhotonView>();

        // If a Gamemaster exists, then use its data
        if (GameMaster.instance != null)
        {
            maxScore = GameMaster.instance.saveData.maxKills;
            gameDuration = GameMaster.instance.saveData.maxRoundTime;

            player1Name = GameMaster.instance.currentPlayer1.playerName;
            player2Name = GameMaster.instance.currentPlayer2.playerName;
        }

        state = GameState.Intro;

        player1ObjectOnline = null;
        player2ObjectOnline = null;

        view.RPC("SetupGame", RpcTarget.All);

    }

    // Setup Game
    [PunRPC]
    public void SetupGame()
    {
        // Check if the local player is player 1 and instantiate only if the object doesn't already exist
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1 && player1ObjectOnline == null)
        {
            player1ObjectOnline = PhotonNetwork.Instantiate(onlinePlayerPrefab.name, player1Spawnpoint.position, Quaternion.identity);
        }
        // Check if the local player is player 2 and instantiate only if the object doesn't already exist
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 2 && player2ObjectOnline == null)
        {
            player2ObjectOnline = PhotonNetwork.Instantiate(onlinePlayerPrefab.name, player2Spawnpoint.position, Quaternion.identity);
        }

        view.RPC("IntroSequence", RpcTarget.All);
    }

    // Start Countdown
    [PunRPC]
    public IEnumerator IntroSequence()
    {
        alertText.text = "Get Ready!";

        yield return new WaitForSeconds(1);

        int countdownTimer = 4;

        while (countdownTimer > 1)
        {
            yield return new WaitForSeconds(1);

            countdownTimer--;

            alertText.text = $"Starting in {countdownTimer}";
        }

        yield return new WaitForSeconds(1);

        alertText.text = "Go!";

        yield return new WaitForSeconds(0.5f);
        
        alertText.enabled = false;

        view.RPC("RunGame", RpcTarget.All);
    }

    [PunRPC]
    public void RunGame()
    {
        // Sets default scores to zero
        player1Score = 0;
        player2Score = 0;

        // Sets score per kill before game ends
        scorePerKill = 1;

        // Updates Timer
        currentTime = Time.time;
        timer = gameDuration;

        // Updates Player Names
        //if (player1Name == "") player1Name = "Player1";
        //if (player2Name == "") player2Name = "Player2";

        // Sets score of both players to zero
        scoreText.text = $"{player1Name} - {player1Score} | {player2Name} - {player2Score}";

        // Sets timer to gameDuration length
        timerText.text = $"{gameDuration}";

        // Starts the Game Loop
        state = GameState.Running;
    }

    public void Update()
    {
        view.RPC("GameLoop", RpcTarget.All);
    }

    // Game Loop
    [PunRPC]
    public void GameLoop()
    {
        if (state == GameState.End) EndGame();

        // Only executes function if the gamestate is running
        if (state != GameState.Running) return;

        // Checks if player falls in void, hence, has died and grants score to apponent
        voidCheck();
        // Only executes if a player has hit the maxScore
        if (player1Score >= maxScore || player2Score >= maxScore) state = GameState.End;

        // Updates timer
        view.RPC("UpdateTimer", RpcTarget.All);
    }

    // Checks if player has fallen into the void
    [PunRPC]
    public void voidCheck()
    {
        // Check if player1ObjectOnline is not null before accessing its transform
        if (player1ObjectOnline != null && player1ObjectOnline.transform.position.y < respawnThreshold)
        {
            playerDied = PlayerIdentity.Players.player1.ToString();
            playerScored = PlayerIdentity.Players.player2.ToString();
        }
        // Check if player2ObjectOnline is not null before accessing its transform
        else if (player2ObjectOnline != null && player2ObjectOnline.transform.position.y < respawnThreshold)
        {
            playerDied = PlayerIdentity.Players.player2.ToString();
            playerScored = PlayerIdentity.Players.player1.ToString();
        }
        else
        {
            playerDied = playerScored = null;
        }

        if (playerDied != null && playerScored != null)
        {
            view.RPC("KillCheck", RpcTarget.All, playerDied, playerScored);
        }
    }

    // Checks which player made the "kill" and which player "died"
    [PunRPC]
    public void KillCheck(string playedDied, string playerScored)
    {
        if (playerDied == null || playerScored == null) return;

        view.RPC("RespawnPlayer", RpcTarget.All, playerDied);
        view.RPC("UpdateScore", RpcTarget.All, playerScored);

    }

    // Respawns Player that died due to the void
    [PunRPC]
    public void RespawnPlayer(string player)
    {
        // Teleports player back to the spawn position which is (0, 40, 0)
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1) player1ObjectOnline.transform.position = new Vector3(0f, 40f, 0f);
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 2) player2ObjectOnline.transform.position = new Vector3(0f, 40f, 0f);
    }

    // Updates score to add +1 score to the player that "killed"
    [PunRPC]
    public void UpdateScore(string player)
    {
        // Gives the winning player +1 score
        if (playerScored == "player1") { GameMaster.instance.currentPlayer1.kills += scorePerKill; player1Score += scorePerKill; }
        else { GameMaster.instance.currentPlayer2.kills += scorePerKill; player2Score += scorePerKill; }

        // Updates Score
        scoreText.text = $"{player1Name} - {player1Score} | {player2Name} - {player2Score}";
    }

    //Updates timer
    [PunRPC]
    public void UpdateTimer()
    {
        // Sets the previous time to a variable to calculate difference in time
        previousTime = currentTime;

        // Gets current time as function is called
        currentTime = Time.time;

        // Calculates difference in time and rounds to 1 decimal place
        timer -= (currentTime - previousTime);
        float timeRemaining = timer;

        // Converts Seconds to Milliseconds
        timeRemaining *= 1000;
        timeRemaining = Mathf.Round(timeRemaining);

        // Checks if timer has reached zero and stops the game
        if (timeRemaining <= 0)
        {
            state = GameState.End;
            return;
        }

        // Converts Milliseconds to decaseconds, seconds and minutes
        int decaseconds = Mathf.FloorToInt((timeRemaining % 1000) / 100);
        int seconds = Mathf.FloorToInt((timeRemaining / 1000) % 60);
        int minutes = Mathf.FloorToInt(timeRemaining / 60000);

        // Displays correct formnat accounting for minutes, seconds and decaseconds
        if (timeRemaining >= 60000) displayTime = string.Format("{0:00}:{1:00}", minutes, seconds);
        else displayTime = string.Format("{0}.{1}", seconds, decaseconds); 

        // Displays time to timer text component
        timerText.text = $"{displayTime}";
    }

    [PunRPC]
    public void EndGame()
    {
        GameMaster.instance.SortTempList(GameMaster.instance.tempPlayers, true);
        GameMaster.instance.SaveGame();

        alertText.enabled = true;

        if (player1Score > player2Score) alertText.text = $"{player1Name} Wins!";
        if (player2Score > player1Score) alertText.text = $"{player2Name} Wins!";
        if (player1Score == player2Score) alertText.text = "It's a draw!";

        view.RPC("ReturnToMainMenu", RpcTarget.All);
    }

    [PunRPC]
    public IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("MainMenu");
    }
}