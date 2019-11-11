using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombeffect : MonoBehaviour
{
    public GameObject player;
    public void BombShoot(Vector3 dir)
    {
        GetComponent<Rigidbody>().AddForce(dir);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision player)
     {
        if (player.gameObject.CompareTag("bomb"))
        {
            BombShoot(new Vector3(0, 200, 200));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
