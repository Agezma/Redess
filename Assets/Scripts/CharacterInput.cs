using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput : MonoBehaviourPun, IController
{
    public CharacterHead myChar;
    float grenadeCount;
    bool rewindInCD;
    Rewindable currentRewindable;
    bool isDead;

    ScoreManager scoreboard;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void Start()
    {
        if (!photonView.IsMine) return;

        PlayerInstantiator.Instance.RequestChar(PhotonNetwork.LocalPlayer);

        scoreboard = Main.instance.GetScoreboard();

        PlayerInstantiator.Instance.RequestTurnModel(PhotonNetwork.LocalPlayer);
    }
    public void SetChar(CharacterHead head)
    {
        myChar = head;
    }
    public void Update()
    {

        if (!photonView.IsMine || !myChar || myChar.isDead) return;

        if (Rewind() && !myChar.rewindInCD && myChar.currentRewindable)
        {
            PlayerInstantiator.Instance.RequestRewind(PhotonNetwork.LocalPlayer);
        }
        if (Shoot())
        {
            PlayerInstantiator.Instance.RequestShoot(PhotonNetwork.LocalPlayer);
        }
        if (ThrowGranade() && myChar.grenadeCount > 0)
        {
            PlayerInstantiator.Instance.RequestThrowGrenade(PhotonNetwork.LocalPlayer);
        }
        PlayerInstantiator.Instance.RequestMoveCamera(PhotonNetwork.LocalPlayer, HorizontalRotation(), VerticalRotation());

        if (OpenScoreboard())
        {
            scoreboard.gameObject.SetActive(true);
        }else if (CloseScoreboard())
        {
            scoreboard.gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        if (!myChar || myChar.isDead) return;

        PlayerInstantiator.Instance.RequestMove(PhotonNetwork.LocalPlayer, Horizontal(), Vertical());
    }


    public float Horizontal()
    {
        return Input.GetAxis("Horizontal");
    }

    public float Vertical()
    {
        return Input.GetAxis("Vertical");
    }
    public bool Jump()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    public float HorizontalRotation()
    {
        return Input.GetAxis("Mouse X");
    }

    public float VerticalRotation()
    {
        return Input.GetAxis("Mouse Y");
    }

    public bool Shoot()
    {
        return Input.GetMouseButtonDown(0);
    }
    public bool ThrowGranade()
    {
        return Input.GetKeyDown(KeyCode.Q);
    }
    public bool Rewind()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    public bool OpenScoreboard()
    {
        return Input.GetKeyDown(KeyCode.Tab);
    }

    public bool CloseScoreboard()
    {
        return Input.GetKeyUp(KeyCode.Tab);
    }

}
