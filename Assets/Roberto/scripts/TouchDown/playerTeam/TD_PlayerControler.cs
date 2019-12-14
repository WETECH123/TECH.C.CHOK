using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace td
{
    public class TD_PlayerControler : TD_Character
    {
        
        float fireRate = 0.2f;
        float fireCounter = 0.2f;
        public STATUS status = null;
        TD_Ally ally;
        Camera c;
        public TD_BulletSpawner BULLETSPAWNER { get; set; }
       

        TD_canvas canvas = null;
        
        #region returnValues
        public bool L2_input()
        {
            return Input.GetButton(StaticStrings.L2_key);
        }
        public TD_Ally getAlly()
        {
            return ally;
        }
        

        public void resetStatus()
        {
            respawning();
            canMove = true;
        }
        bool R2Input()
        {

            return Input.GetButton(StaticStrings.R2_key);
        }
        bool SquareInput()
        {
            return Input.GetButtonDown(StaticStrings.Square_key);
        }
        #endregion


        #region initialing  
      
        public override void Init()
        {
            BULLETSPAWNER = GetComponentInChildren<TD_BulletSpawner>();
            canvas = GetComponentInChildren<TD_canvas>();
            status = new STATUS();
            TD_GameManager gm = FindObjectOfType<TD_GameManager>();
            gm.setResetableObjects(this.gameObject);
            ally = FindObjectOfType<TD_Ally>();
            GameObject door = GameObject.FindGameObjectWithTag("door");
            door.GetComponent<TD_PointZone>().tm = this.team;
            c = GameObject.FindGameObjectWithTag("FollowCamera").GetComponent<Camera>();
            BULLETSPAWNER.INITIALIZING(this);
            anim = GetComponent<Animator>();
            base.Init();
        }
        #endregion

        public override void updating()
        {
            base.updating();
            targetShoot();
            canvas.activeCrossAir(L2_input());
            if (R2Input())
            {
                fire();
            }
            if (SquareInput())
            {
                tossBomb();
            }
        }
        private void tossBomb()
        {
            if (status.BOMB < 1) { return; }
            status.BOMB--;
            anim.SetTrigger(StaticStrings.bomb);
        }

        public void targetShoot()
        {
            Vector3 dir;
            if (L2_input())
            {
                dir = getCameraDir();
            }
            else dir = transform.forward * 50;
            BULLETSPAWNER.GiveDirection(dir);
        }
        public Vector3 getCameraDir()
        {
            Vector3 cameradir = c.transform.forward * status.getCurrentweapon.bulletRange;
            Vector3 dir = new Vector3(cameradir.x, cameradir.y + 10, cameradir.z);
            return dir;
        }
        public void shot()
        {
            BULLETSPAWNER.shoot();
        }
        void fire()
        {
            fireRate -= Time.deltaTime;
            if (fireRate <= 0)
            {
                fireRate = fireCounter;
                Target_and_Shoot();
            }

        }
        void Target_and_Shoot()
        {
            targetShoot();
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName(StaticStrings.shooting) && !anim.IsInTransition(0))
                anim.SetTrigger(StaticStrings.shooting);
        }

     
    }
}

