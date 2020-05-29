using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerInstantiator : MonoBehaviourPunCallbacks
{
    public GameObject prefabPlayer;

    public Transform[] spawnPos;

    List<Player> allPlayers = new List<Player>();

    public void Awake()
    {
        GetPlayers();

        for (int i = 0; i < allPlayers.Count; i++)
        {
            PhotonNetwork.Instantiate(prefabPlayer.name, spawnPos[i].position, spawnPos[i].rotation);
        }
    }

    void GetPlayers()
    {
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            allPlayers.Add(PhotonNetwork.CurrentRoom.GetPlayer(i));
        }
    }
}
