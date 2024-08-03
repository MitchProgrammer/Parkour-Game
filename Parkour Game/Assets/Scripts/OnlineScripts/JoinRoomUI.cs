using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class JoinRoomUI : MonoBehaviourPunCallbacks
{
    public TMP_InputField joinField, createField;
    public TMP_Text debugText;
    public byte maxNumberPerRoom;

    private void OnEnable()
    {
        debugText.text = "";
    }

    public void AttemptToJoinRoom()
    {
        PhotonNetwork.JoinRoom(joinField.text);
    }
    public void AttemptToCreateRoom()
    {
        PhotonNetwork.CreateRoom(createField.text, new RoomOptions { MaxPlayers = maxNumberPerRoom });
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        debugText.text = "Failed to join room because of following reason " + message;
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        debugText.text = "Failed to create room because of following reason " + message;
    }
}
