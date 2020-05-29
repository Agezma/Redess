using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CharacterAttack
{
    float damage;
    float range;
    Transform myPos;

    public CharacterAttack(float dmg, float rng, Transform pos)
    {
        damage = dmg;
        range = rng;
        myPos = pos;
    }

    public Bullet Shoot(Vector3 pos, Quaternion rot, Bullet prefabBullet)
    {

        RaycastHit hit;
        Vector3 ray = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 100));

        //Vector3 dir = ray - transform.position;

        if (Physics.Raycast(myPos.position, myPos.forward,out hit))
         {
            //dir = hit.point - transform.position;
             if (hit.collider.gameObject.GetComponent<CharacterHead>())
             {
                 CharacterHead enemy = hit.collider.GetComponent<CharacterHead>();
                enemy.TakeDamage(damage);
             }
         }
        Debug.DrawRay(myPos.position, myPos.forward);
        GameObject myBullet = PhotonNetwork.Instantiate(prefabBullet.name, pos, rot);

        return myBullet.GetComponent<Bullet>();
    }

    public Granade ThrowGranade(Vector3 pos, Quaternion rot, Granade prefabGranade)
    {
        GameObject myGrenade = PhotonNetwork.Instantiate(prefabGranade.name, pos, rot);
        return myGrenade.GetComponent<Granade>(); ;
    }
}
