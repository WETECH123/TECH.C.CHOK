using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlourBullet : WPbullet
{
    
    public override void Effect(Collider c, B_Player p)
    {
        int rnd = Random.Range(1, 11);
        if (rnd >= 9)
        {
            Iflour f = c.GetComponent<Iflour>();
            if (f == null) return;
            f.activeFlour();
        }
    }
}
