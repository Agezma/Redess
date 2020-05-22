using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class RoomDat : MonoBehaviour
{
    [SerializeField] Text myText;
    public RoomInfo roomInfo;
    public NetManager netManager;
    public Button button;

    private void Start()
    {
    }

    public void SetRoomInfo(RoomInfo info)
    {
        roomInfo = info;
        myText.text = info.Name + "\n" + "Players: " + info.PlayerCount + "/" + info.MaxPlayers;
        button.onClick.AddListener(delegate () { netManager.ConnectToRoom(info.Name); });
    }
    public void UpdateInfo(RoomInfo info)
    {
        myText.text = info.Name + "\n" + "Players: " + info.PlayerCount + "/" + info.MaxPlayers;
    }



}
