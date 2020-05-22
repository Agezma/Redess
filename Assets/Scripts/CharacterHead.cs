using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CharacterHead : MonoBehaviourPun
{
    IController myController;
    [HideInInspector]
    public Rigidbody rb;
    
    [SerializeField] float speed;
    [SerializeField] float jumpForce;

    [SerializeField] float damage;
    [SerializeField] float range;
    [SerializeField] float rewindCooldown;
    [SerializeField] Transform weapon;
    [SerializeField] Bullet prefabBullet;
    [SerializeField] Camera myCam;

    [SerializeField] Granade prefabGrenade;
    [SerializeField] Transform grenadePos;
    [SerializeField] Animator myAnim;

    [SerializeField] float maxHp;
    float currentHP;

    bool rewindInCD = false;
    bool rewinding;
    float currentRewindCD;

    Rewindable currentRewindable;

    CharacterAttack charAttack;
    PlayerAnimator anim;

    [SerializeField] GameObject[] modelsToHide;
    [SerializeField] GameObject myWeaponModel;

    private void Awake()
    {
        if (PhotonNetwork.LocalPlayer.IsLocal)
        {
            foreach (var item in modelsToHide)
            {
                item.gameObject.SetActive(false);
            }
            myWeaponModel.SetActive(true);
        }   
    }

    void Start()
    {
        charAttack = new CharacterAttack(damage, range, transform);
        anim = new PlayerAnimator(myAnim);
        myController = new CharacterInput();
        rb = GetComponent<Rigidbody>();
        if(photonView.IsMine)
            myCam.gameObject.SetActive(true);
       
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        Move(myController.Horizontal(), myController.Vertical());

        if (myController.Rewind() && !rewindInCD)
        {
            StartCoroutine(currentRewindable.RewindTime());
            rewindInCD = true;
            currentRewindCD = rewindCooldown;
            anim.SetTrigger("Rewind");
        }
        if (myController.Shoot())
        {
            Shoot();
        }
        if(myController.ThrowGranade())
        {
            ThrowGrenadeAnim();
        }
    }


    void Move(float velX, float velY)
    {
        Vector3 dir = (transform.right * velX + transform.forward * velY).normalized * speed;
        rb.velocity = dir;
        anim.SetHorizontal(velX);
        anim.SetVertical(velY);
    }    

    void Shoot()
    {
        charAttack.Shoot(weapon.position, weapon.rotation , prefabBullet);
        anim.SetTrigger("Shoot");
    }

    public void ThrowGrenadeAnim()
    {
        anim.SetTrigger("ThrowGrenade");
    }
    public void ThrowGrenade()
    {
        Granade asd = charAttack.ThrowGranade(grenadePos.position, grenadePos.rotation, prefabGrenade);
        currentRewindable = asd;
    }

    void CooldownRewind()
    {
    }

    public void TakeDamage(float dmg)
    {
        photonView.RPC("ReceiveDamage", RpcTarget.AllBuffered, dmg);
    }

    [PunRPC]
    void ReceiveDamage(float dmg)
    {
        currentHP -= dmg;
    }
}
