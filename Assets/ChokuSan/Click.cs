using UnityEngine;
using System.Collections;

public class Click : MonoBehaviour
{

    public GameObject Enemy1;
    public GameObject Bomb;

    public float Enemy;
    public float BombLeft = 0;
    public float radius = 5f;
    public float force = 70f;

    public bool Bomb_Attack;
    public bool Normal_Attack;


    private void Start()
    {
        Bomb_Attack = false;
        Normal_Attack = true;
        Enemy = LayerMask.NameToLayer("Enemy1");
    }

    void NormalAttack()
    {

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.layer == Enemy)
            {
                Enemy1.GetComponent<ADV_EnemyHealth>().HP -= 10;
                if (Enemy1.GetComponent<ADV_EnemyHealth>().HP <= 0)
                {
                    Destroy(Enemy1.gameObject);
                }
            }
        }
    }

    void BombAttack()
    {
        RaycastHit hit;  
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.layer == Enemy)
            {
                Debug.Log("debug");
                foreach (Collider nearbyObject in colliders)
                {
                    Debug.Log("debug1");
                    Rigidbody rb_o = nearbyObject.GetComponent<Rigidbody>();
                    if (rb_o != null)
                    {
                        rb_o.AddExplosionForce(force, transform.position, radius);
                        Debug.Log("Bomb!");
                    }
                }
            }
        }


        /*       
        GameObject Bomb_1 = Instantiate(Bomb)as GameObject;
        Rigidbody rb = Bomb_1.GetComponent<Rigidbody>();
        Vector3 worldDir = ray.direction;
        Bomb_1.GetComponent<Bomb_Controller>().Shoot(worldDir.normalized * 5000);
        //rb.AddForce(transform.forward * )  
        */
    }
    void Update()
    {
        if (Normal_Attack == true && Input.GetButtonDown("Fire1"))
        {
            NormalAttack();
        }
        else if (Bomb_Attack == true && Input.GetButtonDown("Fire1"))
        {
            BombAttack();
            BombLeft -= 1;
        }
        else if (Bomb_Attack == false && BombLeft >=1 && Input.GetKeyDown("g"))
        {
            Bomb_Attack = true;
            Normal_Attack = false;
            
        }
        else if(Bomb_Attack == true && Input.GetKeyDown("g"))
        {
            Bomb_Attack = false;
            Normal_Attack = true;
        }
        if(BombLeft <= 0)
        {
            Bomb_Attack = false;
            Normal_Attack = true;
        }
    }
}