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
         if (Physics.Raycast(myPos.position, myPos.forward,out hit))
         {
             if (hit.collider.gameObject.GetComponent<CharacterHead>())
             {
                 CharacterHead enemy = hit.collider.GetComponent<CharacterHead>();
                enemy.TakeDamage(damage);
             }
         }

        GameObject myBullet = PhotonNetwork.Instantiate(prefabBullet.name, pos, rot);

        return myBullet.GetComponent<Bullet>();
    }

    public Granade ThrowGranade(Vector3 pos, Quaternion rot, Granade prefabGranade)
    {
        GameObject myGrenade = PhotonNetwork.Instantiate(prefabGranade.name, pos, rot);
        return myGrenade.GetComponent<Granade>(); ;
    }
}
