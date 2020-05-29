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
        charAttack = new CharacterAttack(damage, range, transform);
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
        Granade myGranade = charAttack.ThrowGranade(grenadePos.position, grenadePos.rotation, prefabGrenade);
        currentRewindable = myGranade;
    }

    void CooldownRewind()
    {
    }

    public void TakeDamage(float dmg)
    {
        if (!photonView.IsMine) return;
        currentHP -= dmg;
        Debug.Log(currentHP + "Recibi daño");
        onUI.UpdateLifeText(currentHP);
        if(currentHP <= 0)
        {
            Die();
        }
    }

    [PunRPC]
    void ReceiveDamage(float dmg)
    {
    }

    public void Die()
    {
        isDead = true;
        anim.SetTrigger("Die");
    }
}
