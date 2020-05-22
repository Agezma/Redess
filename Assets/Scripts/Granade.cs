using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : Rewindable
{
    [SerializeField] float speed = 5;
    [SerializeField] float lifeSpan = 5f;
    [SerializeField] float explosionRange = 5;
    [SerializeField] float explosionDamage = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        shouldBeCapturingPosition = true;
    }

    private void FixedUpdate()
    {
        addNewPos(transform.position, transform.rotation, rb.velocity);
        
    }

    public IEnumerator Explode()
    {
        yield return new WaitForSeconds(3f);
        Explosion();
    }
    public void Explosion()
    {
        Collider[] myCols = Physics.OverlapSphere(transform.position, explosionRange);

        foreach (var item in myCols)
        {
            if (item.GetComponent<CharacterHead>())
            {
                CharacterHead current = item.GetComponent<CharacterHead>();
                current.TakeDamage(( 1 - (Vector3.Distance(item.transform.position, transform.position) / explosionRange)) * explosionDamage );
                Vector3 force = item.transform.position - transform.position;
                current.rb.AddForce(new Vector3(force.x, force.y / 5, force.z));
            }
        }
    }
}
