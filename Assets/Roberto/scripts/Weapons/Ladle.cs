using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladle : MeleeWeapon
{
    [SerializeField]
    float damage = 3;
    Team enemyteam=Team.chocolate;
    Rigidbody rb;
    
    public override void Initialize(B_Player p)
    {
        base.Initialize(p);
       
    }
    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag == StaticStrings.helper || other.tag == StaticStrings.player
            || other.tag == StaticStrings.tower)
        {
            rb = other.GetComponent<Rigidbody>();
            enemyteam = other.GetComponent<ITeam>().getTeam();

            if (enemyteam != tm)
            {
                IHealth h = other.GetComponent<IHealth>();
                h.takeDamage(damage);
               EffectDirector.instance.playInPlace(transform.position, StaticStrings.tornado);
                if (rb == null) return;
                rb.AddForce(new Vector3(200, 800, 800));
                Invoke("stop", 5);
                
            }
            else
            {
              FiniStateMachine  SM = other.GetComponent<FiniStateMachine>();
                if (SM)
                {
                    SM.becomeInvincible(true);
                    
                }
            }
            if (Soundmanager.instance == null) return;
            Soundmanager.instance.PlaySeByName(StaticStrings.wind);

        }
    }
    public void stop()
    {
        EffectDirector.instance.stopParticle(StaticStrings.tornado);
    }
   
}
