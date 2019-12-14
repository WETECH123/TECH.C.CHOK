using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpicyBall : Bomb
{
    public override void Effect(Collider c, B_Player p)
    {
        EffectDirector.instance.playInPlace(transform.position, StaticStrings.SPICESKILL);
        IConfused confused = c.GetComponent<IConfused>();
        if (confused != null)
        {
            confused.confuse();
        }
    }
}
