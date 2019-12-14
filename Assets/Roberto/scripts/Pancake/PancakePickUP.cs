using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PancakePickUP : MonoBehaviour
{

    bool cantake = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Helper")
        {
            if (!cantake) return;
            KitchenHelper helper= other.GetComponent<KitchenHelper>();
            if (other.GetComponent<FiniStateMachine>().currentState.stateType != stateType.cake) return;
            if (helper.haveMaterial) return;
            helper.reciveMaterial();
            PancakeSpawner.instance.RemovePancake(this.transform);
            Destroy(gameObject);
        }
       
    }

    public void Cantake()
    {
        cantake = true;
    }
}
