using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class LobbyManager : MonoBehaviour
{
    //UI Elements
    public TMP_InputField player1Name;
    public TMP_InputField player2Name;
    public TMP_InputField maxKills;
    public TMP_InputField maxTime;

    //UI buttons
    public Button playButton;
    public Button player1ReadyUp;
    public Button player2ReadyUp;
    public Button player1NotReady;
    public Button player2NotReady;
    //bools
    public bool player1IsReady;
    public bool player2IsReady;


    // Start is called before the first frame update
    void Start()
    {
        if (GameMaster.instance.saveData.lastPlayerNames[0] == null)
        {
            player1Name.text = "Insert Player Name";
        }
        else
        {
            player1Name.text = GameMaster.instance.saveData.playerNames[0];
        }
        if (GameMaster.instance.saveData.lastPlayerNames[1] == null)
        {
            player2Name.text = "Insert Player Name";
        }
        else
        {
            player2Name.text = GameMaster.instance.saveData.playerNames[1];
        }
        maxKills.text = GameMaster.instance.saveData.maxKills.ToString();
        maxTime.text = GameMaster.instance.saveData.maxRoundTime.ToString();
    }

    public void UpdatePlayerName(int playerNum)
    {
        if (playerNum == 1)
        {
            GameMaster.instance.currentPlayer1.playerName = player1Name.text;
        }
        if (playerNum == 2)
        {
            GameMaster.instance.currentPlayer2.playerName = player2Name.text;
        }
    }
    public void UpdateKills()
    {
        GameMaster.instance.saveData.maxKills = int.Parse(maxKills.text);

    }
    public void UpdateTime()
    {
        GameMaster.instance.saveData.maxRoundTime = float.Parse(maxTime.text);
    }
    public void EnableBools(int playerNum)
    {
        if (playerNum == 1)
        {
            player1IsReady = true;
            player1ReadyUp.interactable = false;
            player1NotReady.interactable = true;
        }
        if (playerNum == 2)
        {
            player2IsReady = true;
            player2ReadyUp.interactable = false;
            player2NotReady.interactable = true;
        }
        if (player1IsReady && player2IsReady)
        {
            playButton.interactable = true;
        }
        else
        {
            playButton.interactable = false;
        }
    }
    public void DisableBools(int playerNum)
    {
        if (playerNum == 1)
        {
            player1IsReady = false;
            player1ReadyUp.interactable = true;
            player1NotReady.interactable = false;
        }
        if (playerNum == 2)
        {
            player2IsReady = false;
            player2ReadyUp.interactable = true;
            player2NotReady.interactable = false;
        }
        if (player1IsReady && player2IsReady)
        {
            playButton.interactable = true;
        }
        else
        {
            playButton.interactable = false;
        }
    }

}