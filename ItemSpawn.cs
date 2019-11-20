using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    public GameObject a;
    public GameObject b;
    public GameObject c;
    public GameObject d;
    public GameObject e;
    public GameObject f;
    public float Time_Spawn =5;
    public float CD = 10;
    // Start is called before the first frame update
    void Start()
    {

    }

    void Item_spawn()
    {
        CD += 1;
        if (CD >=60)
        {
            CD = 0;
            Time_Spawn -= 1;
            if (Time_Spawn <= 0)
            {
                Instantiate(a, new Vector3(Random.Range(-100, 100), Random.Range(0, 100), Random.Range(-100, 100)), Quaternion.identity);
                Instantiate(b, new Vector3(Random.Range(-100, 100), Random.Range(0, 100), Random.Range(-100, 100)), Quaternion.identity);
                Instantiate(b, new Vector3(Random.Range(-100, 100), Random.Range(0, 100), Random.Range(-100, 100)), Quaternion.identity);
                Instantiate(d, new Vector3(Random.Range(-100, 100), Random.Range(0, 100), Random.Range(-100, 100)), Quaternion.identity);
                Instantiate(e, new Vector3(Random.Range(-100, 100), Random.Range(0, 100), Random.Range(-100, 100)), Quaternion.identity);
                Instantiate(f, new Vector3(Random.Range(-100, 100), Random.Range(0, 100), Random.Range(-100, 100)), Quaternion.identity);
                Time_Spawn = 5;
                Time_Spawn = 5;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Item_spawn();
    }
}
