using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectManager : MonoBehaviour
{
    public float time = 180;
    public float p_time;
    public float q_a = 1f;
    public float q = 0;
    public GameObject AttackEffect;
    // Start is called before the first frame update
    void Start()
    {
        p_time = 0;
        q_a = 1f;
        time = 180;
        AttackEffect.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 0 / 255);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("a")) {
            q = 255f;

            AttackEffect.GetComponent<Image>().color = new Color(255f / 255f, 0f / 255f, 0f / 255f, q / 255);
            time = 180;
            p_time = 1;
        }
        if (!Input.GetKeyDown("a")) {
            time -= p_time;
            if (time < 0) {
                p_time = 0;
            }
        }
        if (time<= 0f && !Input.GetKeyDown("a")) {         
            q -= q_a;
            q_a = 1f;
            AttackEffect.GetComponent<Image>().color = new Color(255f / 255f, 0f / 255f, 0f / 255f, q / 255);
            if (q <= 0) {
                q_a = 0;
            }
        }
        
            //Debug.Log("q");
        
    }
}
