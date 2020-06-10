using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CharacterHead : MonoBehaviourPun
{
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

    [HideInInspector] public bool rewindInCD = false;
    bool rewinding;
    float currentRewindCD;
    [HideInInspector] public int kills;
    [HideInInspector] public int deaths;

    [HideInInspector] public Rewindable currentRewindable;

    CharacterAttack charAttack;
    PlayerAnimator anim;

    [SerializeField] GameObject[] modelsToHide;
    [SerializeField] GameObject myWeaponModel;

    [HideInInspector] public bool isDead = false;


    public UpdateOnUI onUI;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        charAttack = new CharacterAttack(damage, range, transform, myCam);
        anim = new PlayerAnimator(myAnim);
        rb = GetComponent<Rigidbody>();
        onUI = GetComponent<UpdateOnUI>();
        grenadeCount = 3;
        isDead = false;
        currentHP = maxHp;
        kills = deaths = 0;
    }

    public void TurnModel()
    {
        myCam.gameObject.SetActive(true);

        myWeaponModel.SetActive(true);

        foreach (var item in modelsToHide)
        {
            item.gameObject.SetActive(false);
        }
    }


    private void Update()
    {
        if (isDead) return;

        CooldownRewind();
    }



    public void Move(float velX, float velY)
    {
        Vector3 dir = (transform.right * velX + transform.forward * velY).normalized * speed;
        rb.velocity = dir;
        anim.SetHorizontal(velX);
        anim.SetVertical(velY);
    }

    public void Shoot()
    {
        if (currentRewindable)
        {
            StopCoroutine(currentRewindable.capturePosition);
            currentRewindable.ClearRewindable(); //primero stopeo el anterior
            currentRewindable.shouldBeCapturingPosition = false;
        }
        currentRewindable = charAttack.Shoot(weapon.position, prefabBullet); //despues asigno el nuevo
        currentRewindable.owner = this;
    }

    public void ThrowGrenadeAnim()
    {
        anim.SetTrigger("ThrowGrenade");
        grenadeCount--;
    }
    public void ThrowGrenade()
    {
        onUI.UpdateGrenades(grenadeCount);
        if (currentRewindable)
        {
            currentRewindable.ClearRewindable();
            StopCoroutine(currentRewindable.capturePosition);
            currentRewindable.shouldBeCapturingPosition = false;
        }
        currentRewindable = charAttack.ThrowGranade(grenadePos.position, myCam.transform.rotation, prefabGrenade);

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

            onUI.UpdateRewindCD(currentRewindCD, rewindCooldown);

            if (currentRewindCD <= 0)
            {
                rewindInCD = false;
            }
        }
    }

    public bool TakeDamage(float dmg)
    {
        currentHP -= dmg;
        onUI.UpdateLifeText(currentHP);
        if (currentHP <= 0 && !isDead)
        {
            Die();
            return true;
        }
        else return false;
    }


    bool Die()
    {
        isDead = true;
        anim.SetTrigger("Die");
        StartCoroutine(Respawner());
        return isDead;

    }

    IEnumerator Respawner()
    {
        yield return new WaitForSeconds(2f);
        Respawn();
    }
    void Respawn()
    {
        currentHP = maxHp;
        onUI.UpdateLifeText(currentHP);
        isDead = false;
        anim.Respawn();
        PlayerInstantiator.Instance.RequestRespawn(PhotonNetwork.LocalPlayer);
    }
}
