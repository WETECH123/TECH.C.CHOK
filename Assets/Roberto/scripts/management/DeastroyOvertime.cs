﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeastroyOvertime : MonoBehaviour
{
    public float timer = 2;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timer);
    }
    
}
