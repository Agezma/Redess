using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetManager : MonoBehaviourPunCallbacks
{
    public InputField roomName;
    public Text Log;
    public Text PlayersCount;
    public int playersCount;

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
        Log.text += "\nJoining";
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

    private void FixedUpdate()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            playersCount = PhotonNetwork.CurrentRoom.PlayerCount;
            PlayersCount.text = playersCount.ToString();

            if (PhotonNetwork.CurrentRoom.PlayerCount >= 4)
            {
                PhotonNetwork.LoadLevel("Level");
            }
        }
    }
}
