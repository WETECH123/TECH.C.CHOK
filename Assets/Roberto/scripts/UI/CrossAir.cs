using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossAir : MonoBehaviour
{
   // float maxSpread = 80;
    float spreadSpeed = 5;
    float targetSpread = 30;
    float t;
    float curSpeed;
    public RectTransform[] part_s;
    Vector2[] positions = new Vector2[4];

    private void Start()
    {
        positions[0] = new Vector2(0,1);
        positions[1] = new Vector2(0,-1);
        positions[2] = new Vector2(-1,0);
        positions[3] = new Vector2(1,0);
    }
    public void Update() 
    {
        t = Time.deltaTime * spreadSpeed;
        curSpeed = Mathf.Lerp(curSpeed, targetSpread, t);
        for (int i = 0; i < part_s.Length; i++)
        {
           RectTransform p = part_s[i];
            p.anchoredPosition = positions[i] * curSpeed;
        }
    }
}
