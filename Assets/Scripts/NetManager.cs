using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class NetManager : MonoBehaviourPunCallbacks
{
    public InputField roomName;
    static bool isConected = false;
    Text currentText;
    Player asd;
    private void Awake()
    {
        Connect();
        
    }
    private void Start()
    {
    }


    void Connect()
    {
        if (!isConected)
        {
            PhotonNetwork.ConnectUsingSettings();
            isConected = true;
        }
    }
    

    #region Net Events
    public override void OnConnectedToMaster()
    {
        JoinRoomScene("Menu");
        Debug.Log("Conected");
        PhotonNetwork.JoinLobby();
    }


    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
    }

    public override void OnJoinedRoom()
    {
        JoinRoomScene("Level");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
    }

    public override void OnCreatedRoom()
    {    
    }
    

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
    }
    #endregion

    public void NewRoomCreate()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 10;
        
        PhotonNetwork.CreateRoom(roomName.text, options, TypedLobby.Default);
    }

    public void ConnectToLobby()
    {
        PhotonNetwork.JoinLobby();
    }
    
    public void ConnectToRoom(string level)
    {
        PhotonNetwork.JoinRoom(level);
    }
    
    public void JoinRoomScene(string level)
    {
        PhotonNetwork.LoadLevel(level);
    }
}
