using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayManager : MonoBehaviour
{
    #region Variables

    //Player Identification
    [Header("Player Identification")]

    [HideInInspector] public GameObject player1Object;
    [HideInInspector] public GameObject player2Object;

    public GameObject player1Prefab;
    public GameObject player2Prefab;

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
    #endregion

    #region singleton
    public static GamePlayManager gmpInstance;

    private void Awake()
    {
        gmpInstance = this;
    }
    #endregion

    // Start Game Setup
    public void Start()
    {
        // If a Gamemaster exists, then use its data
        if (GameMaster.instance != null)
        {
            maxScore = GameMaster.instance.saveData.maxKills;
            gameDuration = GameMaster.instance.saveData.maxRoundTime;

            player1Name = GameMaster.instance.currentPlayer1.playerName;
            player2Name = GameMaster.instance.currentPlayer2.playerName;
        }

        state = GameState.Intro;
        SetupGame();
    }

    // Setup Game
    public void SetupGame()
    {
        // Spawn Players
        player1Object = Instantiate(player1Prefab, player1Spawnpoint.position, player1Spawnpoint.rotation);
        player2Object = Instantiate(player2Prefab, player2Spawnpoint.position, player2Spawnpoint.rotation);

        // Freeze Players
        player1Object.GetComponent<PlayerMovement>().enabled = false;
        player2Object.GetComponent<PlayerMovement>().enabled = false;

        StartCoroutine(IntroSequence());
    }

    // Start Countdown
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

        // Unfreeze Players
        player1Object.GetComponent<PlayerMovement>().enabled = true;
        player2Object.GetComponent<PlayerMovement>().enabled = true;

        RunGame();
    }

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
        if (player1Name == "") player1Name = "Player1";
        if (player2Name == "") player2Name = "Player2";

        // Sets score of both players to zero
        scoreText.text = $"{player1Score} | {player2Score}";

        // Sets timer to gameDuration length
        timerText.text = $"{gameDuration}";

        // Starts the Game Loop
        state = GameState.Running;
    }

    public void Update()
    {
        GameLoop();
    }

    // Game Loop
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
        UpdateTimer();

        
    }

    // Checks if player has fallen into the void
    public void voidCheck()
    {
        if (player1Object.transform.position.y < respawnThreshold)
        {
            playerDied = PlayerIdentity.Players.player1.ToString();
            playerScored = PlayerIdentity.Players.player2.ToString();
        }
        else if (player2Object.transform.position.y < respawnThreshold)
        {
            playerDied = PlayerIdentity.Players.player2.ToString();
            playerScored = PlayerIdentity.Players.player1.ToString();
        }
        else playerDied = playerScored = null;

        // Calls KillCheck() with data of both players
        KillCheck(playerDied, playerScored);
    }

    // Checks which player made the "kill" and which player "died"
    public void KillCheck(string playedDied, string playerScored)
    {
        if (playerDied == null || playerScored == null) return;

        // Respawns dead player
        RespawnPlayer(playerDied);

        // Calls UpdateScore() to update the score with passthrough variable of winner
        UpdateScore(playerScored);
    }

    // Respawns Player that died due to the void
    public void RespawnPlayer(string player)
    {
        // Teleports player back to the spawn position which is (0, 40, 0)
        if (player == "player1") player1Object.transform.position = new Vector3(0f, 40f, 0f);
        else player2Object.transform.position = new Vector3(0f, 40f, 0f);
    }

    // Updates score to add +1 score to the player that "killed"
    public void UpdateScore(string player)
    {
        // Gives the winning player +1 score
        if (playerScored == "player1") { GameMaster.instance.currentPlayer1.kills += scorePerKill; player1Score += scorePerKill; }
        else { GameMaster.instance.currentPlayer2.kills += scorePerKill; player2Score += scorePerKill; }

        // Updates Score
        scoreText.text = $"{player1Score} | {player2Score}";
    }

    //Updates timer
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

    public void EndGame()
    {
        GameMaster.instance.SortTempList(GameMaster.instance.tempPlayers, true);
        GameMaster.instance.SaveGame();

        SceneManager.LoadScene("EndScene");
    }
}