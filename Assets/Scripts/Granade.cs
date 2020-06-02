using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : Rewindable
{
    [SerializeField] float speed = 5;
    [SerializeField] float lifeSpan = 5f;
    [SerializeField] float minRange = 3;
    [SerializeField] float maxRange = 8f;

    [SerializeField] float explosionDamage = 5f;

    public ParticleSystem explotion;
    public ParticleSystem impulse;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;

        StartCoroutine(Explode());
        
        shouldBeCapturingPosition = true;
        capturePosition = addPositionAlways();
        StartCoroutine(capturePosition);
    }

    public IEnumerator Explode()
    {
        yield return new WaitForSeconds(3f);
        Explosion();
    }
    public void Explosion()
    {
        //StopCoroutine(capturePosition);
        Collider[] myCols = Physics.OverlapSphere(transform.position, maxRange);
        explotion.Play();
        impulse.Play();
        foreach (var item in myCols)
        {
            if (item.GetComponent<CharacterHead>())
            {
                CharacterHead current = item.GetComponent<CharacterHead>();
                var dist = Vector3.Distance(item.transform.position, transform.position);
                if (dist < minRange)
                {
                    current.TakeDamage((int)explosionDamage);
                }
                else
                {
                    current.TakeDamage((int)(( 1 - ((dist - minRange) / (maxRange-minRange))) * explosionDamage ));                   
                }

                Vector3 force = item.transform.position - transform.position;
                current.rb.AddForce(new Vector3(force.x, force.y / 5, force.z));
            }
        }
    }
}
