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
    public Dictionary<Player, PlayerInfo> dicInfo = new Dictionary<Player, PlayerInfo>();
    public Dictionary<Player, CharacterInput> dicControls = new Dictionary<Player, CharacterInput>();

    public CharacterInput controller;
    public PlayerInfo playerInfo;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if(Instance == null && photonView.IsMine)
        {
            SetMyServer();  
        }
    }

    private void Start()
    {
        StartCoroutine(ShowPing());
    }


    IEnumerator ShowPing()
    {
        while (true)
        {
            for (int i = 0; i < dicInfo.Count; i++)
            {
                dicInfo[PhotonNetwork.LocalPlayer].Ping.text = PhotonNetwork.GetPing().ToString();
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    Player[] GetCurrentPlayersList()
    {
        return PhotonNetwork.PlayerList;
    }

    public void RequestRespawn(Player player)//Para cada spawn, calcular la distancia a los players, el q tenga el minimo mas lejos es el q queda.
    {
        photonView.RPC("Respawn", server, player);
    }
    [PunRPC]
    void Respawn(Player player)
    {
        Transform current = spawnPos[0];
        allPlayers = GetCurrentPlayersList();
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
        dicChars[player].transform.position = current.position;
        dicChars[player].transform.rotation = current.rotation;
    }


    public void SetMyServer()
    {
        photonView.RPC("SetServer", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer, "Level");      
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
        LoadLevel("Level");
        var playerLocal = PhotonNetwork.LocalPlayer;

        //if (serverPlayer != PhotonNetwork.LocalPlayer)
        //{
        photonView.RPC("AddPlayer", server, playerLocal);
        //photonView.RPC("AddController", server, playerLocal);

        CreateStats();
        //}
    }

    IEnumerator WaitForLoad(string sceneName)
    {
        while (PhotonNetwork.CurrentRoom.PlayerCount < PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            yield return new WaitForEndOfFrame();
        }
        PhotonNetwork.LoadLevel(sceneName);
    }

    public void LoadLevel(string scene)
    {
        photonView.RPC("LoadLevelToAll", RpcTarget.AllBuffered, scene);
    }
    [PunRPC]
    void LoadLevelToAll(string scene)
    {
        StartCoroutine(WaitForLoad(scene));
    }

    //[PunRPC]
    //void AddController(Player localPlayer)
    //{
    //    CharacterInput control = Instantiate(controller, Vector3.zero, Quaternion.identity).GetComponent<CharacterInput>();
    //    Instance.dicControls.Add(localPlayer, control);
    //}  

    public void RequestChar(Player player)
    {
        photonView.RPC("GetChar", server, player);
    }

    [PunRPC]
    void GetChar(Player player)
    {
        Debug.Log("Pide char");
        if (dicControls.ContainsKey(player))
        {
            Debug.Log("Contiene char");
            dicControls[player].SetChar(dicChars[player]);
        }
    }

    public void CreateStats()
    {
        photonView.RPC("AddPlayerStats", server, PhotonNetwork.LocalPlayer);
    }
    [PunRPC]
    void AddPlayerStats(Player player)
    {
        PlayerInfo current = Main.instance.ScoreBoard.CreateInfo(player);
        dicInfo.Add(player, current);
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

        character.onUI.lifeText = Main.instance.GetLifeText();
        character.onUI.grenadeCounter = Main.instance.GetGrenadeText();
        character.onUI.rewindImage = Main.instance.GetRewindImg();
        dicChars.Add(player, character);
    
        CameraRotator cam = character.GetComponent<CameraRotator>();
        dicCam.Add(player, cam);
    }


    #region TurnCamAndModel
    public void RequestTurnModel(Player player)
    {
        photonView.RPC("TurnModel", player, player);
    }

    [PunRPC]
    void TurnModel(Player player)
    {
        if (dicChars.ContainsKey(player))
        {
            dicChars[player].TurnModel();
        }
    }
    #endregion

    #region Move
    public void RequestMove(Player player, float velX, float velY)
    {
        photonView.RPC("Move", server, player, velX, velY);
    }
    [PunRPC]
    void Move(Player player, float velX, float velY)
    {
        if (dicChars.ContainsKey(player) && !dicChars[player].isDead)
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
        photonView.RPC("Rewind", RpcTarget.AllBuffered, player); // TODO all buffered o server? ni idea
    }
    [PunRPC]
    void Rewind(Player player)
    {
        if (dicChars.ContainsKey(player) && !dicChars[player].rewindInCD && dicChars[player].currentRewindable && !dicChars[player].isDead)
        {
            dicChars[player].RewindTime();
        }
    }
    #endregion

    /*public void RequestRewindable()
    {
        photonView.RPC();
    }
    [PunRPC]
    void ReqRewind
    */
    #region Shoot
    public void RequestShoot(Player player)
    {
        photonView.RPC("Shoot", server, player);
    }
    [PunRPC]
    void Shoot(Player player)
    {
        if (dicChars.ContainsKey(player) && !dicChars[player].isDead)
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
        if (dicChars.ContainsKey(player) && dicChars[player].grenadeCount > 0 && !dicChars[player].isDead)
        {
            dicChars[player].ThrowGrenadeAnim();
        }
    }
    #endregion

    #region Damage

    public void RequestTakeDamage(Player player, float dmg)
    {
        photonView.RPC("TakeDamage", RpcTarget.AllBuffered, player,  dmg);
    }

    [PunRPC]
    void TakeDamage(Player player, float dmg)
    {
        if (dicChars.ContainsKey(player))
        {
            dicChars[player].TakeDamage(dmg);
        }
    }


    #endregion
}
