using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using td;
public class BulletSpawner : MonoBehaviour
{
    [SerializeField]
    protected WPbullet weaponBullet = null;
    [SerializeField]
    Transform spawnPoint=null;
    public Sprite weaponImage = null;
   public AnimatorOverrideController controller;
   public  Transform getSpawnpoint()
    {
        return spawnPoint;
    }

    [SerializeField]
    float bulletSpeed = 50;
    B_Player p;
    Vector3 direction=new Vector3();
    int specialBullet = 0;
    public delegate void usingDifferentBullet();
   public usingDifferentBullet bulletSpawning;

    public void INITIALIZING(B_Player play)
    {
        p = play;
        p.BULLETSPAWNER = this;
        direction = transform.forward;
        bulletSpawning = shoot;

    }
  
    public void changeAnimator()
    {
        p.overrideAnimator(controller);
        if (weaponBullet == null) return;
        p.status.getCurrentweapon.changeBullet(this.weaponBullet);
      
    }
    public void shot()
    {
        bulletSpawning();
    }

    void shoot()
    {
        WPbullet newBullet = Instantiate(p.status.getCurrentweapon.bullet, spawnPoint.position,spawnPoint.transform.rotation );
        newBullet.passAttacker(p);
       
        newBullet.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed);

    }
    public void changeToSpecialShoot(int value, WPbullet bullet)
    {
        p.setBullet(bullet);
        specialBullet = value;
        bulletSpawning = specialShoot;
    }

    public void specialShoot()
    {
        WPbullet newBullet = Instantiate(p.status.getCurrentweapon.bullet, spawnPoint.position, spawnPoint.transform.rotation);
        newBullet.passAttacker(p);
        newBullet.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed);
        specialBullet--;
        if(specialBullet<=0)
        {
            specialBullet = 0;
            bulletSpawning = shoot;
            p.setBullet(p.getDefaultBullet());
            if (p.getCanvas() == null) return;
            p.getCanvas().resetIngredientImage();
        }
    }
    public void GiveDirection(Vector3 dir)
    {
        direction = dir;
    }
}
