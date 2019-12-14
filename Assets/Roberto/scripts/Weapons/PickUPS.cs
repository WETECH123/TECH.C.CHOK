using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;

public class PickUPS : MonoBehaviour
{
    public itemType ItemType = itemType.bullet;
    [SerializeField]
    WPbullet bullet = null;
    [SerializeField]
    Sprite itemSprite = null;
    [SerializeField]
    int Ammo = 15;
    public Team tm;
   


    private void OnTriggerStay(Collider other)
    {
        Inputhandler inp = other.GetComponent<Inputhandler>();
        if (inp)
        {
            if (Input.GetButtonDown(StaticStrings.Circle_key))
            {
                take(other);
            }
        }
    }


    public void take(Collider other)
    {
        B_Player p = other.GetComponentInChildren<B_Player>();
        if (p == null) return;
        switch (ItemType)
        {
            case itemType.bomb:
                p.status.addBomb(1);
                EffectDirector.instance.playInPlace(transform.position, StaticStrings.PICKUPSTAR);
                break;
            case itemType.bullet:
                if (bullet != null)
                    p.BULLETSPAWNER.changeToSpecialShoot(Ammo, bullet);
                EffectDirector.instance.playInPlace(transform.position, StaticStrings.PICKUPDIAMOND);
                
                if (itemSprite != null)
                {
                    if (p.getCanvas() == null) return;
                    p.getCanvas().changeIngredientImage(itemSprite);
                }
                break;
            case itemType.ingredient:
                Team t = p.getTeam();
                if (t == tm)
                {
                    p.onTakingIngredient();
                    EffectDirector.instance.playInPlace(transform.position, StaticStrings.PICKUPSMILE);
                }
                else
                {
                    return;
                }
                break;
        }
        Destroy(gameObject);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CPUBrain>())
        {
            take(other);
        }
    }


}

public enum itemType
{
    bullet,
    bomb,
    ingredient
}