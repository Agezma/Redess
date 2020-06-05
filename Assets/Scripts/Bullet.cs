using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Bullet : Rewindable
{
    [SerializeField] float lifeSpan = 5f;
    
    public override void Start()
    {
        base.Start();

        StartCoroutine(DestroyMe());        
    }

    IEnumerator DestroyMe()
    {
        yield return new WaitForSeconds(lifeSpan);
        DestroyObj();
    }

    void DestroyObj()
    {
        StopCoroutine(capturePosition);
        shouldBeCapturingPosition = false;
        //Destroy(gameObject);
    }

}
