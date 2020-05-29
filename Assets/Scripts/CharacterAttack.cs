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
    Camera myCam;

    public CharacterAttack(float dmg, float rng, Transform pos, Camera cam)
    {
        damage = dmg;
        range = rng;
        myPos = pos;
        myCam = cam;
    }

    public Bullet Shoot(Vector3 pos, Bullet prefabBullet)
    {

        RaycastHit hit;
        Vector3 ray = myCam.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 100));

        Vector3 dir = ray - myPos.position;

        if (Physics.Raycast(pos, ray - myCam.transform.position,out hit))
         {
             dir = hit.point - pos;
             if (hit.collider.gameObject.GetComponent<CharacterHead>())
             {
                CharacterHead enemy = hit.collider.GetComponent<CharacterHead>();
                enemy.TakeDamage(damage);
             }
         }
        Debug.DrawRay(pos , dir,Color.black, 2f);
        GameObject myBullet = PhotonNetwork.Instantiate(prefabBullet.name, pos, Quaternion.Euler(dir));
        myBullet.transform.forward = dir;

        return myBullet.GetComponent<Bullet>();
    }

    public Granade ThrowGranade(Vector3 pos, Quaternion rot, Granade prefabGranade)
    {
        GameObject myGrenade = PhotonNetwork.Instantiate(prefabGranade.name, pos, rot);
        return myGrenade.GetComponent<Granade>(); ;
    }
}
