using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDirector : MonoBehaviour
{
    public static EffectDirector instance;
    Dictionary<string,Transform> particles = new Dictionary<string,Transform>();
    [SerializeField]
    ParticleSystem[] effects=null;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
       foreach(var item in effects)
        {
           particles.Add(item.gameObject.name, item.transform);
        }
    }
    public void stopParticle(string name)
    {
        if (particles.ContainsKey(name))
        {
          particles[name].GetComponent<ParticleSystem>().Stop();
        }
        else
        {
            Debug.Log("particle is can't finded");
        }
    }
    public void playInPlace(Vector3 place,string name)
    {
        if (particles.ContainsKey(name))
        {
            particles[name].position = place;
            particles[name].GetComponent<ParticleSystem>().Play();

        }
        else
        {
            Debug.Log("particle is can't finded");
        }
    }
}
