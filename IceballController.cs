using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceballController : MonoBehaviour
{
   

    public GameObject iceball;
    public void ice(Vector3 dir)
    {
        GetComponent<Rigidbody>().AddForce(dir);
        transform.parent = GameObject.Find("target").transform;　//プレイヤーのchildrenになる
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {       
        Destroy(gameObject,5f);       
    }
}
