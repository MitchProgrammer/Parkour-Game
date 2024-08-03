using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    #region Private Serializable Fields
    [SerializeField]
    private byte maxPlayersPerRoom;

    [SerializeField]
    private GameObject connectPanel, progressPanel, roomPanel, lobbyPanel;
    #endregion

    #region Private Fields;

    string gameVersion = "1"; //clients version of game, helps gourp users on the same version to avoid game breaking errors later

    #endregion

    #region MonoBehavour callbacks
    void Awake()
    {
        //#crital
        //make sure all clients sync their scene changes
        PhotonNetwork.AutomaticallySyncScene = true;

        connectPanel.SetActive(true);
        progressPanel.SetActive(false);
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        Connect();
    }
    #endregion

    #region PunCallbacks
    public override void OnConnectedToMaster()
    {
        Debug.Log("Successfully connected to server");
        //room options will cause joining room to fail if max players is reached;
        connectPanel.SetActive(false);
        progressPanel.SetActive(false);
        roomPanel.SetActive(true);
        lobbyPanel.SetActive(false);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning("Disconnected from server for following reason " + cause);
    }

    public override void OnJoinedRoom()
    {
        connectPanel.SetActive(false);
        progressPanel.SetActive(false);
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }
    public override void OnCreatedRoom()
    {
        connectPanel.SetActive(false);
        progressPanel.SetActive(false);
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        Debug.Log("successfully created a room");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
    }

    #endregion

    #region public functions
    public void Connect()
    {
        //check if already connected
        if (PhotonNetwork.IsConnected)
        {
            //go to the lobby
            connectPanel.SetActive(false);
            progressPanel.SetActive(false);
            roomPanel.SetActive(true);
            lobbyPanel.SetActive(false);
        }
        else //if we are not already connected, then connect
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;

            connectPanel.SetActive(false);
            progressPanel.SetActive(true);
            roomPanel.SetActive(false);
            lobbyPanel.SetActive(false);
        }
    }
    #endregion
}
