using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Bullet : Rewindable
{
    [SerializeField] float speed = 5;
    [SerializeField] float lifeSpan = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rb.velocity = transform.forward * speed;
        StartCoroutine(DestroyMe());
        
        shouldBeCapturingPosition = true;
        capturePosition = addPositionAlways();
        StartCoroutine(capturePosition);
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
