using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerInstantiator : MonoBehaviourPunCallbacks
{
    public static PlayerInstantiator Instance;

    Player server;

    public GameObject prefabPlayer;

    public Transform[] spawnPos;

    Player[] allPlayers;

    public Dictionary<Player, CharacterHead> dicChars = new Dictionary<Player, CharacterHead>();
    public Dictionary<Player, CameraRotator> dicCam = new Dictionary<Player, CameraRotator>();

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance == null)
        {
            if (photonView.IsMine)
            {
                SetMyServer();
            }
        }
    }

    Player[] GetCurrentPlayersList()
    {
        return PhotonNetwork.PlayerList;
    }

    public void RequestRespawn(Player player, Transform transformToMove) //Para cada spawn, calcular la distancia a los players, el q tenga el minimo mas lejos es el q queda.
    {
        photonView.RPC("Respawn", server, player, transformToMove);
    }
    [PunRPC]
    void Respawn(Transform transf)
    {
        Transform current = spawnPos[0];
        float currentDist = 0;
        float minDist = 0;
        for (int i = 0; i < spawnPos.Length; i++)
        {
            for (int j = 0; j < dicChars.Count; j++)
            {
                currentDist = Vector3.Distance(spawnPos[i].position, dicChars[allPlayers[j]].transform.position);
                if (currentDist > minDist)
                {
                    minDist = currentDist;
                    current = spawnPos[i];
                }
            }
        }
        transf.position = current.position;
        transf.rotation = current.rotation;
    }


    public void SetMyServer()
    {
        if (Instance == null)
        {
            if (photonView.IsMine)
            {
                photonView.RPC("SetServer", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer, "Level");
            }
        }
    }

    [PunRPC]
    void SetServer(Player serverPlayer, string sceneName)
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        server = serverPlayer;

        PhotonNetwork.LoadLevel(sceneName);
        var playerLocal = PhotonNetwork.LocalPlayer;

        //if (serverPlayer != PhotonNetwork.LocalPlayer)
        //
        photonView.RPC("AddPlayer", server, playerLocal);
        //
    }
    [PunRPC]
    void AddPlayer(Player player)
    {
        StartCoroutine(WaitForLevel(player));
    }

    IEnumerator WaitForLevel(Player player)
    {
        while (PhotonNetwork.LevelLoadingProgress > 99)
        {
            yield return new WaitForEndOfFrame();
        }
        CharacterHead character = PhotonNetwork.Instantiate(prefabPlayer.name, Vector3.zero, Quaternion.identity).GetComponent<CharacterHead>();
        dicChars.Add(player, character);
        CameraRotator cam = character.GetComponent<CameraRotator>();
        dicCam.Add(player, cam);

    }

    #region Move
    public void RequestMove(Player player, float velX, float velY)
    {
        photonView.RPC("Move", server, player, velX, velY);
    }
    [PunRPC]
    void Move(Player player, float velX, float velY)
    {
        if (dicChars.ContainsKey(player))
        {
            dicChars[player].Move(velX, velY);
        }
    }
    #endregion

    #region CameraMove
    public void RequestMoveCamera(Player player, float x, float y)
    {
        photonView.RPC("MoveCam", server, player, x, y);
    }
    [PunRPC]
    void MoveCam(Player player, float x, float y)
    {
        if (dicCam.ContainsKey(player))
        {
            dicCam[player].RotateCamera(x, y);
        }
    }
    #endregion

    #region Rewind
    public void RequestRewind(Player player)
    {
        photonView.RPC("Rewind", server, player);
    }
    [PunRPC]
    void Rewind(Player player)
    {
        if (dicChars.ContainsKey(player))
        {
            dicChars[player].RewindTime();
        }
    }
    #endregion

    #region Shoot
    public void RequestShoot(Player player)
    {
        photonView.RPC("Shoot", server, player);
    }
    [PunRPC]
    void Shoot(Player player)
    {
        if (dicChars.ContainsKey(player))
        {
            dicChars[player].Shoot();
        }
    }
    #endregion

    #region ThrowGrenade
    public void RequestThrowGrenade(Player player)
    {
        photonView.RPC("ThrowGrenade", server, player);
    }
    [PunRPC]
    void ThrowGrenade(Player player)
    {
        if (dicChars.ContainsKey(player))
        {
            dicChars[player].ThrowGrenadeAnim();
        }
    }
    #endregion
}
