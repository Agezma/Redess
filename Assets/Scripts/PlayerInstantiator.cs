using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerInstantiator : MonoBehaviourPunCallbacks
{
    public GameObject prefabPlayer;

    public Transform[] spawnPos;

    Player[] allPlayers;

    public void Awake()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        GetPlayers();

        for (int i = 0; i < allPlayers.Length; i++)
        {
            PhotonNetwork.Instantiate(prefabPlayer.name, spawnPos[i].position, spawnPos[i].rotation);
        }
    }

    void GetPlayers()
    {
        allPlayers = PhotonNetwork.PlayerList;

    }
}
