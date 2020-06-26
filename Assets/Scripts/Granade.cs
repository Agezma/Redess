using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : Rewindable
{
    [SerializeField] float lifeSpan = 5f;
    [SerializeField] float minRange = 3;
    [SerializeField] float maxRange = 8f;

    [SerializeField] float explosionDamage = 5f;

    [SerializeField] ParticleSystem explotion;
    [SerializeField] ParticleSystem impulse;
    [SerializeField] ParticleSystem rewindParticles;
    [SerializeField] ParticleSystem rewindParticles2;

    IEnumerator explosion;
    float explosionTimer;

    bool hasExploded = false;

    public override void Start()
    {
        base.Start();
               
        explosionTimer = lifeSpan;
        explosion = Explode(explosionTimer);
        StartCoroutine(explosion);        
    }

    private void Update()
    {
        if (!hasExploded && !backingTime)
        {
            explosionTimer -= Time.deltaTime;
        }
    }

    public IEnumerator Explode(float time)
    {
        yield return new WaitForSeconds(time);
        Explosion();
    }
    public void Explosion()
    {
        //StopCoroutine(capturePosition);
        hasExploded = true;
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

    public override IEnumerator RewindTime()
    {
        if (backingTime) yield break;

        rewindParticles2.Play();
        StopCoroutine(explosion);
        shouldBeCapturingPosition = false;
        backingTime = true;
        rewindParticles.Play();
        float aux = timeToRewind / myPos.Count;
        for (int i = 0; i < myPos.Count; i++)
        {
            transform.position = myPos[myPos.Count - i - 1];
            transform.rotation = myRot[myRot.Count - i - 1];
            rb.velocity = Vector3.zero;
            yield return new WaitForSeconds(aux);
        }
        rb.velocity = myVel[0];
        /*for (int i = 0; i < mypos.Count; i++)
        {
            transform.position = mypos[i];

            yield return new WaitForSeconds(aux);
        }*/
        StartCoroutine(Explode(explosionTimer + myPos.Count*timeBetweenSaves));
        myPos.Clear();
        myRot.Clear();
        myVel.Clear();
        rewindParticles.Stop();
        backingTime = false;
        shouldBeCapturingPosition = true;
        //rewindParticles.gameObject.SetActive(false);
    }
}
