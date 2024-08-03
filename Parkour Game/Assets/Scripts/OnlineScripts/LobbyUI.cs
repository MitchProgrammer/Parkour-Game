using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyUI : MonoBehaviourPunCallbacks
{
    [Header("UI Components")]
    public TMP_InputField playerNameField;
    public Button readyButton, cancelButton, playButton;

    public TMP_Text roomName, playerNumbers, debugText;

    PhotonView view;

    [Header("Settings")]
    public bool[] isReady;

    private void OnEnable()
    {
        view = GetComponent<PhotonView>();

        roomName.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;
        playerNumbers.text = "Waiting For Players " + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
        isReady = new bool[PhotonNetwork.CurrentRoom.MaxPlayers];

        //toggle buttons
        readyButton.interactable = true;
        cancelButton.interactable = false;
        if (PhotonNetwork.IsMasterClient) playButton.interactable = true;
        else playButton.interactable = false;
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        playerNumbers.text = "Waiting For Players " + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        playerNumbers.text = "Waiting For Players " + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
    }
    
    public void UpdatePlayerName() //Take the text in the input field and apply it to the nickname and current player name
    {
        PhotonNetwork.LocalPlayer.NickName = playerNameField.text;
        if(GameMaster.instance != null)
        {
            if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
            {
                GameMaster.instance.currentPlayer1.playerName = playerNameField.text;
            }
            else if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
            {
                GameMaster.instance.currentPlayer2.playerName = playerNameField.text;
            }
        }
    }
    
    public void UpdateReadyBools(bool readyBool) //show whether this character is ready or not.
    {
        if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            view.RPC("ApplyBools", RpcTarget.All, 1, readyBool);
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            view.RPC("ApplyBools", RpcTarget.All, 2, readyBool);
        }
    }
    [PunRPC]
    public void ApplyBools(int playerNumber, bool readyBool)
    {
            isReady[playerNumber -1] = readyBool;
    }

    public void AttemptToStart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if(PhotonNetwork.CurrentRoom.PlayerCount != PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                debugText.text = "Cannot start game. Waiting on more players to join";
                return;
            }
            foreach(bool check in isReady)
            {
                if(check == false)
                {
                    debugText.text = "Not all players are ready";
                    return;
                }
            }
            PhotonNetwork.LoadLevel("OnlinePlayground");
        }
        else
        {
            debugText.text = "Cannot start game. You are not the master";
        }
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainMenu");
    }
}
