using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections.Generic;

public class NetManager : MonoBehaviourPunCallbacks
{
    public InputField roomName;
    public Text Log;
    public Text PlayersCount;
    public int playersCount;
    public int maxPlayers = 4;

    public PlayerInstantiator server;
    public CharacterInput controller;

    private void Awake()
    {
        Connect();
    }

    void Connect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    #region Net Events

    public override void OnCreatedRoom()
    {
        Debug.Log("crear sv");
        PhotonNetwork.Instantiate(server.name, Vector3.zero, Quaternion.identity);
    }
    public override void OnConnectedToMaster()
    {
        JoinRoomScene("Menu");
        Debug.Log("Conected");
        PhotonNetwork.JoinLobby();
        Log.text += "Conected To Sever...";        
    }

    public void JoinRandom()
    {
        if (!PhotonNetwork.JoinRandomRoom())
        {
            Log.text += "\nJoining Room...";
        }
        else
        {
            Log.text += "\nFail Joining Room...";
        }
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Log.text = "\nNo Rooms Active, Creating One...";

        RoomOptions option = new RoomOptions();
        option.MaxPlayers = (byte)maxPlayers;

        if (PhotonNetwork.CreateRoom(roomName.text, option, TypedLobby.Default))
        {
            Log.text += "\nRoom Created...";
        }
        else
        {
            Log.text += "\nFail Creating Room...";
        }
    }

    public override void OnJoinedRoom()
    {
        Log.text += "\nWaiting for players";
        Debug.Log("instanciar control");
        GameObject control = PhotonNetwork.Instantiate(controller.name, Vector3.zero, Quaternion.identity) ;
        server.GetComponent<PlayerInstantiator>().dicControls.Add(PhotonNetwork.LocalPlayer, control.GetComponent<CharacterInput>());
        if (PhotonNetwork.CurrentRoom.PlayerCount >= PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
       
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
    }
    #endregion

    public void ConnectToLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    public void JoinRoomScene(string level)
    {
        PhotonNetwork.LoadLevel(level);
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            playersCount = PhotonNetwork.CurrentRoom.PlayerCount;
            PlayersCount.text = playersCount.ToString();
            var players = PhotonNetwork.CurrentRoom.Players;
            if (PhotonNetwork.CurrentRoom.PlayerCount >= maxPlayers)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
    }
}
