using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientsSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject[] ingredients=null;
    [SerializeField]
    float spawningTime = 20;
    float spawningCounter = 0;
    [SerializeField]
    Transform topLeft=null,bottomRight=null;
    float x=0, y=0, z=0;

    void Start()
    {
        spawningCounter = spawningTime;
    }

    // Update is called once per frame
    void Update()
    {
        spawningCounter -= Time.deltaTime;
        if (spawningCounter <= 0)
        {
            spawningCounter = spawningTime;
            int rand = Random.Range(0, ingredients.Length);
            
            x = Random.Range(topLeft.transform.position.x, bottomRight.transform.position.x);
            y = Random.Range(bottomRight.transform.position.z, topLeft.transform.position.z);
            Vector3 pos = new Vector3(x, 2.5f, z);
            Instantiate(ingredients[rand], pos, Quaternion.identity);
        }
    }
}
