using UnityEngine;
using System.Collections;

public class Click : MonoBehaviour
{

    public GameObject Enemy1;
    public GameObject Bomb;

    public float Enemy;
    public float BombLeft = 0;

    public bool Bomb_Attack;


    private void Start()
    {
        Bomb_Attack = true;
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        GameObject Bomb_1 = Instantiate(Bomb)as GameObject;
        Rigidbody rb = Bomb_1.GetComponent<Rigidbody>();
        Vector3 worldDir = ray.direction;
        Bomb_1.GetComponent<Bomb_Controller>().Shoot(worldDir.normalized * 5000);
        //rb.AddForce(transform.forward * )
    }
    void Update()
    {
        if (Bomb_Attack == false && Input.GetButtonDown("Fire1"))
        {
            NormalAttack();
        }
        else if (Bomb_Attack == true && Input.GetButtonDown("Fire1"))
        {
            BombAttack();
        }
    }
}