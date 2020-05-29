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

        if (PhotonNetwork.CreateRoom(roomName.text, new RoomOptions(), TypedLobby.Default))
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
        Log.text += "\nJoined";
        if (PhotonNetwork.CurrentRoom.PlayerCount >= maxPlayers)
        {
            PhotonNetwork.LoadLevel("Level");
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
                PhotonNetwork.LoadLevel("Level");
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }           
        }
    }
}
