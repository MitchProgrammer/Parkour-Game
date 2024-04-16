using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameMaster : MonoBehaviour
{
    public GameData saveData;

    // Refrence to current players
    [HideInInspector] public PlayerData currentPlayer1, currentPlayer2;

    // Hold a temp list of scored to be sorted
    public List<PlayerData> tempPlayers = new List<PlayerData>(10);

    // Debug switches
    public bool debugButtons;
    public bool loadOnStart = true;

    #region Singleton
    public static GameMaster instance;

    private void Awake()
    {
        if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }
    #endregion

    public void Start()
    {
        currentPlayer1 = new PlayerData();
        currentPlayer2 = new PlayerData();

        if (loadOnStart) LoadGame();
        else { saveData = new GameData(); CreateTempList(); }  
    }

    // Create temp list of all players, filled in with save data
    public void CreateTempList()
    {
        // Create empty list
        tempPlayers = new List<PlayerData>();

        // Get players from saveData and append them to list
        for (int i = 0; i < saveData.playerNames.Length; i++)
        {
            // Create a player profile
            PlayerData newPlayer = new PlayerData();

            // Input the information from the saveData to the new player
            newPlayer.playerName = saveData.playerNames[i];
            newPlayer.kills = saveData.kills[i];
            newPlayer.deaths = saveData.deaths[i];

            // Calculate the KDR
            if (newPlayer.deaths == 0) newPlayer.kdr = newPlayer.kills;
            else if (newPlayer.kills == 0) newPlayer.kdr = -newPlayer.deaths;
            else newPlayer.kdr = (float)newPlayer.kills / (float)newPlayer.deaths;

            tempPlayers.Add(newPlayer);
        }
    }

    public List<PlayerData> SortTempList(List<PlayerData> unSortedPlayers, bool addCurrentPlayers = false)
    {
        if (addCurrentPlayers)
        {
            if (tempPlayers.Find(p => p.playerName == currentPlayer1.playerName) == null) tempPlayers.Add(currentPlayer1);
            else
            {
                PlayerData existingPlayer = tempPlayers.Find(p => p.playerName == currentPlayer1.playerName);
                existingPlayer.kills = currentPlayer1.kills;
                existingPlayer.deaths = currentPlayer1.deaths;

                if (existingPlayer.deaths == 0) existingPlayer.kdr = existingPlayer.kills;
                else if (existingPlayer.kills == 0) existingPlayer.kdr = -existingPlayer.deaths;
                else existingPlayer.kdr = (float)existingPlayer.kills / (float)existingPlayer.deaths;
            }

            if (tempPlayers.Find(p => p.playerName == currentPlayer2.playerName) == null) tempPlayers.Add(currentPlayer2);
            else
            {
                PlayerData existingPlayer = tempPlayers.Find(p => p.playerName == currentPlayer2.playerName);
                existingPlayer.kills = currentPlayer2.kills;
                existingPlayer.deaths = currentPlayer2.deaths;

                if (existingPlayer.deaths == 0) existingPlayer.kdr = existingPlayer.kills;
                else if (existingPlayer.kills == 0) existingPlayer.kdr = -existingPlayer.deaths;
                else existingPlayer.kdr = (float)existingPlayer.kills / (float)existingPlayer.deaths;
            }
        }

        List<PlayerData> sortedPlayers = unSortedPlayers.OrderByDescending(p => p.kdr).ToList();

        return sortedPlayers;
    }

    // Convert the list to simple data arrays
    // Save the arrays to saveData
    public void SaveHighScoreData(List<PlayerData> players)
    {
        for (int i = 0; i < 10; i++)
        {
            saveData.playerNames[i] = players[i].playerName;
            saveData.kills[i] = players[i].kills;
            saveData.deaths[i] = players[i].deaths;
        }
    }

    // Save Game
    public void SaveGame()
    {
        SortTempList(tempPlayers, false);
        SaveHighScoreData(tempPlayers);

        saveData.lastPlayerNames[0] = currentPlayer1.playerName;
        saveData.lastPlayerNames[1] = currentPlayer2.playerName;

        SaveSystem.instance.SaveGame(saveData);
    }

    // Loading the game
    public void LoadGame()
    {
        // Attempt to get saveData file 
        saveData = SaveSystem.instance.LoadGame();

        if (saveData == null) { saveData = new GameData(); Debug.Log("No data was found, new file created instead"); }

        currentPlayer1.playerName = saveData.lastPlayerNames[0];
        currentPlayer2.playerName = saveData.lastPlayerNames[1];
        CreateTempList();
    }

    #region debugging
    private void Update()
    {
        if (!debugButtons) return;

        if (Input.GetKeyDown(KeyCode.O))
        {
            if (tempPlayers != null)
            {
                tempPlayers = SortTempList(tempPlayers, false);
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveGame();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadGame();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RandomFillData();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            ClearData();
        }

    }

    #region debugging functions
    void ClearData()
    {
        foreach (PlayerData player in tempPlayers)
        {
            player.playerName = "";
            player.kills = 0;
            player.deaths = 0;
            player.kdr = 0;
        }
    }
    void RandomFillData()
    {
        //create possible letters to randomise from
        string glyphs = "abcdefghijklmnopqrstuvwxyz";

        foreach (PlayerData player in tempPlayers)
        {
            //generate a random name for the temp player
            int charAmount = Random.Range(3, 10);
            player.playerName = "";
            for (int i = 0; i < charAmount; i++)
            {
                player.playerName += glyphs[Random.Range(0, glyphs.Length)];
            }
            //generate random Kills score
            player.kills = Random.Range(0, 5);
            //generate random deaths
            player.deaths = Random.Range(0, 5);
            //calculate kd
            if (player.deaths == 0) player.kdr = player.kills;
            else player.kdr = player.kills / player.deaths;
        }

    }
    #endregion
    #endregion

    #region Testing
    /*
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            saveData.AddScore(1);
            PrintScore();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            saveData.AddScore(-1);
            PrintScore();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveSystem.instance.SaveGame(saveData);
            Debug.Log("Saved Data");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            saveData = SaveSystem.instance.LoadGame();
            Debug.Log("Loaded Data");
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            saveData.ResetData();
            Debug.Log("Cleared Data");
        }
    }

    public void PrintScore() { Debug.Log($"The current score is {saveData.score}"); }
    */
    #endregion

}
