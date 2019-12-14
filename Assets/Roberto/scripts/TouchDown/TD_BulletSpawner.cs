using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace td
{
    public class TD_BulletSpawner : MonoBehaviour
    {
        [SerializeField]
        protected WPbullet weaponBullet = null;
        [SerializeField]
        Transform spawnPoint = null;

        public Transform getSpawnpoint()
        {
            return spawnPoint;
        }

        [SerializeField]
        float bulletSpeed = 50;
        TD_PlayerControler p;
        Vector3 direction = new Vector3();

        

        public void INITIALIZING(TD_PlayerControler play)
        {
            p = play;
            p.BULLETSPAWNER = this;
            p.status.getCurrentweapon.bullet = weaponBullet;
            direction = transform.forward;
        }
      
      public void aiShot(Vector3 d)
        {
            direction = d;
            WPbullet newBullet = Instantiate(weaponBullet, spawnPoint.position, spawnPoint.transform.rotation);
            newBullet.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed);
        }
       public void shoot()
        {
            WPbullet newBullet = Instantiate(p.status.getCurrentweapon.bullet, spawnPoint.position, spawnPoint.transform.rotation);
            newBullet.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed);
        }
  
        public void normalShoot(Vector3 dir)
        {
            direction = dir * 50;
            WPbullet newBullet = Instantiate(weaponBullet, spawnPoint.position, spawnPoint.transform.rotation);
            newBullet.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed);
        }
        public void GiveDirection(Vector3 dir)
        {
            direction = dir;
        }
    }

}
