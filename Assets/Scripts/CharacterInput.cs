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

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void Start()
    {
        grenadeCount = myChar.grenadeCount;
        rewindInCD = myChar.rewindInCD;
        currentRewindable = myChar.currentRewindable;
        isDead = myChar.isDead;
    }


    public void Update()
    {
        if (!photonView.IsMine || isDead) return;
        if (Rewind() && !rewindInCD && currentRewindable)
        {
            PlayerInstantiator.Instance.RequestRewind(PhotonNetwork.LocalPlayer);
        }
        if (Shoot())
        {
            PlayerInstantiator.Instance.RequestShoot(PhotonNetwork.LocalPlayer);
        }
        if (ThrowGranade() && grenadeCount > 0)
        {
            PlayerInstantiator.Instance.RequestThrowGrenade(PhotonNetwork.LocalPlayer);
        }
        PlayerInstantiator.Instance.RequestMoveCamera(PhotonNetwork.LocalPlayer, HorizontalRotation(), VerticalRotation());
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine || isDead) return;

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

    
}
