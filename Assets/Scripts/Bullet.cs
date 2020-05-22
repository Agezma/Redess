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
        shouldBeCapturingPosition = true;
        StartCoroutine(DestroyMe());
    }

    private void Update()
    {
        
    }
    private void FixedUpdate()
    {
        addNewPos(transform.position, transform.rotation, rb.velocity);
    }


    IEnumerator DestroyMe()
    {
        yield return new WaitForSeconds(lifeSpan);
        Destroy(gameObject);
    }

}
