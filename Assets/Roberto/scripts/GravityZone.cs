using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityZone : MonoBehaviour
{

    float force = 10;
    [SerializeField]
    float s = 10;
    Vector3 size = new Vector3(10, 10, 10);
    BoxCollider coll = null;
    Vector3 direction = Vector3.zero;
    [SerializeField]
    float lifetime = 5;
    void Start()
    {
        coll = GetComponent<BoxCollider>();
        size = new Vector3(s, s, s);
        coll.size = size;
        Destroy(gameObject, lifetime);
    }
   
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, size);
    }

    private void OnTriggerEnter(Collider other)
    {
        actrat(other);
    }
    private void OnTriggerStay(Collider other)
    {
        actrat(other);
    }

    void actrat(Collider c)
    {
        if (c.tag == StaticStrings.player || c.tag == StaticStrings.helper
           || c.tag == StaticStrings.AI||c.tag==StaticStrings.cpu||c.tag==StaticStrings.bullet)
        {
            Rigidbody rb = c.GetComponent<Rigidbody>();
            if (rb != null)
            {
                actractiveForce(c.transform.position, rb);

            }
        }
    }
    public void actractiveForce(Vector3 enemypos,Rigidbody rb)
    {
        direction = transform.position- enemypos ;
        rb.velocity = direction * force;
    }
}
