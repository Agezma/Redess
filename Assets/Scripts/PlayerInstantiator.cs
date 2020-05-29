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
        GetSpawnPoint();
        for (int i = 0; i < allPlayers.Length; i++)
        {
            if(PhotonNetwork.LocalPlayer.ActorNumber-1 == i)
            {
                PhotonNetwork.Instantiate(prefabPlayer.name, spawnPos[PhotonNetwork.LocalPlayer.ActorNumber-1].position, spawnPos[PhotonNetwork.LocalPlayer.ActorNumber-1].rotation);
            }
        }
    }

    void GetSpawnPoint()
    {
        allPlayers = PhotonNetwork.PlayerList;
    }
}
