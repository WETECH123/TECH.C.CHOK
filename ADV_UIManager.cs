using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ADV_UIManager : MonoBehaviour
{
    public GameObject a;
    public GameObject b;
    public GameObject c;
    public float life = 3;
    // Start is called before the first frame update
    void Start()
    {
        life = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (life == 3)
            {
                life -= 1;
                a.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 0 / 255);
            }
            else if (life == 2)
            {
                life -= 1;
                b.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 0 / 255);
            }
            else if (life == 1)
            {
                life -= 1;
                c.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 0 / 255);
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (life == 2)
            {
                life += 1;
                a.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255 / 255);
            }
            else if (life == 1)
            {
                life += 1;
                b.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255 / 255);
            }   
        }
    }
}
