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
    [SerializeField] float rewindCooldown = 30f;
    [SerializeField] Transform weapon;
    [SerializeField] Bullet prefabBullet;
    [SerializeField] Camera myCam;

    [SerializeField] Granade prefabGrenade;
    [SerializeField] Transform grenadePos;
    [SerializeField] Animator myAnim;
    public int grenadeCount = 3;

    [SerializeField] float maxHp;
    public Color[] colors = new Color[2];
    public float currentHP { get; private set; }

    bool rewindInCD = false;
    bool rewinding;
    float currentRewindCD;
    [HideInInspector] public float kills;
    [HideInInspector] public float deaths;

    Rewindable currentRewindable;

    CharacterAttack charAttack;
    PlayerAnimator anim;

    [SerializeField] GameObject[] modelsToHide;
    [SerializeField] GameObject myWeaponModel;

    bool isDead = false;

    UpdateOnUI onUI;

    private void Awake()
    {
        if (photonView.IsMine)
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
        charAttack = new CharacterAttack(damage, range, transform, myCam);
        anim = new PlayerAnimator(myAnim);
        myController = new CharacterInput();
        rb = GetComponent<Rigidbody>();
        if(photonView.IsMine)
            myCam.gameObject.SetActive(true);
        onUI = GetComponent<UpdateOnUI>();

        isDead = false;
        currentHP = maxHp;
       
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        Move(myController.Horizontal(), myController.Vertical());

        CooldownRewind();

        if (myController.Rewind() && !rewindInCD && currentRewindable)
        {
            RewindTime();
        }
        if (myController.Shoot())
        {
            Shoot();
        }
        if(myController.ThrowGranade() && grenadeCount > 0)
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
        if (currentRewindable)
        {
            StopCoroutine(currentRewindable.capturePosition);
            currentRewindable.ClearRewindable(); //primero stopeo el anterior
            currentRewindable.shouldBeCapturingPosition = false;
        }

        currentRewindable = charAttack.Shoot(weapon.position, prefabBullet); //despues asigno el nuevo
    }

    public void ThrowGrenadeAnim()
    {
        anim.SetTrigger("ThrowGrenade");
    }
    public void ThrowGrenade()
    {
        if (currentRewindable)
        {
            currentRewindable.ClearRewindable(); 
            StopCoroutine(currentRewindable.capturePosition);
            currentRewindable.shouldBeCapturingPosition = false;
        }
        currentRewindable = charAttack.ThrowGranade(grenadePos.position, myCam.transform.rotation, prefabGrenade);
        grenadeCount--;
        onUI.UpdateGrenades(grenadeCount);
    }

    public void RewindTime()
    {
        StartCoroutine(currentRewindable.RewindTime());
        rewindInCD = true;
        currentRewindCD = rewindCooldown;
        anim.SetTrigger("Rewind");
    }

    void CooldownRewind()
    {
        if (rewindInCD)
        {
            currentRewindCD -= Time.deltaTime;

            onUI.UpdateRewindCD( currentRewindCD, rewindCooldown);

            if (currentRewindCD <= 0)
            {
                rewindInCD = false;
            }
        }
    }

    public bool TakeDamage(float dmg)
    {
        if (!photonView.IsMine) return false;
        photonView.RPC("ReceiveDamage", RpcTarget.AllBuffered, dmg);
        if (currentHP <= 0 && !isDead)
        {
            Die();
            return true;
        }
        else return false;
    }

    [PunRPC]
    void ReceiveDamage(float dmg)
    {
        currentHP -= dmg;
        onUI.UpdateLifeText(currentHP);
        
    }

    bool Die()
    {
        isDead = true;
        anim.SetTrigger("Die");
        return isDead;
    }
}
