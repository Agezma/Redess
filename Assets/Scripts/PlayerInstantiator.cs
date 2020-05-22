using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerInstantiator : MonoBehaviourPunCallbacks
{
    public GameObject prefabPlayer;

    public void Awake()
    {
        PhotonNetwork.Instantiate(prefabPlayer.name, Vector3.zero, Quaternion.identity);            
    }
}
