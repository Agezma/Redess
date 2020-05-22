using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomSelector : MonoBehaviourPunCallbacks
{
    [SerializeField] RoomDat roomInfo;
    [SerializeField] NetManager netManager;
    List<RoomDat> myRooms = new List<RoomDat>();
    List<int> players = new List<int>();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {       
        foreach (var item in roomList)
        {
            int i = myRooms.FindIndex(x => x.roomInfo.Name == item.Name);
            if (item.RemovedFromList)
            {
                if (i != -1)
                {
                    PhotonNetwork.Destroy(myRooms[i].gameObject);
                    players.RemoveAt(i);
                    myRooms.RemoveAt(i);
                }
            }
            else
            {
                if(i!= -1)
                    PhotonNetwork.Destroy(myRooms[i].gameObject);
                RoomDat newRoom = Instantiate(roomInfo, Vector3.zero, Quaternion.identity);
                /*if (newRoom && myRooms.Contains(newRoom))
                {
                    newRoom.UpdateInfo(item);
                    
                    PhotonNetwork.Destroy(myRooms[i].gameObject);
                    players[i] = item.PlayerCount;
                    myRooms.RemoveAt(i);
                }*/
                if (newRoom)
                {
                    newRoom.transform.SetParent(transform);
                    newRoom.SetRoomInfo(item);
                    newRoom.netManager = netManager;
                    myRooms.Add(newRoom);
                    players.Add(item.PlayerCount);
                }
            }
        }
    }
}

