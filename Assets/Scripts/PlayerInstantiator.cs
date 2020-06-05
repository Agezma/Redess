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

    CharacterHead[] allChars;

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

        allChars = FindObjectsOfType<CharacterHead>();
    }

    void GetSpawnPoint()
    {
        allPlayers = PhotonNetwork.PlayerList;
    }

    public Transform Respawn() //Para cada spawn, calcular la distancia a los players, el q tenga el minimo mas lejos es el q queda.
    {
        Transform current = spawnPos[0];
        float currentDist = 0;
        float minDist = 10000;
        for (int i = 0; i < spawnPos.Length; i++)
        {
            for (int j = 0; j < allChars.Length; j++)
            {
                currentDist = Vector3.Distance(spawnPos[i].position, allChars[j].transform.position);
                if (currentDist < minDist)
                {
                    minDist = currentDist;
                    current = spawnPos[i];
                }
            }
        }

        return current;
    }
}
