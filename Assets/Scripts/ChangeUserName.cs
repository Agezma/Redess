using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class ChangeUserName : MonoBehaviour
{
    public Button CreateRoom;
    public Button JoinRoom;
    public Text UserName;

    private void Start()
    {
        JoinRoom.interactable = false;
        CreateRoom.interactable = false;
    }

    public void NewName(string newName)
    {
        Player playerLocal = PhotonNetwork.LocalPlayer;
        playerLocal.NickName = newName;
        UserName.text = "User Name: " + newName;
        JoinRoom.interactable = true;
        CreateRoom.interactable = true;
    }
}
