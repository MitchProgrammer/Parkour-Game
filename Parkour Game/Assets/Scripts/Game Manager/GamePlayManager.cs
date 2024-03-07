using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    // Store Player Identity
    // Gameplay States
    // Store UI
    // Store Score
    // Store Timer
    // Store Player Data

    #region Variables

    //Player Identification
    [Header("Player Identification")]
    public GameObject player1Object;
    public GameObject player2Object;

    public string player1Name;
    public string player2Name;

    // Gamestates
    public enum GameState { Intro, Gameplay, Pause, End };

    [Header("Gamestate Identification")]
    public GameState state;

    // Player Scores
    [Header("Player Scores")]
    public int player1Score;
    public int player2Score;

    public int maxScore;

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

    // UI Setting
    [Header("UI Settings")]
    public TMP_Text scoreText;
    public TMP_Text timerText;
    #endregion

    // Start Game Setup
    public void Start()
    {
        StartGameSetup();
    }

    public void StartGameSetup()
    {

    }

    // Game Loop
    public void Update()
    {
        voidCheck();
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

        // Gives the winning player +1 score
        UpdateScore(playerScored);
    }

    // Respawns Player that died due to the void
    public void RespawnPlayer(string player)
    {
        if (player == "player1") player1Object.transform.position = new Vector3(0f, 40f, 0f);

        else player2Object.transform.position = new Vector3(0f, 40f, 0f);
    }

    // Updates score to add +1 score to the player that "killed"
    public void UpdateScore(string player)
    {
        if (playerScored == "player1") player1Score++;
        else player2Score++;

        // (Temporary) Logs player scores to Console. TODO: Log the scored to TMP_Text to display on screen
        Debug.Log(player1Score + " " + player2Score);
    }
}