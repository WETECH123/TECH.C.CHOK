using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADV_Player : MonoBehaviour
{
    public float speed = 10f;

    private Transform target;
    private int wavepointIndex = 0;

     void Start()
    {
        target = ADV_Waypoints.points[0];        
    }

     void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position,target.position)<= 0.2f)
        {
            GetNexWaypoint();
        }
    }

    void GetNexWaypoint()
    {
        if (wavepointIndex >= ADV_Waypoints.points.Length -1)
        {
            //Destroy(gameObject);
        }
        wavepointIndex++;
       target = ADV_Waypoints.points[wavepointIndex];
    }
   
}
