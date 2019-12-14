using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowMan : MonoBehaviour
{
    [SerializeField]
    int maxMovement = 6;
    [SerializeField]
    ParticleSystem snow=null;
    int movement = 0;
    GameObject objectToFreez = null;
    bool go = false;
    bool ai=false;
    float frozeTimer = 1.5f;
    float scrollingTime = 1.5f;
    void Update()
    {
        if (ai)
        {
            automaticScroll();
        }
        else
        {
            if (Input.GetButtonDown(StaticStrings.X_key))
            {
                addMovement();
            }
        }
    }

    void addMovement()
    {
        movement++;
        GetComponent<Animator>().SetTrigger(StaticStrings.Scroll);
        snow.Play();
        if (movement > maxMovement)
        {
            if (!go)
            {
                go = true;
                ripristing();
            }
            
        }
    }

    public void objectToFreeze(GameObject obj,bool isAI)
    {
        objectToFreez = obj;
        objectToFreez.SetActive(false);
        ai = isAI;
    }
    private void ripristing()
    {
        if(objectToFreez!=null)
        objectToFreez.SetActive(true);
        FiniStateMachine sm = objectToFreez.GetComponent<FiniStateMachine>();
        if (sm != null) { sm.changeState(stateType.cake); }
        Destroy(this.gameObject);
    }

    public void automaticScroll()
    {
        frozeTimer -= Time.deltaTime;
        if (frozeTimer <= 0)
        {
            frozeTimer = scrollingTime;
            addMovement();
            
        }
    }
}
